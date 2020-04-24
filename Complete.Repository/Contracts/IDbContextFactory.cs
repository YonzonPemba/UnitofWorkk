﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Repository.Contracts
{
    public interface IDbContextFactory
    {
        DbContext GetContext();
    }
}
