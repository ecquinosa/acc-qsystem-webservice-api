using com.allcard.institution.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.repository
{
    public interface IMemberRepository : IBaseRepository<Member>
    { 
    
    }
    public class MemberRepository : BaseRepository<Member> , IMemberRepository
    {
        public MemberRepository(InstitutionContext context) : base(context)
        {
        }
    }
}
