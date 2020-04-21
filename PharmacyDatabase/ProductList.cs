using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDatabase
{
    public class ProductList
    {
        public IQueryable<Product> Products
        {
            get
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                    return db.Products;
            }
        }

        public void Add(string name, string manufacturer, bool prescription)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                if (db.Products.Any(x => x.Name == name))
                    throw new Exception("Product with given name already exists");
                db.Products.InsertOnSubmit(new Product()
                {
                    Name = name,
                    Manufacturer = manufacturer,
                    Prescription = prescription
                });
                db.SubmitChanges();
            }
        }

        public void Remove(Guid id)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                db.Products.DeleteAllOnSubmit(from s in db.Products
                                              where s.Id == id
                                              select s);
                db.SubmitChanges();
            }
        }

        public void Import(string filePath)
        {
            //TODO: implement
        }

        public void Export(string filePath)
        {
            //TODO: implement
        }
    }
}
