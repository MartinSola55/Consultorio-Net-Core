using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;

namespace Consultorio.Data.Repository
{
    public class HorarioRepository(ApplicationDbContext db) : Repository<Horario>(db), IHorarioRepository
    {
        private readonly ApplicationDbContext _db = db;


    }
}