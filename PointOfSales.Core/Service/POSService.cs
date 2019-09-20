using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using System.Collections.Generic;
using System.Linq;
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

        public async Task UpsertProductModifiers(int productId, List<ProductModifier> productModifierClient)
        {
           
            var modDb = await _context.Product
                        .Include(_ => _.ProductModifier)
                        .Where(_ => _.Id == productId)
                        .FirstOrDefaultAsync();


            var productModDb = modDb.ProductModifier;

            if (productModDb.Count() == 0)
            {
                foreach (var item in productModifierClient)
                {
                    item.ProductId = productId;
                    productModDb.Add(item);
                }
            }
            else
            {
                foreach (var item in productModifierClient)
                {
                    if (!productModDb.Any(_ => _.ProductId == productId && _.ModifierId == item.ModifierId))
                    {
                        item.ProductId = productId;
                        _context.Add(item);
                    }
                }

                foreach (var item in productModDb)
                {
                    if (!productModifierClient.Any(_ => _.ProductId == productId && _.ModifierId == item.ModifierId))
                    {
                        _context.Remove(item);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
