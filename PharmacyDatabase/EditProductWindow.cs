using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmacyDatabase
{
    public class EditProductWindow : ProductWindow
    {
        private Product p;

        public EditProductWindow(ref ProductList productList, Product product) : base(ref productList)
        {
            Title = "Edit Product " + product.Name;
            btnConfirm.Content = "Edit";
            p = product;
            txtName.Text = p.Name;
            txtManuf.Text = p.Manufacturer;
            txtPrice.Text = p.SellingPrice.ToString();
            chbPres.IsChecked = p.Prescription; 
        }

        protected override void confirmAction()
        {
            pl.Edit(p, (string)txtName.Text, (string)txtManuf.Text, (bool)chbPres.IsChecked, decimal.Parse(txtPrice.Text));
        }
    }
}
