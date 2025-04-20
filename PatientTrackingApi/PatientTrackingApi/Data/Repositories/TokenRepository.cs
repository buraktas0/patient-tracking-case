using Microsoft.EntityFrameworkCore;
using PatientTrackingApi.Data.Contexts;
using PatientTrackingApi.Domain.Entities;

namespace PatientTrackingApi.Data.Repositories;

public class TokenRepository
{
    private readonly PatientDbContext context;
    private readonly ILogger<TokenRepository> logger;

    public TokenRepository(PatientDbContext context, ILogger<TokenRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        try
        {
            var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (refreshToken == null)
            {
                logger.LogWarning($"Refresh token not found.");
                return null;
            }

            return refreshToken;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving refresh token.");
            throw;
        }
    }

    public async Task CreateAsync(string refreshToken, int userId)
    {
        try
        {
            var token = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                UserId = userId
            };

            await context.RefreshTokens.AddAsync(token);
            await context.SaveChangesAsync();
            logger.LogInformation($"Token for user {token.UserId} created successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating token.");
            throw;
        }
    }

    public async Task UpdateAsync(RefreshToken token)
    {
        try
        {
            context.RefreshTokens.Update(token);
            await context.SaveChangesAsync();
            logger.LogInformation($"Token for user {token.UserId} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating token.");
            throw;
        }
    }
}