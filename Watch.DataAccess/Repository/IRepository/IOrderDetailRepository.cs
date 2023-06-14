﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Models;

namespace Watch.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository :IRepository<Category>
    {
        void Update(Category obj);
    }
}
