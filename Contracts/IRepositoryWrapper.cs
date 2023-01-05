using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    { 
        IOwnerRepository Owner { get; } 
        IAccountRepository Account { get; } 
        //IRepositoryExpressionCriteria<T> RepositoryExpressionCriteria { get; }
        void Save(); 
    }
}
