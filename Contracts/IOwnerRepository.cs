using Entities.Models;

namespace Contracts
{
    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        IEnumerable<Owner> GetAllOwners(bool IncludeRelations);
        //IEnumerable<Owner> GetOwnersByConditions(string Condition, string FieldName);

        Task<IEnumerable<Owner>> GetOwnersByConditions(string Condition, string FieldName);
        Owner GetOwnerById(Guid ownerId);
        Owner GetOwnerWithDetails(Guid ownerId);
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);
    }
}
