using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Seeders
{
    public class UserRoleSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager; 

        public UserRoleSeeder(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager; 
        }

        public async void SeedRoles()
        {
            var adminRoleBool = _roleManager.RoleExistsAsync("admin").Result;
            var clientRoleBool = _roleManager.RoleExistsAsync("client").Result;
            var healthWorkerRoleBool = _roleManager.RoleExistsAsync("healthworker").Result;

            if(!adminRoleBool)
                await _roleManager.CreateAsync(new IdentityRole("admin"));
            if(!healthWorkerRoleBool)
                await _roleManager.CreateAsync(new IdentityRole("healthworker"));
            if(!clientRoleBool)
                await _roleManager.CreateAsync(new IdentityRole("client"));
        }
    }
}
