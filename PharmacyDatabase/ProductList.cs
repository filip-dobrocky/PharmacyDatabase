using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDatabase
{
    public class ProductList
    {
        private DataClassesDataContext _db = new DataClassesDataContext();

        public IQueryable<Product> Products
        {
            get { return _db.Products; }
        }

        public void Add(string name, string manufacturer, bool prescription)
        {
            if (_db.Products.Any(x => x.Name == name))
                throw new Exception("Product with given name already exists");
            _db.Products.InsertOnSubmit(new Product() { Name = name,
                                                        Manufacturer = manufacturer,
                                                        Prescription = prescription });
            _db.SubmitChanges();
        }

        public void Remove(Guid id)
        {
            _db.Products.DeleteAllOnSubmit(from s in _db.Products
                                           where s.Id == id
                                           select s);
            _db.SubmitChanges();
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
