using DynamicLinq;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryExpressionCriteria<T> : IRepositoryBase<T> where T : class
    {
        public IEnumerable<T> SetupAndExecuteLambdaExpression<T>(List<WebApiDynamicCommunication> WebApiDynamicCommunication_Object_List);
    }
}
