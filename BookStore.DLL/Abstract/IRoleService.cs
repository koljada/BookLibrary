using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DLL.Abstract
{
     public interface IRoleService:IStoreService<Role>
    {
         Role GetRoleByName(string roleName);
    }
}
