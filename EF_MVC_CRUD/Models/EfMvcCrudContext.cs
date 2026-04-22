using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EF_MVC_CRUD.Models;

public partial class EfMvcCrudContext : DbContext
{
    public EfMvcCrudContext()
    {
    }

    public EfMvcCrudContext(DbContextOptions<EfMvcCrudContext> options)
        : base(options)
    {
    }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Technology> Technologies { get; set; }
      
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__States__C3BA3B3A85640512");

            entity.Property(e => e.StateName).HasMaxLength(100);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B994602CF44");

            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.ImagePath).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(150);

            entity.HasOne(d => d.State).WithMany(p => p.Students)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_States");
        });

        modelBuilder.Entity<Technology>(entity =>
        {
            entity.HasKey(e => e.TechnologyId).HasName("PK__Technolo__7057015854407F3C");

            entity.Property(e => e.TechnologyName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
