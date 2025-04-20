using PatientTrackingApi.Data.Contexts;
using PatientTrackingApi.Domain.Entities;

namespace PatientTrackingApi.Data.Repositories;

public class LogRepository
{
    private readonly PatientDbContext context;

    public LogRepository(PatientDbContext context)
    {
        this.context = context;
    }

    public async Task LogAsync(Log log)
    {
        await context.Logs.AddAsync(log);
        await context.SaveChangesAsync();
    }
}