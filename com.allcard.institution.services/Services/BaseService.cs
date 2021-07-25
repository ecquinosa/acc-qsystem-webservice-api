using AutoMapper;
using com.allcard.common;
using com.allcard.institution.repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.services
{
    public interface IBaseService
    {
        Task<responseVM> Create(requestVM payload, bool IsSave);
        Task<responseVM> Update(requestVM payload, bool IsSave);
        Task<responseVM> Delete(requestVM payload, bool IsSave);
        Task<responseVM> Get(requestVM payload);

    }
    public abstract class BaseService : IBaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected string _audience;
        protected string _subject;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public virtual async Task<responseVM> Create(requestVM payload, bool IsSave)
        {
            return new responseVM();
        }

        public virtual async Task<responseVM> Delete(requestVM payload, bool IsSave)
        {
            return new responseVM();
        }

        public virtual async Task<responseVM> Get(requestVM payload)
        {
            return new responseVM();
        }

        public virtual async Task<responseVM> Update(requestVM payload, bool IsSave)
        {
            return new responseVM();
        }
    }
}
