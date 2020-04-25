using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmacyDatabase
{
    public class AddProductWindow : ProductWindow 
    {
        public AddProductWindow(ref ProductList productList) : base(ref productList)
        {
            Title = "Add Product";
            btnConfirm.Content = "Add";
        }

        protected override void confirmAction()
        {
            pl.Add((string)txtName.Text, (string)txtManuf.Text, (bool)chbPres.IsChecked, decimal.Parse(txtPrice.Text));
        }
    }
}
