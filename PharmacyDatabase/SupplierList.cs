using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDatabase
{
    public class SupplierList
    {
        public IQueryable<Supplier> Suppliers
        {
            get
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                    return db.Suppliers;
            }
        }

        public void Add(string name)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                if (db.Suppliers.Any(x => x.Name == name))
                    throw new Exception("Supplier with given name already exists");
                db.Suppliers.InsertOnSubmit(new Supplier() { Name = name });
                db.SubmitChanges();
            }
        }

        public void Remove(Guid id)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                db.Suppliers.DeleteAllOnSubmit(from s in db.Suppliers
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
