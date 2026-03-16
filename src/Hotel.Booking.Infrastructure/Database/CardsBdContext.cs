using Hotel.Booking.Domain.Entities;
//using Hotel.Booking.Infrastructure.Database.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hotel.Booking.Infrastructure.Database
{
    public class CardsBdContext : DbContext
    {
        public CardsBdContext(DbContextOptions<CardsBdContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Habilitar logging detallado ANTES de los interceptores
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.LogTo(
                message => System.Diagnostics.Debug.WriteLine(message),
                new[] { DbLoggerCategory.Database.Command.Name },
                LogLevel.Information
            );
#endif
            //optionsBuilder.AddInterceptors(
            //    new SymmetricKeyInterceptor(),
            //    new EncryptionSaveChangesInterceptor(),
            //    new CCDeliveryCardIdInterceptor()
            //);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica TODAS las configuraciones automáticamente
           
            // Booking domain entities (if present)
            modelBuilder.Entity<Hotels?>()?.ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<RoomType?>()?.ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<RoomInventory?>()?.ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<RatePlan?>()?.ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<Bookings?>()?.ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<Guest?>()?.ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<Payment?>()?.ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<IdempotencyRecord?>()?.ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<AuditLog?>()?.ToTable(tb => tb.UseSqlOutputClause(false));
        }


        // Subtypes

        // Value/Child entities
       

        // Booking domain (optional if entities exist)
        public DbSet<Domain.Entities.Hotels>? Hotels => Set<Hotels>();
        public DbSet<RoomType>? RoomTypes => Set<RoomType>();
        public DbSet<RoomInventory>? RoomInventories => Set<RoomInventory>();
        public DbSet<RatePlan>? RatePlans => Set<RatePlan>();
        public DbSet<Domain.Entities.Bookings>? Bookings => Set<Bookings>();
        public DbSet<Guest>? Guests => Set<Guest>();
        public DbSet<Payment>? Payments => Set<Payment>();
        public DbSet<IdempotencyRecord>? IdempotencyRecords => Set<IdempotencyRecord>();
        public DbSet<AuditLog>? AuditLogs => Set<AuditLog>();

    }
}
