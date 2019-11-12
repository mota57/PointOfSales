using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PointOfSales.Core.Service
{
    public class ModifierService 
    {
        
        private readonly POSContext _context;
        public ModifierService(POSContext _context)
        {
            
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
    }
}
