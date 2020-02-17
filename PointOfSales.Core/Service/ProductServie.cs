using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSales.Core.Service
{
 
    public class ProductService
    {
        private readonly POSContext _context;

        public ProductService(POSContext context)
        {
            this._context = context;
        }

        public async Task<Product> GetProduct(int productId)
        {

             var product = await _context.Product
                         .Include(_ => _.ProductModifier)
                         .FirstOrDefaultAsync(_ => _.Id == productId);

            return product;
        }

        public void UpsertDeleteProductModifiers(Product product, ProductFormDTO dto) 
        {
            var productModifierClient = ProductMapper.CreateListOfProductModifier(dto);

            UpsertDeleteProductModifiers(product, productModifierClient);
        }
       
        /// <summary>
        /// Assing a modifiers to a product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productModifierClient"></param>
        /// <returns></returns>
        public void UpsertDeleteProductModifiers(Product product, List<ProductModifier> productModifierClient)
        {
            
            var productId = product.Id;
           
            var productModifierDb = product.ProductModifier;
            var noRecordsInDatabase = productModifierDb.Count() == 0;
            if (noRecordsInDatabase)
            {
                foreach (var item in productModifierClient)
                {
                    item.ProductId = productId;
                    productModifierDb.Add(item);
                }
            }
            else
            {
                foreach (var item in productModifierClient)
                {
                    var noRecordMatchInDatabase = !productModifierDb.Any(_ => _.ProductId == productId && 
                                                                    _.ModifierId == item.ModifierId); 
                    if (noRecordMatchInDatabase)
                    {
                        item.ProductId = productId;
                        _context.Add(item);
                    }
                }

                foreach (var item in productModifierDb)
                {
                    var noRecordMatchInClient = !productModifierClient.Any(_ => _.ProductId == productId 
                                                            && _.ModifierId == item.ModifierId); 
                    if (noRecordMatchInClient)
                    {
                        _context.Remove(item);
                    }
                }
            }
        }
    }
}
