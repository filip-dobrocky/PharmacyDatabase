﻿using System;
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

        private void lwRefresh()
        {
            lwProducts.ItemsSource = null;
            lwProducts.ItemsSource = pl.Products;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pl.Add((string)txtName.Text, (string)txtManuf.Text, chbPres.IsEnabled, decimal.Parse(txtPrice.Text));
                lwRefresh();
                lblError.Content = "";
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtSearch.Text))
                lwRefresh();
            else
                lwProducts.ItemsSource = pl.Search((string)txtSearch.Text);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnAdd.IsEnabled = !(String.IsNullOrWhiteSpace(txtManuf.Text) ||
                                 String.IsNullOrWhiteSpace(txtName.Text) ||
                                 String.IsNullOrWhiteSpace(txtPrice.Text));
        }





        //Test
    }
}
