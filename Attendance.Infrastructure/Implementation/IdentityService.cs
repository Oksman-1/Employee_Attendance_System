using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto.Auth;
using Attendance.Application.Abstractions.Repositories;
using Attendance.Infrastructure.Identity;
using Attendance.Shared.GenericResponse;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Attendance.Infrastructure.Implementation;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<IdentityService> _logger;
    private readonly IEmployeeRepository _employeeRepository;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ILogger<IdentityService> logger,
        IEmployeeRepository employeeRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
        _employeeRepository = employeeRepository;
    }

    public async Task<GenericResponse<string>> CreateUserAsync(string email, string password, string role)
    {
        _logger.LogInformation("Creating identity user for {Email} with role {Role}", email, role);

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return GenericResponse<string>.Duplicate($"User with email {email} already exists in identity system.");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return GenericResponse<string>.InternalError($"Failed to create identity user: {errors}");
        }

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }

        await _userManager.AddToRoleAsync(user, role);

        return GenericResponse<string>.Success("User created successfully", user.Id, "201");
    }

    public async Task<GenericResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request)
    {
        _logger.LogInformation("Login attempt for {Email}", request.Email);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return GenericResponse<AuthResponseDto>.Unauthorized("Invalid email or password");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return GenericResponse<AuthResponseDto>.Unauthorized("Invalid email or password");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Employee";

        var token = GenerateJwtToken(user, role);

        var authResponse = new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            FullName = "", // FullName is in Employee record, the controller or client can handle this.
            Role = role
        };

        return GenericResponse<AuthResponseDto>.Success("Login successful", authResponse, "200");
    }

    public async Task<GenericResponse<AuthResponseDto>> LoginViaQrCodeAsync(string qrCode, string pin)
    {
        _logger.LogInformation("---------------Login via QR code attempt---------------");

        if (string.IsNullOrWhiteSpace(qrCode))
            return GenericResponse<AuthResponseDto>.BadRequest("QR Code cannot be empty");

        if (string.IsNullOrWhiteSpace(pin))
            return GenericResponse<AuthResponseDto>.BadRequest("PIN cannot be empty");

        var employee = await _employeeRepository.GetByCodeAsync(qrCode);
        if (employee == null)
            return GenericResponse<AuthResponseDto>.Unauthorized("Invalid QR Code or PIN");

        if (!employee.IsActive)
            return GenericResponse<AuthResponseDto>.Unauthorized("Employee is not active");

        var pinHash = Attendance.Shared.Common.HashHelper.ComputeSha256Hash(pin);
        if (employee.PinHash != pinHash)
            return GenericResponse<AuthResponseDto>.Unauthorized("Invalid QR Code or PIN");

        var user = await _userManager.FindByEmailAsync(employee.Email);
        if (user == null)
            return GenericResponse<AuthResponseDto>.Unauthorized("User account not found for this employee");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Employee";

        var token = GenerateJwtToken(user, role);

        var authResponse = new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            FullName = employee.FullName,
            Role = role
        };

        return GenericResponse<AuthResponseDto>.Success("Login successful", authResponse, "200");
    }

    public async Task SeedRolesAsync()
    {
        string[] roles = { "Admin", "Employee" };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private string GenerateJwtToken(ApplicationUser user, string role)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing"));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"] ?? "60")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task SeedDefaultAdminAsync()
    {
        var adminEmail = _configuration["DefaultAdmin:Email"] ?? "admin@company.com";
        var adminPassword = _configuration["DefaultAdmin:Password"] ?? "AdminPassword123!";
        var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
        
        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                _logger.LogInformation("Default admin user seeded successfully.");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to seed default admin user. Errors: {Errors}", errors);
            }
        }
    }

    public async Task<GenericResponse<string>> UpdateUserEmailAsync(string oldEmail, string newEmail)
    {
        var user = await _userManager.FindByEmailAsync(oldEmail);
        if (user == null)
            return GenericResponse<string>.NotFound("Identity user not found");

        user.Email = newEmail;
        user.UserName = newEmail;
        
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return GenericResponse<string>.Success("User email updated in Identity.", null, "200");

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return GenericResponse<string>.InternalError($"Failed to update user email: {errors}");
    }

    public async Task<GenericResponse<string>> ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return GenericResponse<string>.NotFound("User not found");

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (result.Succeeded)
            return GenericResponse<string>.Success("Password changed successfully", null, "200");

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return GenericResponse<string>.BadRequest($"Password change failed: {errors}");
    }

    public async Task<GenericResponse<string>> ChangePinAsync(string email, string currentPin, string newPin)
    {
        var employee = await _employeeRepository.GetByEmailAsync(email);
        if (employee == null)
            return GenericResponse<string>.NotFound("Employee record not found for this user.");

        var currentPinHash = Attendance.Shared.Common.HashHelper.ComputeSha256Hash(currentPin);
        if (employee.PinHash != currentPinHash)
            return GenericResponse<string>.BadRequest("Current PIN is incorrect.");

        var newPinHash = Attendance.Shared.Common.HashHelper.ComputeSha256Hash(newPin);
        employee.PinHash = newPinHash;

        await _employeeRepository.UpdateAsync(employee);

        return GenericResponse<string>.Success("PIN changed successfully.", null, "200");
    }

    public async Task<GenericResponse<string>> ResetPasswordAsync(string email, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return GenericResponse<string>.NotFound("User not found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded)
            return GenericResponse<string>.Success("Password reset successfully", null, "200");

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return GenericResponse<string>.BadRequest($"Password reset failed: {errors}");
    }

    public async Task<GenericResponse<string>> ResetPinAsync(string email, string newPin)
    {
        var employee = await _employeeRepository.GetByEmailAsync(email);
        if (employee == null)
            return GenericResponse<string>.NotFound("Employee record not found for this user.");

        var newPinHash = Attendance.Shared.Common.HashHelper.ComputeSha256Hash(newPin);
        employee.PinHash = newPinHash;

        await _employeeRepository.UpdateAsync(employee);

        return GenericResponse<string>.Success("PIN reset successfully.", null, "200");
    }
}
