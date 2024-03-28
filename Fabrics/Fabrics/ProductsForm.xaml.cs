using Fabrics.DataBase;
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

namespace Fabrics
{
    /// <summary>
    /// Логика взаимодействия для ProductsForm.xaml
    /// </summary>
    public partial class ProductsForm : Window
    {
        VinidiktovDay5Entities DB = new VinidiktovDay5Entities();
        public List<InfoProduct> listMain;
        Users user;
        public ProductsForm()
        {
            InitializeComponent();
            listMain = DB.InfoProduct.ToList();
            AutoLoad();
            ListView listView = ProductList;
            int totalCount = listView.Items.Count;
            int displayedCount = listView.Items.Count;
            CountInf.Text = $"Количество записей: {displayedCount} из {totalCount}";
            SortProductCmb.Items.Add("Весь список");
            SortProductCmb.Items.Add("По убыванию");
            SortProductCmb.Items.Add("По возрастанию");
            var items = DB.InfoProduct.Select(f => f.Manufacture).Distinct().ToList();
            items.Insert(0, "Весь список");
            FilterProductCmb.ItemsSource = items;
            AddProductBtn.Visibility = Visibility.Hidden;
            RedactProductBtn.Visibility = Visibility.Hidden;
            DeleteProductBtn.Visibility = Visibility.Hidden;
            FilterProductCmb.Visibility = Visibility.Hidden;
            SerchVis.Visibility = Visibility.Hidden;
            CostVis.Visibility = Visibility.Hidden;
            Manuf.Visibility = Visibility.Hidden;
            SortProductCmb.Visibility = Visibility.Hidden;
            FindLineTxb.Visibility = Visibility.Hidden;

        }
        public ProductsForm(Users users)
        {
            InitializeComponent();
            user = users;
            listMain = DB.InfoProduct.ToList();
            AutoLoad();
            ListView listView = ProductList;
            int totalCount = listView.Items.Count;
            int displayedCount = listView.Items.Count;
            CountInf.Text = $"Количество записей: {displayedCount} из {totalCount}";
            SortProductCmb.Items.Add("Весь список");
            SortProductCmb.Items.Add("По убыванию");
            SortProductCmb.Items.Add("По возрастанию");
            var items = DB.InfoProduct.Select(f => f.Manufacture).Distinct().ToList();
            items.Insert(0, "Весь список");
            FilterProductCmb.ItemsSource = items;
            OpenFormTB.Text = $"Добро пожаловать {users.FIO}";
            AddProductBtn.Visibility = Visibility.Hidden;
            RedactProductBtn.Visibility = Visibility.Hidden; 
            DeleteProductBtn.Visibility = Visibility.Hidden;
            if (user.ID_Role == 1)
            {
                AddProductBtn.Visibility = Visibility.Visible;
                RedactProductBtn.Visibility = Visibility.Visible; 
                DeleteProductBtn.Visibility = Visibility.Visible;
            }
        }

        public void AutoLoad()
        {
            List<InfoProduct> List = listMain;
            ProductList.ItemsSource = List;
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            AddOrUpdateProduct add = new AddOrUpdateProduct();
            add.Show();
            Close();
        }

        private void RedactProductBtn_Click(object sender, RoutedEventArgs e)
        {
                int index = 0; // индекс строки, которую вы хотите выбрать
                if (index >= 0 && index < ProductList.Items.Count)
                {
                ProductList.SelectedIndex = index;
                }
                var rowselected = (InfoProduct)ProductList.SelectedItem;
                AddOrUpdateProduct add = new AddOrUpdateProduct(rowselected);
                add.Show();
                Close();
        }

        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            var dialogwindow = MessageBox.Show("Вы уверены, что хотите удалить данные?", "Подтверждение действия!", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogwindow == MessageBoxResult.Yes)
            {
                var selectrow = (InfoProduct)ProductList.SelectedItem;
                if (selectrow != null)
                {
                    var searchTable = DB.Product.FirstOrDefault(x => x.ID_Product == selectrow.ID_Product);
                    if (searchTable != null)
                    {
                        // Проверка наличия связанных записей в таблице OrderProduct
                        var orderProducts = DB.OrderProduct.Where(op => op.ID_Product == searchTable.ID_Product).ToList();
                        foreach (var op in orderProducts)
                        {
                            DB.OrderProduct.Remove(op);
                        }

                        DB.Product.Remove(searchTable);
                        DB.SaveChanges();

                        DB = new VinidiktovDay5Entities();
                        ProductList.ItemsSource = DB.InfoProduct.ToList();
                    }
                }
                else
                {
                    MessageBox.Show("Необходимо выбрать строку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            }
            catch { }
        }

        private void OutMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }

        private void SortProductCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /// <summary>
            /// Сортировка данных по убыванию и возрастанию цены услуги
            /// </summary>
            try
            {
                if (SortProductCmb.SelectedIndex == 0)
                    listMain = DB.InfoProduct.ToList();
                else if (SortProductCmb.SelectedIndex == 1)
                    listMain = listMain.OrderBy(f => f.Cost).ToList();
                else if (SortProductCmb.SelectedIndex == 2)
                    listMain = listMain.OrderByDescending(f => f.Cost).ToList();
                ProductList.ItemsSource = listMain;
                ListView listView = ProductList;
                int totalCount = listView.Items.Count;
                int displayedCount = listView.Items.Count;
                CountInf.Text = $"Количество записей: {displayedCount} из {totalCount}";
            }
            catch
            {
                MessageBox.Show("Проблемы сортировки", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterProductCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /// <summary>
            /// Фильтрация скидки по указанному значению в ComboBox'е
            /// </summary>
            try
            {
                if (FilterProductCmb.SelectedItem != null)
                {
                    if (FilterProductCmb.SelectedItem.ToString() == "Весь список")
                    {
                        ProductList.ItemsSource = DB.InfoProduct.ToList();
                        ListView listView = ProductList;
                        int totalCount = listView.Items.Count;
                        int displayedCount = listView.Items.Count;
                        CountInf.Text = $"Количество записей: {displayedCount} из {totalCount}";
                    }
                    else
                    {
                        ProductList.ItemsSource = DB.InfoProduct.Where
                            (f => f.Manufacture == FilterProductCmb.SelectedItem.ToString()).ToList();
                        ListView listView = ProductList;
                        int totalCount = listView.Items.Count;
                        int displayedCount = listView.Items.Count;
                        CountInf.Text = $"Количество записей: {displayedCount} из {totalCount}";
                    }
                }
            }
            catch
            {
                MessageBox.Show("Проблемы фильтрации", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FindLineTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var filtered = listMain.ToList();
                var resultFilter = filtered.Where(x => x.GetType().GetProperties().
                Any(p => p.GetValue(x, null)?.ToString().ToLower().
                Contains(FindLineTxb.Text) ?? false)).ToList();
                ProductList.ItemsSource = resultFilter;
                ListView listView = ProductList;
                int totalCount = listView.Items.Count;
                int displayedCount = listView.Items.Count;
                CountInf.Text = $"Количество записей: {displayedCount} из {totalCount}";
                if (resultFilter.Count == 0)
                {
                    ResultInfo.Visibility = Visibility.Visible;
                    ResultInfo.Text = "По результату поиска ничего не найдено!";
                }
                else
                {
                    ResultInfo.Visibility = Visibility.Hidden;
                }
            }
            catch 
            {
                MessageBox.Show("Проблемы поиска", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
