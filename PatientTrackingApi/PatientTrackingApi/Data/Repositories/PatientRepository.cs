using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Npgsql.Internal;
using PatientTrackingApi.Data.Contexts;
using PatientTrackingApi.Domain.Entities;
using PatientTrackingApi.Domain.Requests;

namespace PatientTrackingApi.Data.Repositories;

public class PatientRepository
{
    private readonly PatientDbContext context;
    private readonly ILogger<PatientRepository> logger;
    private readonly IHttpContextAccessor httpContextAccessor;

    public PatientRepository(
        PatientDbContext context,
        ILogger<PatientRepository> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        this.context = context;
        this.logger = logger;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Patient>?> GetAllAsync()
    {
        try
        {
            var patients = await context.Patients.Where(x => !x.IsDeleted).ToListAsync();
            if (patients == null || patients.Count == 0)
            {
                logger.LogWarning("No patients found.");
                return null;
            }

            return patients;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving patients.");
            throw;
        }
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        try
        {
            var patient = await context.Patients.FindAsync(id);
            if (patient == null)
            {
                logger.LogWarning($"Patient with ID {id} not found.");
                return null;
            }

            if (patient.IsDeleted)
            {
                logger.LogWarning($"Patient with ID {id} is marked as deleted.");
                return null;
            }

            return patient;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while retrieving patient with ID {id}.");
            throw;
        }
    }

    public async Task<Patient> CreateAsync(PatientCreateRequest request)
    {
        try
        {
            var patient = new Patient
            {
                Name = request.Name,
                Surname = request.Surname,
                BirthDate = DateTime.SpecifyKind(request.BirthDate, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = GetCurrentUserId(),
                UpdatedBy = GetCurrentUserId(),
            };

            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();
            return patient;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a new patient.");
            throw;
        }
    }

    public async Task<Patient?> UpdateAsync(PatientUpdateRequest request)
    {
        try
        {
            var patient = await context.Patients.FindAsync(request.Id);
            if (patient == null)
            {
                logger.LogWarning($"Patient with ID {request.Id} not found.");
                return null;
            }

            patient.Name = request.Name;
            patient.Surname = request.Surname;
            patient.BirthDate = DateTime.SpecifyKind(request.BirthDate, DateTimeKind.Utc);
            patient.UpdatedAt = DateTime.UtcNow;
            patient.UpdatedBy = GetCurrentUserId();

            context.Patients.Update(patient);
            await context.SaveChangesAsync();
            return patient;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while updating patient with ID {request.Id}.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var patient = await context.Patients.FindAsync(id);
            if (patient == null)
            {
                logger.LogWarning($"Patient with ID {id} not found.");
                return false;
            }

            patient.IsDeleted = true;
            patient.DeletedAt = DateTime.UtcNow;
            patient.DeletedBy = GetCurrentUserId();

            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while deleting patient with ID {id}.");
            throw;
        }
    }
    
    private string? GetCurrentUserId()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}