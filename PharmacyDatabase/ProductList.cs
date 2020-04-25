using System;
using System.Collections.Generic;
using System.IO;
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
                DataClassesDataContext db = new DataClassesDataContext();
                return db.Products;
            }
        }

        public void Add(string name, string manufacturer, bool prescription, decimal price)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                if (db.Products.Any(x => x.Name == name))
                    throw new Exception("Product with given name already exists");
                db.Products.InsertOnSubmit(new Product()
                {
                    Name = name,
                    Manufacturer = manufacturer,
                    Prescription = prescription,
                    SellingPrice = price
                });
                db.SubmitChanges();
            }
        }

        public void Edit(Product product, string name, string manufacturer, bool prescription, decimal price)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var p = db.Products.Single(x => x.Id == product.Id);
                p.Name = name;
                p.Manufacturer = manufacturer;
                p.Prescription = prescription;
                p.SellingPrice = price;
                db.SubmitChanges();
            }
        }

        public void Remove(Product product)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                db.Products.DeleteAllOnSubmit(from s in db.Products
                                              where s.Id == product.Id
                                              select s);
                db.SubmitChanges();
            }
        }

        public IEnumerable<Product> Search(string query)
        {
            return from p in Products
                   where p.Name.ToLower().Contains(query.ToLower()) ||
                         p.Manufacturer.ToLower().Contains(query.ToLower())
                   select p;
        }

        public void Import(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                string line;
                char separator = ';';
                while ((line = r.ReadLine()) != null)
                {
                    if (line.StartsWith("sep="))
                    {
                        separator = line[4];
                        continue;
                    }
                    var values = line.Split(separator);
                    if (values.Length != 4)
                        throw new FormatException("Invalid file");
                    Boolean prescription;
                    Decimal price;
                    if (!Boolean.TryParse(values[2], out prescription))
                        throw new FormatException("Invalid file: could not parse boolean " + values[2]);
                    if (!Decimal.TryParse(values[3], out price))
                        throw new FormatException("Invalid file: could not parse decimal " + values[3]);
                    db.Products.InsertOnSubmit(new Product()
                    {
                        Name = values[0],
                        Manufacturer = values[1],
                        Prescription = prescription,
                        SellingPrice = price
                    });

                }
                db.SubmitChanges();
            }
        }

        public void Export(string filePath)
        {
            using (StreamWriter w = new StreamWriter(filePath))
            {
                w.WriteLine("sep=;");
                foreach (Product p in Products)
                    w.WriteLine("{0};{1};{2};{3}", p.Name, p.Manufacturer, p.Prescription, p.SellingPrice);
            }
        }
    }
}
