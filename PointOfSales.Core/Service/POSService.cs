﻿using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSales.Core.Service
{
    //handle validation
    //
    public class ProductServie
    {
        private readonly POSContext context;

        public ProductServie(POSContext context)
        {
            this.context = context;
        }

        public void CreateProduct(ProductDTO dto)
        {

        }
        public void UppdateProduct()
        {

        }
    }

   public  class POSService 
    {
        private readonly POSContext _context;

        public POSService(POSContext context)
        {
            this._context = context;
        }


        /// <summary>
        /// Add or update modifier and Item modifier
        /// </summary>
        /// <param name="modClient"></param>
        public void UpsertDeleteModiferAndItemModifier(Modifier modClient)
        {
            //load the modifier
            var modDb = _context.Modifier
                .Include(_ => _.ItemModifier)
                .FirstOrDefault(_ => _.Id == modClient.Id);

            if (modDb == null)
            {
                _context.Add(modClient);
            }
            else
            {
                //set values
                _context.Entry(modDb).CurrentValues.SetValues(modClient);

                //check what are in the db and update it
                foreach (var item in modClient.ItemModifier)
                {
                    var itemDb = modDb.ItemModifier.FirstOrDefault(_ => _.Id == item.Id);

                    if (itemDb == null)
                    {
                        item.ModifierId = modClient.Id;
                        _context.ItemModifier.Add(item);
                    }
                    else
                    {
                        _context.Entry<ItemModifier>(itemDb).CurrentValues.SetValues(item);
                    }
                }

                foreach (var item in modDb.ItemModifier)
                {
                    if (!modClient.ItemModifier.Any(_ => _.Id == item.Id))
                    {
                        _context.ItemModifier.Remove(item);
                    }

                }
            }
        }


        /// <summary>
        /// Assing a modifiers to a product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productModifierClient"></param>
        /// <returns></returns>
        public async Task UpsertDeleteProductModifiers(int productId, List<ProductModifier> productModifierClient)
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
