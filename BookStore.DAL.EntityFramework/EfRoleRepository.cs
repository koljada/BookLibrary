using System.Collections.Generic;
using System.Linq;
using BookStore.DO.Entities;
using System.Data.Entity;
using BookStore.DAL.Interface.Abstract;


namespace BookStore.DAL.EntityFramework
{
    public class EfRoleRepository:EfStoreRepository<Role>, IRoleRepository
    {
        public override IList<Role> GetAll()
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Roles.Include(r => r.Users).ToList();
            }
        }

        public override void Save(Role obj)
        {
            using (EfDbContext context = new EfDbContext())
            {
                context.Roles.Add(obj);
                context.SaveChanges();
            }
        }

        public Role GetRoleByName(string roleName)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Roles.FirstOrDefault(r => r.Name == roleName);
            }
        }
    }
}
