using Microsoft.EntityFrameworkCore;

namespace AtonAssignment.Models;

/// <summary>
/// Database context.
/// </summary>
public partial class AtondbContext : DbContext
{
    public AtondbContext()
    {
    }

    public AtondbContext(DbContextOptions<AtondbContext> options)
            : base(options)
    {
        Database.EnsureCreated();
    }
    
    /// <summary>
    /// Users table.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.RevokedBy)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
