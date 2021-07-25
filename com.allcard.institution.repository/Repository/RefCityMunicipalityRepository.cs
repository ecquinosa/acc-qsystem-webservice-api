using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IRefCityMunicipalityRepository : IBaseRepository<RefCityMunicipality>
    {
        Task<IList<RefCityMunicipality>> Search(string value);
        Task<IList<RefCityMunicipality>> GetByProvinces(List<string> regionCode);
    }
    public class RefCityMunicipalityRepository : BaseRepository<RefCityMunicipality>, IRefCityMunicipalityRepository
    {
        public RefCityMunicipalityRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<IList<RefCityMunicipality>> GetByProvinces(List<string> provinceCodes)
        {
            return await _context.RefCityMunicipality.Where(a=> provinceCodes.Contains(a.ProvinceCode)).ToListAsync();
        }

        public async Task<IList<RefCityMunicipality>> Search(string value)
        {
            return await _context.RefCityMunicipality.Where(a => a.Description.Contains(value)).ToListAsync();
        }
    }
}
