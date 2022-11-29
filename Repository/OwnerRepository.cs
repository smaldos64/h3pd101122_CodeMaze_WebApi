//using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository 
    { 
        public OwnerRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext) 
        { 
        }

        public IEnumerable<Owner> GetAllOwners(bool IncludeRelations)
        {
            if (true == IncludeRelations)
            {
                return FindAll()
                    .Include(a => a.Accounts)
                    .OrderBy(ow => ow.Name)
                    .ToList();
            }
            else
            {
                return FindAll()
                    .OrderBy(ow => ow.Name)
                    .ToList();
            }
        }

        public async Task<IEnumerable<Owner>> GetOwnersByConditions(string Condition,
                                                                    string FieldName)
        {
            //try
            //{
            //    var ParameterExpression1 = Expression.Parameter(typeof(Owner));
            //    //var ParameterExpression = Expression.Parameter(Type.GetType(
            //    //    "ExpressionTreeTests.owner"), "owner");
            //    var ParameterExpression = Expression.Parameter(typeof(Owner), "owner");
            //    var Constant = Expression.Constant(Condition);
            //    var Property = Expression.Property(ParameterExpression,
            //        FieldName);

            //    var ThisExpression = Expression.Equal(Property, Constant);

            //    var LambdaExpression = Expression.Lambda<Func<Owner, bool>>(ThisExpression, ParameterExpression);

            //    var Owners = FindByCondition(LambdaExpression);

            //    return Owners;
            //}
            //catch (Exception Error)
            //{
            //    string ErrorString = Error.ToString();
            //}

            //return null;

            var OwnerFilter = "o => o." + FieldName + "==" + Condition;
            var Options = ScriptOptions.Default.AddReferences(typeof(Owner).Assembly);

            Func<Owner, bool> OwnerFilterexpression = await CSharpScript.EvaluateAsync<Func<Owner, bool>>(OwnerFilter, Options);

            var OwnerList = RepositoryContext.Owners.Where(OwnerFilterexpression);

            return (OwnerList);
        }

        public Owner GetOwnerById(Guid ownerId)
        {
            return FindByCondition(owner => owner.Id.Equals(ownerId))
                .FirstOrDefault();
        }
        
        public Owner GetOwnerWithDetails(Guid ownerId)
        {
            return FindByCondition(owner => owner.Id.Equals(ownerId))
                .Include(ac => ac.Accounts)
                .FirstOrDefault();
        }

        public void CreateOwner(Owner owner) => Create(owner);

        public void UpdateOwner(Owner owner) => Update(owner);

        public void DeleteOwner(Owner owner) => Delete(owner);
    }
}
