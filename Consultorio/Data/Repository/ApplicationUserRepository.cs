using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Microsoft.EntityFrameworkCore;

namespace Consultorio.Data.Repository
{
    public class ApplicationUserRepository(ApplicationDbContext db) : Repository<ApplicationUser>(db), IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db = db;


        public async Task<IdentityRole> GetRole(string userID)
        {
            return await _db
                .Roles
                .FirstAsync(x => x.Id.Equals(_db.UserRoles.First(x => x.UserId.Equals(userID)).RoleId));
        }
    }
}