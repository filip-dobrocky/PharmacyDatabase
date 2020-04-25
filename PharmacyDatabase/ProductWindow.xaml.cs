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
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public abstract partial class ProductWindow : Window
    {
        protected ProductList pl;

        public ProductWindow(ref ProductList productList)
        {
            InitializeComponent();
            pl = productList;
        }

        //protected abstract void btnConfirm_Click(object sender, RoutedEventArgs e);

        protected abstract void confirmAction();

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                confirmAction();
                ((MainWindow)Application.Current.MainWindow).ProductViewRefresh();
                this.Close();
            }
            catch (FormatException)
            {
                lblError.Content = "Invalid price format";
            }
            catch (Exception addException)
            {
                lblError.Content = addException.Message;
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnConfirm.IsEnabled = !(String.IsNullOrWhiteSpace(txtManuf.Text) ||
                                     String.IsNullOrWhiteSpace(txtName.Text) ||
                                     String.IsNullOrWhiteSpace(txtPrice.Text));
        }
    }
}
