﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DLL.Interface.Abstract
{
    public interface IStoreService<T> where T : class
    {
         T GetById(int id);
         IList<T> GetAll();
         void Create(T obj);
         void Save(T obj);
         T Delete(int id);
    }
}
