using Microsoft.EntityFrameworkCore;
using PatientTrackingApi.Domain.Entities;

namespace PatientTrackingApi.Data.Contexts;

public class PatientDbContext : DbContext
{
    public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options) { }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Log> Logs { get; set; }
}