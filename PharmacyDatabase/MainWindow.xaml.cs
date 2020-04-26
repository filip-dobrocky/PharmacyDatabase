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
        private Inventory inventory = new Inventory();

        public MainWindow()
        {
            InitializeComponent();
            lwProducts.ItemsSource = pl.Products;
            lwInventory.ItemsSource = inventory.AvailableProducts;
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

        public void InventoryViewRefresh()
        {
            lwInventory.ItemsSource = null;
            lwInventory.ItemsSource = inventory.AvailableProducts;
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
            if (cbSuppliers.IsEnabled = cbProductSuppliers.IsEnabled = btnEdit.IsEnabled = btnDelete.IsEnabled = lwProducts.SelectedIndex != -1)
            {
                cbSuppliers.SelectedIndex = -1;
                lwInventory.SelectedIndex = -1;
                txtSell.IsEnabled = btnSell.IsEnabled = false;
                cbProductSuppliers.ItemsSource = ((Product)lwProducts.SelectedItem).Suppliers;
                this.DataContext = (Product)lwProducts.SelectedItem;
            }
        }

        private void lwInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtSell.IsEnabled = btnSell.IsEnabled = lwInventory.SelectedIndex != -1)
            {
                lwProducts.SelectedIndex = -1;
                txtSell.Text = ((Product)lwInventory.SelectedItem).AvailableAmount.ToString();
                cbProductSuppliers.IsEnabled = txtResupply.IsEnabled = btnResupply.IsEnabled = false;
                this.DataContext = (Product)lwInventory.SelectedItem;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                ProductViewRefresh();
                InventoryViewRefresh();
            }
            else
            {
                switch (((TextBox)sender).Name)
                {
                    case "txtSearch":
                        lwProducts.ItemsSource = pl.Search((string)((TextBox)sender).Text);
                        break;
                    case "txtSearchInv":
                        lwInventory.ItemsSource = inventory.Search((string)((TextBox)sender).Text);
                        break;
                }  
            }   
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
            ((Product)lwProducts.SelectedItem).AssignSupplier((Supplier)cbSuppliers.SelectedItem, decimal.Parse(txtPrice.Text));
            MessageBox.Show(string.Format("{0} now supplies {1} for {2}", 
                ((Supplier)cbSuppliers.SelectedItem).Name, 
                ((Product)lwProducts.SelectedItem).Name, 
                txtPrice.Text));
            lwProducts.SelectedIndex = -1;
        }

        private void txtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal price;
            btnAssign.IsEnabled = decimal.TryParse(txtPrice.Text, out price);
        }

        private void cbProductSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtResupply.IsEnabled = cbProductSuppliers.SelectedIndex != -1;
            int amount;
            if (btnResupply != null)
                btnResupply.IsEnabled = int.TryParse(txtSell.Text, out amount);
            if (cbProductSuppliers.SelectedIndex == -1)
                lblBuyingPrice.Content = "";
            else
                lblBuyingPrice.Content = string.Format("Buying price: {0} per piece",
                    Product.GetBuyingPrice((Product)lwProducts.SelectedItem, (Supplier)cbProductSuppliers.SelectedItem)); 
        }

        private void txtResupply_TextChanged(object sender, TextChangedEventArgs e)
        {
            int amount;
            if (btnResupply != null)
                btnResupply.IsEnabled = int.TryParse(txtResupply.Text, out amount);
        }

        private void btnResupply_Click(object sender, RoutedEventArgs e)
        {
            inventory.Resupply((Product)lwProducts.SelectedItem, (Supplier)cbProductSuppliers.SelectedItem, int.Parse(txtResupply.Text));
            InventoryViewRefresh();
            MessageBox.Show(string.Format("Resupplied {0}pc(s) of {1} from {2} for total amount of {3}",
                txtResupply.Text,
                ((Product)lwProducts.SelectedItem).Name,
                ((Supplier)cbProductSuppliers.SelectedItem).Name,
                int.Parse(txtResupply.Text) * Product.GetBuyingPrice((Product)lwProducts.SelectedItem, (Supplier)cbProductSuppliers.SelectedItem)
                ));
        }

        private void txtSell_TextChanged(object sender, TextChangedEventArgs e)
        {
            int amount;
            if (btnSell != null)
                btnSell.IsEnabled = int.TryParse(txtSell.Text, out amount);
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            inventory.Sell((Product)lwInventory.SelectedItem, int.Parse(txtSell.Text));
            MessageBox.Show(string.Format("Sold {0} pc(s) of {1} for total amount of {2}",
                txtSell.Text,
                ((Product)lwInventory.SelectedItem).Name,
                ((Product)lwInventory.SelectedItem).SellingPrice * int.Parse(txtSell.Text)
                ));
            lwInventory.SelectedIndex = -1;
            InventoryViewRefresh();
        }
    }
}
