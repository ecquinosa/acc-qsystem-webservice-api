using com.allcard.institution.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.repository
{
    public interface IRoleRepository : IBaseRepository<Role>
    { 
    
    }
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(InstitutionContext context) : base(context)
        {
        }
    }
}
