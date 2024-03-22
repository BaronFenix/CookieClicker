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

namespace CookieClickerWPF
{
    /// <summary>
    /// Логика взаимодействия для LotteryWindow.xaml
    /// </summary>
    public partial class LotteryWindow : Window
    {
        public LotteryWindow()
        {
            InitializeComponent();
            lottery_cookie_counter.Text = $"{ MainWindow.cookie_counter }";

        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Lottery(object sender, RoutedEventArgs e)
        {

            if (MainWindow.cookie_counter >= 100)
            {
                MainWindow.cookie_counter -= 100;
                lottery_cookie_counter.Text = $"{MainWindow.cookie_counter}";

                Random rand = new Random();
                double randNum = rand.NextDouble();
                if (randNum < 0.1)
                {
                    MainWindow.cookie_counter += 1000;
                    mainMessage.Text = @"ПОЗДРАВЛЯЕМ ВЫ ВЫИИГРАЛИ
1000 ПЕЧЕНЕК!!!";
                }
                else mainMessage.Text = @"Вы ничего не выииграли :( 
Ничего в следующий раз повезет!";
            }
        }
    }
}
