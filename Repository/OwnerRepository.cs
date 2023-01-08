//using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using DynamicLinq;

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
                // Hvis RepositoryBase bruger IEnumerable
                // skal konstruktionen herunder bruges.
                //return base.RepositoryContext.Owners.
                //    Include(a => a.Accounts).
                //    OrderBy(ow => ow.Name)
                //    .ToList();

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

        public IEnumerable<Owner>? GetOwnersByConditions(List<WebApiDynamicCommunication> WebApiDynamicCommunication_Object_List)
        {
            //RepositoryExpressionCriteria<Owner> RepositoryExpressionCriteria_Object =
            //    new RepositoryExpressionCriteria<Owner>();

            //RepositoryExpressionCriteria_Object.Add(WebApiDynamicCommunication_Object_List[0].FieldName,
            //                                        WebApiDynamicCommunication_Object_List[0].Value,
            //                                        WebApiDynamicCommunication_Object_List[0].Expression);
            //var Lambda = RepositoryExpressionCriteria_Object.GetLambda();
            //if (null != Lambda)
            //{
            //    var LambdaCompile = Lambda.Compile();
            //    return FindAll().Where(LambdaCompile);
            //}
            //else
            //{
            //    return null;
            //}
            return null;
        }

        public IEnumerable<Owner>? GetOwnersByConditions(string FieldName,
                                                         object Value, 
                                                         ExpressionType Expression)
        {
            //RepositoryExpressionCriteria<Owner> RepositoryExpressionCriteria_Object = 
            //    new RepositoryExpressionCriteria<Owner>();

            //RepositoryExpressionCriteria_Object.Add(FieldName, Value, Expression);
            //var Lambda = RepositoryExpressionCriteria_Object.GetLambda();
            //if (null != Lambda)
            //{
            //    var LambdaCompile = Lambda.Compile();
            //    return FindAll().Where(LambdaCompile);
            //}
            //else
            //{
            //    return null;
            //}

            return null;
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

            try
            {
                var OwnerFilter = "o => o." + FieldName + "==" + Condition;
                var Options = ScriptOptions.Default.AddReferences(typeof(Owner).Assembly);

                Func<Owner, bool> OwnerFilterexpression = await CSharpScript.EvaluateAsync<Func<Owner, bool>>(OwnerFilter, Options);

                var OwnerList = RepositoryContext.Owners.Where(OwnerFilterexpression);

                return (OwnerList);
            }
            catch(Exception Error)
            {
                string ErrorString = Error.ToString();
            }

            return (null);
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
