using BookStore.DAL.Interface.Abstract;
using BookStore.DLL.Interface.Abstract;
using BookStore.DO.Entities;

namespace BookStore.DLL.RepositoryService
{
    public class RoleService:StoreService<Role>,IRoleService
    {
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Role GetRoleByName(string roleName)
        {
            return _repository.GetRoleByName(roleName);
        }
    }
}
