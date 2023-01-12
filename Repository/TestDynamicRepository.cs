using DynamicLinq;
using Entities.Models;
using Entities;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace Repository
{
    public class TestDynamicRepository : RepositoryBase<TestDynamic>, ITestDynamicRepository
    {
        public TestDynamicRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
