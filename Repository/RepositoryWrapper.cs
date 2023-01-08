using Contracts;
using DynamicLinq;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    { 
        private RepositoryContext _repoContext; 
        private IOwnerRepository _owner; 
        private IAccountRepository _account; 
        public IOwnerRepository Owner 
        { 
            get 
            { 
                if (_owner == null) 
                { 
                    _owner = new OwnerRepository(_repoContext); 
                } 
                return _owner; 
            } 
        } 
        
        public IAccountRepository Account 
        { 
            get 
            { 
                if (_account == null) 
                { 
                    _account = new AccountRepository(_repoContext); 
                } 
                return _account; 
            } 
        } 
        
        public RepositoryWrapper(RepositoryContext repositoryContext) 
        { 
            _repoContext = repositoryContext; 
        } 
        
        public void Save() 
        {
            _repoContext.SaveChanges();
        }

        private IQueryable<T> FindAllDynamic<T>() where T : class
        {
#if (ENABLED_FOR_LAZY_LOADING_USAGE)
            return _repoContext.Set<T>();
#else
            return this.RepositoryContext.Set<T>().AsNoTracking();
#endif
        }

        public IEnumerable<T>? GetOwnersByConditions<T>(List<WebApiDynamicCommunication> WebApiDynamicCommunication_Object_List) where T : class
        {
            RepositoryExpressionCriteria<T> RepositoryExpressionCriteria_Object =
                new RepositoryExpressionCriteria<T>();

            RepositoryExpressionCriteria_Object.Add(WebApiDynamicCommunication_Object_List[0].FieldName,
                                                    WebApiDynamicCommunication_Object_List[0].Value,
                                                    WebApiDynamicCommunication_Object_List[0].Expression);
            var Lambda = RepositoryExpressionCriteria_Object.GetLambda();
            if (null != Lambda)
            {
                var LambdaCompile = Lambda.Compile();
                return FindAllDynamic<T>().Where(LambdaCompile);
            }
            else
            {
                return null;
            }
        }
    }
}
