using System;
using System.Collections.Generic;
using System.IO;
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

        public void Edit(Supplier supplier, string name)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var s = db.Suppliers.Single(x => x.Id == supplier.Id);
                s.Name = name;
                db.SubmitChanges();
            }
        }

        public void Remove(Supplier supplier)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                db.Suppliers.DeleteAllOnSubmit(from s in db.Suppliers
                                               where s.Id == supplier.Id
                                               select s);
                db.SubmitChanges();
            }
        }

        public IEnumerable<Supplier> Search(string query)
        {
            return from s in Suppliers
                   where s.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                   select s;
        }

        public void Import(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (!db.Suppliers.Any(x => x.Name == line))
                        db.Suppliers.InsertOnSubmit(new Supplier() { Name = line });
                }
                db.SubmitChanges();
            }
        }

        public void Export(string filePath)
        {
            using (StreamWriter w = new StreamWriter(filePath))
            {
                foreach (Supplier s in Suppliers)
                    w.WriteLine(s.Name);
            }
        }
    }
}
