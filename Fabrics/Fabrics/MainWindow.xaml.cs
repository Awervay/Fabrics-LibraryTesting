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
using TestingLibraryCalculation;
using WSUniversalLib;
using System.Windows.Navigation;
using Fabrics.DataBase;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Fabrics
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer = new DispatcherTimer();
        VinidiktovDay5Entities DB = new VinidiktovDay5Entities();
        public MainWindow()
        {
            InitializeComponent();
        }

        public int sec = 10;

        public void timer()
        {
            Timer.Interval = new TimeSpan(0, 0, 0, 1);
            Timer.Tick += DispatcherTimer_Tick;
            Timer.Start();
        }

        void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (sec > 0)
            {
                sec--;
                Autorization.IsEnabled = false;
                TimeClose.Visibility = Visibility.Visible;
                Okay.IsEnabled = false;
                TimeClose.Text = string.Format
                    ("Вход заблокирован на 00:0{0}:{1} секунд", sec / 60, sec % 60);
            }
            else if (sec == 0)
            {
                Timer.Tick -= DispatcherTimer_Tick;
                Timer.Stop();
                Autorization.IsEnabled = true;
                Okay.IsEnabled = true;
                TimeClose.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Генерация рандомных символов
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string RandomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new StringBuilder();
            Random random = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = pool[random.Next(0, pool.Length)];
                builder.Append(c);
            }
            return builder.ToString();
        }
        /// <summary>
        /// Генерация Captch
        /// </summary>
        private void CanvasAdd()
        {
            Random random = new Random();

            for (int i = 0; i < 20; i++)
            {
                Line line = new Line
                {
                    X1 = random.Next((int)Canvas_Captcha.ActualWidth),
                    Y1 = random.Next((int)Canvas_Captcha.ActualHeight),
                    X2 = random.Next((int)Canvas_Captcha.ActualWidth),
                    Y2 = random.Next((int)Canvas_Captcha.ActualHeight),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                Canvas_Captcha.Children.Add(line);
            }

            for (int i = 0; i < 100; i++)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = 1,
                    Height = 1,
                    Fill = Brushes.Red
                };

                Canvas.SetLeft(ellipse, random.Next((int)Canvas_Captcha.ActualWidth));
                Canvas.SetTop(ellipse, random.Next((int)Canvas_Captcha.ActualHeight));

                Canvas_Captcha.Children.Add(ellipse);
            }
        }

        private void OnCaptcha(Visibility a, bool b)
        {
            Captcha_Obj.Visibility = a;
            Autorization.IsEnabled = b;
        }

        private int count_Click = 0;

        private void Autorization_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text == string.Empty || Password.Password == string.Empty)
            {
                MessageBox.Show("Введите логин или пароль!", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!DB.Users.Any(u => u.Login == Login.Text && (u.Password == Password.Password ||
            u.Password == OpenPassword.Text)))
            //Если в базе нет такого пользователя или пароля    
            {
                MessageBox.Show("Данного аккаунта не существует", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (DB.Users.Any(u => u.Login == Login.Text && (u.Password == Password.Password ||
            u.Password == OpenPassword.Text) && u.ID_Role == 1))
            {
                var users = DB.Users.First(i => i.Login == Login.Text && i.Password == Password.Password);
                ProductsForm adm = new ProductsForm(users);
                adm.Show();
                Close();
            }
            else if (DB.Users.Any(u => u.Login == Login.Text && (u.Password == Password.Password ||
            u.Password == OpenPassword.Text) && (u.ID_Role == 2 || u.ID_Role == 3)))
            {
                if (DB.Users.Any(u => u.ID_Role == 2))
                {
                    var cl = DB.Users.First(i => i.Login == Login.Text && i.Password == Password.Password);
                    ProductsForm client = new ProductsForm(cl);
                    client.Show();
                    Close();
                }
                else if (DB.Users.Any(u => u.ID_Role == 3))
                {
                    var mn = DB.Users.First(i => i.Login == Login.Text && i.Password == Password.Password);
                    ProductsForm manager = new ProductsForm(mn);
                    manager.Show();
                    Close();
                }
            }
            count_Click++;
            //После первой попытки входа, открывается Captcha
            if (count_Click == 1)
            {
                OnCaptcha(Visibility.Visible, false);
                //Включает Капчу и выключает кнопку авторизации
                Captcha_Textbox.Text = RandomString(4);
                CanvasAdd();
            }
        }

        private void Okay_Click(object sender, RoutedEventArgs e)
        {
            if (Captcha_Textbox.Text == Captcha_Textbox2.Text)
            {
                MessageBox.Show("Успешно!");
                OnCaptcha(Visibility.Hidden, true);

                count_Click = 0;
            }
            if (Captcha_Textbox.Text != Captcha_Textbox2.Text)
            {
                MessageBox.Show("Ошибка!");
                timer();
                sec = 10;
                RandomString(4);
                CanvasAdd();
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            Canvas_Captcha.Children.Clear();
            Captcha_Textbox.Text = RandomString(4);
            CanvasAdd();
        }

        public int c = 1;
        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (c == 1)
            {
                OpenPassword.Text = Password.Password; //Отображаем пароль
                OpenPassword.Visibility = Visibility.Visible;
                Password.Visibility = Visibility.Hidden;//Скрываем PasswordBox
                c = 2;
            }
            else
            {
                if (c == 2)
                {
                    Password.Password = OpenPassword.Text;
                    OpenPassword.Visibility = Visibility.Hidden;
                    Password.Visibility = Visibility.Visible;
                    c = 1;
                }
            }
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            ProductsForm pr = new ProductsForm();
            pr.Show();
            Close();
        }
    }
}
