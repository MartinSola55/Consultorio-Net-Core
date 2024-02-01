using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Models;

namespace Consultorio.Data.Repository.IRepository
{
    public interface IObraSocialRepository : IRepository<ObraSocial>
    {
        Task Update(ObraSocial obraSocial);
        Task SoftDelete(long id);
        Task ChangeState(long id);
        Task<bool> IsDuplicated(ObraSocial obraSocial);
        Task<IEnumerable<SelectListItem>> GetDropDownList();
    }
}