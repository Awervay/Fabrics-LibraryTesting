using Fabrics.DataBase;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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

namespace Fabrics
{
    /// <summary>
    /// Логика взаимодействия для AddOrUpdateProduct.xaml
    /// </summary>
    public partial class AddOrUpdateProduct : Window
    {
        VinidiktovDay5Entities DB = new VinidiktovDay5Entities();
        Product isProduct = new Product();
        
        public AddOrUpdateProduct()
        {
            InitializeComponent();
            var items = DB.Product.Select(f => f.ManufactureProduct.Name).Distinct().ToList();
            ManufactureProduct.ItemsSource = items;

        }

        public int id_Product;

        public AddOrUpdateProduct(InfoProduct INF)
        {
            InitializeComponent();
            id_Product = INF.ID_Product;
            var pris = DB.Product.Find(INF.ID_Product);
            TitleProduct.Text = pris.Name;
            PriceProduct.Text = pris.Cost.ToString();
            CountProduct.Text = pris.QuantityStock.ToString();
            DescriptionProduct.Text = pris.DescriptionProduct;
            ManufactureProduct.SelectedItem = INF.Manufacture;
            var items = DB.Product.Select(f => f.ManufactureProduct.Name).Distinct().ToList();
            ManufactureProduct.ItemsSource = items;
            IconProduct.Source = INF.Image;
            if (!string.IsNullOrEmpty(INF.Photo))
            {
                isProduct.Photo = @"\Resource\" + INF.Photo.Split('\\').Last();
            }
        }

        private void AddOrEditProductBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TitleProduct.Text.ToString() == ""
                    || PriceProduct.Text.Length <= 0
                    || ManufactureProduct.Text.Length <= 0
                    || DescriptionProduct.Text == ""
                    || CountProduct.Text.Length <= 0)
                {
                    MessageBox.Show("Заполните все поля ввода!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (Convert.ToInt32(PriceProduct.Text) < 0)
                {
                    MessageBox.Show("Стоимость не может быть отрицательной!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (Convert.ToInt32(CountProduct.Text) < 0)
                {
                    MessageBox.Show("Товар не может быть отрицательным!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Product serv = new Product();
                    serv.Name = TitleProduct.Text;
                    serv.Cost = Convert.ToInt32(PriceProduct.Text);
                    serv.Name = Convert.ToString(ManufactureProduct.Text);
                    serv.DescriptionProduct = DescriptionProduct.Text;
                    serv.QuantityStock = Convert.ToInt32(CountProduct.Text);
                    serv.Photo = isProduct.Photo;
                    DB.Product.AddOrUpdate(serv);
                    DB.SaveChanges();
                    ProductsForm main = new ProductsForm();
                    main.Show();
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ситуация", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Users users = new Users();
            users.ID_Role = 1;
            ProductsForm main = new ProductsForm(users);
            main.Show();
            Close();
        }

        public string path;

        private void ChoosePhotoProduct_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
            "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
            "Portable Network Graphic (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                IconProduct.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                path = openFileDialog.FileName;
                string dir = Environment.CurrentDirectory + @"\Resource\";
                if (!(File.Exists(dir + openFileDialog.FileName.Split('\\').Last())))
                    File.Copy(path, dir + openFileDialog.FileName.Split('\\').Last());
                isProduct.Photo = @"\Resource\" + openFileDialog.FileName.Split('\\').Last();
            }
        }
    }
}
