using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDatabase
{
    public class Inventory
    {
        public IEnumerable<Product> AvailableProducts
        {
            get
            {
                DataClassesDataContext db = new DataClassesDataContext();
                return from p in db.Products
                       join i in db.InventoryProducts
                       on p.Id equals i.ProductId
                       select p;
            }
        }

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
                    int remainingAmount = p.Amount - amount;
                    if (remainingAmount == 0)
                        db.InventoryProducts.DeleteOnSubmit(p);
                    else if (remainingAmount < 0)
                        throw new Exception("Product not available in given amount");
                    else
                        p.Amount = remainingAmount;
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
