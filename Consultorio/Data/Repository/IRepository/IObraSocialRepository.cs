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
        void Update(ObraSocial obraSocial);
        void SoftDelete(long id);
        void ChangeState(long id);
        bool IsDuplicated(ObraSocial obraSocial);
        IEnumerable<SelectListItem> GetDropDownList();
    }
}