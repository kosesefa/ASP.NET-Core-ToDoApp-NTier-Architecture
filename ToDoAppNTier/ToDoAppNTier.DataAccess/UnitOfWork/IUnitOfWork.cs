﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoAppNTier.DataAccess.Interfaces;
using ToDoAppNTier.Entities.Domains;

namespace ToDoAppNTier.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T:BaseEntity;
        Task SaveChanges();
    }
}
