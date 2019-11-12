using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PointOfSales.Core.Service
{

    public  class POSService 
    {
        private readonly POSContext _context;

        public POSService(POSContext context)
        {
            this._context = context;
        }

    }
}
