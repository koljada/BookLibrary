using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BookStore.DAL.Concrete
{
    public class EFRoleRepository:EFStoreRepository<Role>, IRoleRepository
    {
        public override IQueryable<Role> GetAll()
        {
            return context.Roles.Include(r=>r.Users);
        }
        public override void Save(Role obj)
        {
            context.Roles.Add(obj);
            context.SaveChanges();
        }
        public Role GetRoleByName(string role_name)
        {
            return context.Roles.FirstOrDefault(r => r.Name == role_name); 
        }
    }
}
