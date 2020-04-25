using Microsoft.Win32;
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
        private ProductList pl = new ProductList();
        private SupplierList sl = new SupplierList();

        public MainWindow()
        {
            InitializeComponent();
            lwProducts.ItemsSource = pl.Products;
            cbSuppliers.ItemsSource = sl.Suppliers;
        }

        public void ProductViewRefresh()
        {
            lwProducts.ItemsSource = null;
            lwProducts.ItemsSource = pl.Products;
        }

        public void SupplierBoxRefresh()
        {
            cbSuppliers.ItemsSource = null;
            cbSuppliers.ItemsSource = sl.Suppliers;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Product item = (Product)lwProducts.SelectedItem;
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Delete item " + item.Name + "?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                pl.Remove(item);
                ProductViewRefresh();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow(ref pl);
            addProductWindow.Show();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditProductWindow editProductWindow = new EditProductWindow(ref pl, (Product)lwProducts.SelectedItem);
            editProductWindow.Show();
        }

        private void lwProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnDelete.IsEnabled = lwProducts.SelectedIndex != -1;
            cbSuppliers.SelectedIndex = -1;
            cbSuppliers.IsEnabled = lwProducts.SelectedIndex != -1;
            this.DataContext = (Product)lwProducts.SelectedItem;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtSearch.Text))
                ProductViewRefresh();
            else
                lwProducts.ItemsSource = pl.Search((string)txtSearch.Text);
        }

        private void suppliers_Click(object sender, RoutedEventArgs e)
        {
            SuppliersWindow suppliersWindow = new SuppliersWindow(ref sl);
            suppliersWindow.Show();
        }

        private void importProducts_Click(object sender, RoutedEventArgs e)
        {
            string path = getImportFilePath();
            if (path != null)
            {
                pl.Import(path);
                ProductViewRefresh();
            }
        }

        private void exportProducts_Click(object sender, RoutedEventArgs e)
        {
            string path = getExportFilePath();
            if (path != null)
            {
                pl.Export(path);
            }
        }

        private void importSuppliers_Click(object sender, RoutedEventArgs e)
        {
            string path = getImportFilePath();
            if (path != null)
            {
                sl.Import(path);
            }
        }

        private void exportSuppliers_Click(object sender, RoutedEventArgs e)
        {
            string path = getExportFilePath();
            if (path != null)
            {
                sl.Export(path);
            }
        }

        private string getImportFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;
            else
                return null;
        }

        private string getExportFilePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
                return saveFileDialog.FileName;
            else
                return null;
        }

        private void cbSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtPrice.IsEnabled = cbSuppliers.SelectedIndex != -1 && lwProducts.SelectedIndex != -1;
            btnAssign.IsEnabled = !String.IsNullOrWhiteSpace(txtPrice.Text);
        }

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            decimal price;
            decimal.TryParse(txtPrice.Text, out price);
            ((Product)lwProducts.SelectedItem).AssignSupplier((Supplier)cbSuppliers.SelectedItem, price);
        }

        private void txtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal price;
            btnAssign.IsEnabled = decimal.TryParse(txtPrice.Text, out price);
        }
    }
}
