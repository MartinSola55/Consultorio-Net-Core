using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Models;

namespace Consultorio.Data.Repository.IRepository
{
    public interface IAlertaRepository : IRepository<Alerta>
    {
        Task SoftDelete(long id);
        Task<List<Alerta>> GetActivas();
    }
}