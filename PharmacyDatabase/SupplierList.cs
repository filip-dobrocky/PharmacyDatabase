using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDatabase
{
    public class SupplierList
    {
        private DataClassesDataContext _db = new DataClassesDataContext();

        public IQueryable<Supplier> Suppliers
        {
            get { return _db.Suppliers; }
        }

        public void Add(string name)
        {
            if (_db.Suppliers.Any(x => x.Name == name))
                throw new Exception("Supplier with given name already exists");
            _db.Suppliers.InsertOnSubmit(new Supplier() { Name = name });
            _db.SubmitChanges();
        }

        public void Remove(Guid id)
        {
            _db.Suppliers.DeleteAllOnSubmit(from s in _db.Suppliers
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
