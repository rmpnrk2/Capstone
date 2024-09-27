using Microsoft.EntityFrameworkCore;

namespace SouthSideK9Camp.Server.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Shared.Client> Clients => Set<Shared.Client>();
    public DbSet<Shared.Contract> Contracts => Set<Shared.Contract>();
    public DbSet<Shared.Customer> Customers => Set<Shared.Customer>();
    public DbSet<Shared.Dog> Dogs => Set<Shared.Dog>();
    public DbSet<Shared.Invoice> Invoices => Set<Shared.Invoice>();
    public DbSet<Shared.Item> Items => Set<Shared.Item>();
    public DbSet<Shared.Log> Logs => Set<Shared.Log>();
    public DbSet<Shared.Member> Members => Set<Shared.Member>();
    public DbSet<Shared.MembershipDue> MembershipDues => Set<Shared.MembershipDue>();
    public DbSet<Shared.ProgressReport> ProgressReports => Set<Shared.ProgressReport>();
    public DbSet<Shared.Reservation> Reservations => Set<Shared.Reservation>();
    public DbSet<Shared.User> Users => Set<Shared.User>();
    public DbSet<Shared.ReasonForRejection> Reasons => Set<Shared.ReasonForRejection>();
}
