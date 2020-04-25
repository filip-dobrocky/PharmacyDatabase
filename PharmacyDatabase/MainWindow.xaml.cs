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

        public MainWindow()
        {
            InitializeComponent();
            lwProducts.ItemsSource = pl.Products;
        }

        public void ProductViewRefresh()
        {
            lwProducts.ItemsSource = null;
            lwProducts.ItemsSource = pl.Products;
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
            this.DataContext = (Product)lwProducts.SelectedItem;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtSearch.Text))
                ProductViewRefresh();
            else
                lwProducts.ItemsSource = pl.Search((string)txtSearch.Text);
        }
    }
}
