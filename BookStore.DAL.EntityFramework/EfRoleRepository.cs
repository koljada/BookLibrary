using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System.Data.Entity; 


namespace BookStore.DAL.EntityFramework
{
    public class EfRoleRepository:EfStoreRepository<Role>, IRoleRepository
    {
        public override IList<Role> GetAll()
        {
            return Context.Roles.Include(r => r.Users).ToList();
        }
        public override void Save(Role obj)
        {
            Context.Roles.Add(obj);
            Context.SaveChanges();
        }
        public Role GetRoleByName(string roleName)
        {
            return Context.Roles.FirstOrDefault( r => r.Name == roleName); 
        }
    }
}
