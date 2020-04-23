using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDatabase
{
    public class Inventory
    {
        /*IEnumerable<Product> AvailableProducts
        {
            //TODO: Implement
        }*/

        public void Resupply(Product product, Supplier supplier, int amount)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                if (db.InventoryProducts.Any(x => x.ProductId == product.Id))
                {
                    var p = db.InventoryProducts.Single(x => x.ProductId == product.Id);
                    p.Amount += amount;
                }
                else
                {
                    db.InventoryProducts.InsertOnSubmit(new InventoryProduct() { ProductId = product.Id,
                                                                                 Amount = amount });
                }
                db.SubmitChanges();
            }
        }

        public void Sell(Product product, int amount)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                if (db.InventoryProducts.Any(x => x.ProductId == product.Id))
                {
                    var p = db.InventoryProducts.Single(x => x.ProductId == product.Id);
                    p.Amount -= amount;
                }
                else
                {
                    throw new Exception("Product not available");
                }
                db.SubmitChanges();
            }
        }
    }
}
