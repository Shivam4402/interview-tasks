using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EF_MultiTable.Models;

public partial class EfMultiTableContext : DbContext
{
    public EfMultiTableContext()
    {
    }

    public EfMultiTableContext(DbContextOptions<EfMultiTableContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<State> States { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__F2D21B76C74155D0");

            entity.ToTable("City");

            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__City__StateId__4BAC3F29");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__State__C3BA3B3AAFD745F8");

            entity.ToTable("State");

            entity.Property(e => e.StateName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
