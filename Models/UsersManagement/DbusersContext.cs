using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UserManagementAPI.Models.UsersManagement;

public partial class DbusersContext : DbContext
{
    public readonly IConfiguration _configDbUsers;

    public DbusersContext()
    {
    }

    public DbusersContext(IConfiguration configuration)
    {
        _configDbUsers = configuration;
    }

    public DbusersContext(DbContextOptions<DbusersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(_configDbUsers.GetConnectionString("Produccion"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("tb_Users");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
