﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Plagas.Entities;

namespace Plagas.Persistence
{
    public static class UserDataSeeder
    {
        public static async Task Seed(IServiceProvider service)
        {
            // Repositorio de Usuarios
            var userManager = service.GetRequiredService<UserManager<PlagasUserIdentity>>();

            // Repositorio de Roles
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            // Crear roles
            var adminRole = new IdentityRole(Constantes.RolAdmin);
            var customerRole = new IdentityRole(Constantes.RolCustomer);

            if (!await roleManager.RoleExistsAsync(Constantes.RolAdmin))
                await roleManager.CreateAsync(adminRole);

            if (!await roleManager.RoleExistsAsync(Constantes.RolCustomer))
                await roleManager.CreateAsync(customerRole);

            // El usuario Administrador
            var adminUser = new PlagasUserIdentity()
            {
                FirstName = "Administrador",
                LastName = "del Sistema",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                PhoneNumber = "999 999 999",
                Age = 30,
                DocumentType = DocumentTypeEnum.Dni,
                DocumentNumber = "12345678",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin1234*");
            if (result.Succeeded)
            {
                // Obtenemos el registro del usuario
                adminUser = await userManager.FindByEmailAsync(adminUser.Email);
                // Aqui agregamos el Rol de Administrador para el usuario Admin
                if (adminUser is not null)
                    await userManager.AddToRoleAsync(adminUser, Constantes.RolAdmin);
            }
        }

    }
}
