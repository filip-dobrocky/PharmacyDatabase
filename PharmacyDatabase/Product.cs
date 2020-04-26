using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDatabase
{
    public partial class Product
    {
        public IEnumerable<Supplier> Suppliers
        {
            get
            {
                DataClassesDataContext db = new DataClassesDataContext();
                return from s in db.Suppliers
                       join sp in db.SupplierProducts
                       on s.Id equals sp.IdSupplier
                       where sp.IdProduct == Id
                       select s;
            }
        }

        public int AvailableAmount
        {
            get
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    var p = db.InventoryProducts.Single(x => x.ProductId == Id);
                    return p != null ? p.Amount : 0;
                }
            }
        }

        public void AssignSupplier(Supplier supplier, Decimal price)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                if (!db.SupplierProducts.Any(x => x.IdProduct == Id && x.IdSupplier == supplier.Id))
                    db.SupplierProducts.InsertOnSubmit(new SupplierProduct() { IdProduct = Id,
                                                                               IdSupplier = supplier.Id,
                                                                               BuyingPrice = price });
                db.SubmitChanges();
            }
        }

        public static decimal GetBuyingPrice(Product product, Supplier supplier)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var p = db.SupplierProducts.Single(x => x.IdProduct == product.Id & x.IdSupplier == supplier.Id);
                return p.BuyingPrice;
            }
        }
    }
}
