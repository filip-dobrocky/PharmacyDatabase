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
using System.Windows.Shapes;

namespace PharmacyDatabase
{
    /// <summary>
    /// Interaction logic for SuppliersWindow.xaml
    /// </summary>
    public partial class SuppliersWindow : Window
    {
        private SupplierList sl;

        public SuppliersWindow(ref SupplierList supplierList)
        {
            InitializeComponent();
            sl = supplierList;
            lbSuppliers.ItemsSource = sl.Suppliers;
        }

        public void LBRefresh()
        {
            lbSuppliers.ItemsSource = null;
            lbSuppliers.ItemsSource = sl.Suppliers;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            sl.Add(txtName.Text);
            LBRefresh();
            ((MainWindow)Application.Current.MainWindow).SupplierBoxRefresh();
            txtName.Text = "";
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            sl.Remove((Supplier)lbSuppliers.SelectedItem);
            LBRefresh();
            ((MainWindow)Application.Current.MainWindow).SupplierBoxRefresh();
        }

        private void lbSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnRemove.IsEnabled = lbSuppliers.SelectedIndex != -1;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnAdd.IsEnabled = !String.IsNullOrWhiteSpace(txtName.Text);
        }
    }
}
