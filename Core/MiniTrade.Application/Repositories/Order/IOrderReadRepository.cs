﻿using MiniTrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Repositories
{
    public interface IOrderReadRepository:IReadRepository<Order>
    {
    }
}
