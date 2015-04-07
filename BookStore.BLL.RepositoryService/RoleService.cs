using BookStore.DAL.Abstract;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;

namespace BookStore.BLL.RepositoryService
{
    public class RoleService:StoreService<Role>,IRoleService
    {
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository repo)
        {
            _repository = repo;
        }

        public Role GetRoleByName(string roleName)
        {
            return _repository.GetRoleByName(roleName);
        }
    }
}
