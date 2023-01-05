using DynamicLinq;
using Entities.Models;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        IEnumerable<Owner> GetAllOwners(bool IncludeRelations);
        public IEnumerable<Owner>? GetOwnersByConditions(List<WebApiDynamicCommunication> WebApiDynamicCommunication_Object_List);
        IEnumerable<Owner>? GetOwnersByConditions(string FieldName, object Value, ExpressionType Expression);

        Task<IEnumerable<Owner>> GetOwnersByConditions(string Condition, string FieldName);
        Owner GetOwnerById(Guid ownerId);
        Owner GetOwnerWithDetails(Guid ownerId);
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);
    }
}
