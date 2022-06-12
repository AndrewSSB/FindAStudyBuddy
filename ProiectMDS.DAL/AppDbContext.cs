﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProiectMDS.DAL.Configurations;
using ProiectMDS.DAL.Entities;
using ProiectMDS.DAL.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectMDS.DAL
{
    public class AppDbContext : IdentityDbContext<
        User,
        Role,
        int,
        IdentityUserClaim<int>,
        UserRole,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<BaseCourses> BaseCourses { get; set; }
        public DbSet<UserConnections> UserConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new ProfileConfig());
            modelBuilder.ApplyConfiguration(new LocationConfig());
            modelBuilder.ApplyConfiguration(new CoursesConfig());
            modelBuilder.ApplyConfiguration(new BaseCourseConfig());
            modelBuilder.ApplyConfiguration(new UserConnectionConfig());

            modelBuilder.Entity<User>(b =>
            {
                b.HasMany(u => u.UserRoles)
                 .WithOne(ur => ur.User)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.HasMany(u => u.UserRoles)
                 .WithOne(ur => ur.Role)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();
            });
        }
    }
}