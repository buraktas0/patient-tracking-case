using Microsoft.EntityFrameworkCore;
using PatientTrackingApi.Data.Contexts;
using PatientTrackingApi.Domain.Entities;

namespace PatientTrackingApi.Data.Repositories;

public class UserRepository
{
    private readonly PatientDbContext context;
    private readonly ILogger<UserRepository> logger;

    public UserRepository(PatientDbContext context, ILogger<UserRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        try
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                logger.LogWarning($"User with ID {id} not found.");
                return null;
            }

            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving user.");
            throw;
        }
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                logger.LogWarning($"User with email {email} not found.");

            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving user by email.");
            throw;
        }
    }

    public async Task CreateAsync(User user)
    {
        try
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                logger.LogWarning($"User with email {user.Email} already exists.");
                return;
            }

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            logger.LogInformation($"User with email {user.Email} added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding user.");
            throw;
        }
    }
}