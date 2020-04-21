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
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    return from s in db.Suppliers
                           join sp in db.SupplierProducts
                           on s.Id equals sp.IdSupplier
                           where sp.IdProduct == Id
                           select s;

                }
            }
        }

        public void AssignSupplier(Guid supplierId, Decimal price)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                db.SupplierProducts.InsertOnSubmit(new SupplierProduct() { IdProduct = Id,
                                                                           IdSupplier = supplierId,
                                                                           BuyingPrice = price });
                db.SubmitChanges();
            }
        }
    }
}
