using System;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IUnitOfWork
    {
        Task Save();
        bool isDisposed { get; }

        IInstitutionRepository InstitutionRepository { get; }
        IChainRepository ChainRepository { get; }
        IGroupRepository GroupRepository { get; }
        IMerchantRepository MerchantRepository { get; }
        IBranchRepository BranchRepository { get; }
        ILocationRepository LocationRepository { get; }
        IBranchScheduleRepository BranchScheduleRepository { get; }
        IMemberRepository MemberRepository { get; }
        IBranchScheduleMemberRepository BranchScheduleMemberRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserProfileRepository UserProfileRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IAuditLogRepository AuditLogRepository { get; }
        IRefCityMunicipalityRepository RefCityMunicipalityRepository { get; }
        IRefProvinceRepository RefProvinceRepository { get; }

        

    }
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private InstitutionContext context;

        #region === UnitOfWork Constructor ===

        public UnitOfWork(InstitutionContext Context)
        {
            this.context = Context;
        }

        #endregion

        #region === IUnitOfWork Methods ===
        public async Task Save()
        {
            //context.SaveChanges();
            await context.SaveChangesAsync();
        }

        public bool isDisposed
        {
            get { return disposed; }
        }

        #endregion

        #region === IDisposable Members ===

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            this.Dispose(false);
        }
        #endregion

        private IInstitutionRepository institutionRepository;
        public IInstitutionRepository InstitutionRepository
        {
            get
            {
                if (this.institutionRepository == null)
                    this.institutionRepository = new InstitutionRepository(context);
                return institutionRepository;
            }
        }
        private IChainRepository chainRepository;
        public IChainRepository ChainRepository
        {
            get
            {
                if (this.chainRepository == null)
                    this.chainRepository = new ChainRepository(context);
                return chainRepository;
            }
        }
        private IGroupRepository groupRepository;
        public IGroupRepository GroupRepository
        {
            get
            {
                if (this.groupRepository == null)
                    this.groupRepository = new GroupRepository(context);
                return groupRepository;
            }
        }
        private IMerchantRepository merchantRepository;
        public IMerchantRepository MerchantRepository
        {
            get
            {
                if (this.merchantRepository == null)
                    this.merchantRepository = new MerchantRepository(context);
                return merchantRepository;
            }
        }
        private IBranchRepository branchRepository;
        public IBranchRepository BranchRepository
        {
            get
            {
                if (this.branchRepository == null)
                    this.branchRepository = new BranchRepository(context);
                return branchRepository;
            }
        }
        private ILocationRepository locationRepository;
        public ILocationRepository LocationRepository
        {
            get
            {
                if (this.locationRepository == null)
                    this.locationRepository = new LocationRepository(context);
                return locationRepository;
            }
        }

        private IBranchScheduleRepository branchScheduleRepository;
        public IBranchScheduleRepository BranchScheduleRepository
        {
            get
            {
                if (this.branchScheduleRepository == null)
                    this.branchScheduleRepository = new BranchScheduleRepository(context);
                return branchScheduleRepository;
            }
        }


        private IMemberRepository memberRepository;
        public IMemberRepository MemberRepository
        {
            get
            {
                if (this.memberRepository == null)
                    this.memberRepository = new MemberRepository(context);
                return memberRepository;
            }
        }
        private IBranchScheduleMemberRepository branchScheduleMemberRepository;
        public IBranchScheduleMemberRepository BranchScheduleMemberRepository
        {
            get
            {
                if (this.branchScheduleMemberRepository == null)
                    this.branchScheduleMemberRepository = new BranchScheduleMemberRepository(context);
                return branchScheduleMemberRepository;
            }
        }
        private IRoleRepository roleRepository;
        public IRoleRepository RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                    this.roleRepository = new RoleRepository(context);
                return roleRepository;
            }
        }
        private IUserProfileRepository userProfileRepository;
        public IUserProfileRepository UserProfileRepository
        {
            get
            {
                if (this.userProfileRepository == null)
                    this.userProfileRepository = new UserProfileRepository(context);
                return userProfileRepository;
            }
        }
        private IUserRoleRepository userRoleRepository;
        public IUserRoleRepository UserRoleRepository
        {
            get
            {
                if (this.userRoleRepository == null)
                    this.userRoleRepository = new UserRoleRepository(context);
                return userRoleRepository;
            }
        }
        private IAuditLogRepository auditLogRepository;
        public IAuditLogRepository AuditLogRepository
        {
            get
            {
                if (this.auditLogRepository == null)
                    this.auditLogRepository = new AuditLogRepository(context);
                return auditLogRepository;
            }
        }
        private IRefCityMunicipalityRepository refCityMunicipalityRepository;
        public IRefCityMunicipalityRepository RefCityMunicipalityRepository
        {
            get
            {
                if (this.refCityMunicipalityRepository == null)
                    this.refCityMunicipalityRepository = new RefCityMunicipalityRepository(context);
                return refCityMunicipalityRepository;
            }
        }
        private IRefProvinceRepository refProvinceRepository;
        public IRefProvinceRepository RefProvinceRepository
        {
            get
            {
                if (this.refProvinceRepository == null)
                    this.refProvinceRepository = new RefProvinceRepository(context);
                return refProvinceRepository;
            }
        }
    }
}
