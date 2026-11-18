using ElearningQuiz.Core.Entities;
using ElearningQuiz.Core.Enums;
using ElearningQuiz.Infrastructure.Data;
using ElearningQuiz.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElearningQuiz.Infrastructure.Seeders
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ElearningQuizDbContext context)

        {
            await context.Database.MigrateAsync();


            if (!context.Users.Any(u => u.UserId == "usr_001"))
                
            {
                var passwordHasher = new PasswordHasher();
                var defaultAdmin = new User
                {
                    UserId = "usr_001",
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@elearningquiz.com",
                    PasswordHash = passwordHasher.HashPassword("Admin@123"),
                    MobileNo = "+1234567890",
                    Role = UserRole.SuperAdmin,
                    CreatedOn = DateTime.UtcNow
                };

                await context.Users.AddAsync(defaultAdmin);
                await context.SaveChangesAsync();
            }



            if (!await context.Faculties.AnyAsync())
            {
                var defaultFaculty = new Faculty
                {
                    FacId = "fac_001",
                    FacName = "Faculty of ",
                    CreatedOn = DateTime.UtcNow
                };

                await context.Faculties.AddAsync(defaultFaculty);

                var defaultDept = new Department
                {
                    DepId = "dep_iat_001",
                    FacId = "fac_001",
                    DepName = "Instrumentation and Automation Technology Department",
                    DepCode = "IAT",
                    CreatedOn = DateTime.UtcNow
                };

                await context.Departments.AddAsync(defaultDept);
                await context.SaveChangesAsync();
            }


        }
    }
}
