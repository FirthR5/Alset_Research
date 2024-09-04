using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Alset_Research.Models;

public partial class AlsetJournalsContext : DbContext
{
    public AlsetJournalsContext()
    {
    }

    public AlsetJournalsContext(DbContextOptions<AlsetJournalsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Follower> Followers { get; set; }

    public virtual DbSet<Journal> Journals { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DevDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Follower>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Follower__3214EC076A0D0B90");

            entity.HasIndex(e => new { e.ResearcherId, e.FollowerId }, "UQ_Followers").IsUnique();

            entity.HasOne(d => d.FollowerNavigation).WithMany(p => p.FollowerFollowerNavigations)
                .HasForeignKey(d => d.FollowerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Followers_Follower");

            entity.HasOne(d => d.Researcher).WithMany(p => p.FollowerResearchers)
                .HasForeignKey(d => d.ResearcherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Followers_Researcher");
        });

        modelBuilder.Entity<Journal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Journals__3214EC073BD58F00");

            entity.Property(e => e.Pdffile)
                .HasMaxLength(255)
                .HasColumnName("PDFFile");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Journals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Journals_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07085B2D0C");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053455DEF8B4").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
