using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PharmacyDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProductList pl = new ProductList();
        public MainWindow()
        {
            InitializeComponent();
            LVRefresh();
        }

        private class ListViewItem
        {
            public string Name
            { get; set; }
            public decimal Price
            { get; set; }
        }
        public void LVRefresh()
        {
            var items = from item in pl.Products
                        select new ListViewItem { Name = item.Name, Price = item.SellingPrice };
            foreach (var item in items)
            {
                lwProducts.Items.Add(item);
            }
        }



        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            pl.Add((string)txtName.Text, (string)txtManuf.Text, chbPres.IsEnabled, decimal.Parse(txtPrice.Text));
            LVRefresh();

        }





        //Test
    }
}
