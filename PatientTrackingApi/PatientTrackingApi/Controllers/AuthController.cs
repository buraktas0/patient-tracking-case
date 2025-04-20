using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientTrackingApi.Auth;
using PatientTrackingApi.Data.Repositories;
using PatientTrackingApi.Domain.Entities;
using PatientTrackingApi.Domain.Requests;
using PatientTrackingApi.Domain.Responses;

namespace PatientTrackingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserRepository userRepo;
    private readonly TokenRepository tokenRepo;
    private readonly TokenService tokenService;
    private readonly ILogger<AuthController> logger;

    public AuthController(
        UserRepository userRepo,
        TokenRepository tokenRepo,
        TokenService tokenService,
        ILogger<AuthController> logger)
    {
        this.userRepo = userRepo;
        this.tokenRepo = tokenRepo;
        this.tokenService = tokenService;
        this.logger = logger;
    }

    /// <summary>
    /// Logs in a user and returns a JWT token.
    /// </summary>
    /// <param name="request">Login request containing email and password.</param>
    /// <returns>JWT token if login is successful, otherwise an error message.</returns>
    /// <response code="200">Returns the JWT token.</response>
    /// <response code="401">Invalid credentials.</response>
    /// <response code="500">An error occurred while processing the request.</response>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResult>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await userRepo.GetByEmailAsync(request.Email);
            if (user is null) return Unauthorized("Invalid credentials");

            var hashed = PasswordHelper.HashPassword(request.Password, user.Salt);
            if (hashed != user.HashedPassword)
                return Unauthorized("Invalid credentials");

            var authResult = await CreateTokens(user);

            return Ok(authResult);
        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred while logging in the user." + ex.Message);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">Register request containing email and password.</param>
    /// <returns>Success message if registration is successful, otherwise an error message.</returns>
    /// <response code="200">User registered successfully.</response>
    /// <response code="400">User already exists.</response>
    /// <response code="500">An error occurred while processing the request.</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            if (await userRepo.GetByEmailAsync(request.Email) != null)
                return BadRequest("User already exists");

            var salt = PasswordHelper.GenerateSalt();
            var hashedPassword = PasswordHelper.HashPassword(request.Password, salt);

            var user = new User
            {
                Email = request.Email,
                HashedPassword = hashedPassword,
                Salt = salt,
                Role = "User"
            };

            await userRepo.CreateAsync(user);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred while registering the user." + ex.Message);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Refreshes the JWT token.
    /// </summary>
    /// <param name="request">Refresh token request containing the refresh token.</param>
    /// <returns>New JWT token if refresh is successful, otherwise an error message.</returns>
    /// <response code="200">Returns the new JWT token.</response>
    /// <response code="401">Invalid refresh token.</response>
    /// <response code="500">An error occurred while processing the request.</response>
    [HttpPost("refresh-token")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var existingToken = await tokenRepo.GetByTokenAsync(request.Token);
            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            existingToken.IsRevoked = true;
            await tokenRepo.UpdateAsync(existingToken);

            var user = await userRepo.GetByIdAsync(existingToken.UserId);
            if (user == null)
                return Unauthorized("Invalid refresh token");

            var authResult = await CreateTokens(user);

            return Ok(authResult);
        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred while refreshing the token." + ex.Message);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Logs out the user.
    /// </summary>
    /// <param name="request">Logout request containing the token.</param>
    /// <returns>Success message if logout is successful.</returns>
    /// <response code="200">Logged out successfully.</response>
    /// <response code="401">Invalid token.</response>
    /// <response code="500">An error occurred while processing the request.</response>
    [HttpPost("logout")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        try
        {
            var existingToken = await tokenRepo.GetByTokenAsync(request.Token);
            if (existingToken == null)
                return Unauthorized("Invalid token");

            existingToken.IsRevoked = true;
            await tokenRepo.UpdateAsync(existingToken);

            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred while logging out the user." + ex.Message);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    private async Task<AuthResult> CreateTokens(User user)
    {
        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();

        await tokenRepo.CreateAsync(refreshToken, user.Id);

        var authResult = new AuthResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return authResult;
    }
}