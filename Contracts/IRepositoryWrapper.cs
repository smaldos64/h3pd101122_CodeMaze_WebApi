using DynamicLinq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    { 
        IOwnerRepository Owner { get; } 
        IAccountRepository Account { get; }
        ITestDynamicRepository Dynamic { get; }
        IEnumerable<T>? GetOwnersByConditions<T>(List<WebApiDynamicCommunication> WebApiDynamicCommunication_Object_List) where T : class;
        void Save(); 
    }
}
