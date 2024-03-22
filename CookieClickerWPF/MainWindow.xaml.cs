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

namespace CookieClickerWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            multiplier_cost_UI.Text = $"{multiplier_cost}";
            d_cookie_cost_UI.Text = $"{double_cookie_cost}";
            lucky_cost_UI.Text = $"{lucky_cost}";
            lucky_add_cost_UI.Text = $"{lucky_add_cost}"; 
            enhancer_cost_UI.Text = $"{chance_enhancer_cost}";
            lottery_cost_UI.Text = $"{lottery_cost}";
            bribe_cost_UI.Text = $"{bribe_cost}";
        }
        
        // 
        private Random rand = new Random();
        public static int cookie_counter, cookie_per_click = 1;

        // Удвоитель
        private bool double_cookie = false;
        private int double_cookie_cost = 50;

        // Способность удачливый
        private bool lucky = false;
        private int lucky_cost = 100;
        double lucky_chance_percent = 0.01;
        int lucky_bonus = 100;

        int chance_enhancer_cost = 120, chance_enhancer_lvl = 0;

        int lucky_add_lvl, lucky_add_cost = 120;
        
        // Лотерея 
        int lottery_cost = 100;
        double lottery_percent = 0.1;

        // Подкуп
        int bribe_cost = 280, bribe_lvl;

        // Множитель
        private int multiplier_lvl = 0, multiplier_cost = 250;



        // Основная кнопка-печенька
        private void Cookie_Click(object sender, RoutedEventArgs e)
        {
            cookie_counter += (GetMultiplierCookie(cookie_per_click));
            counter_txt.Text = $"{cookie_counter}";

            double randNum = rand.NextDouble();
            
            if (lucky && randNum < lucky_chance_percent)
            {
                MessageBox.Show("YOU WON 100 COOKIES!!!!!!!");
                cookie_counter += lucky_bonus;
            }

            PriceCheckerUI(double_cookie_cost, d_cookie_cost_UI, double_cookie);
            PriceCheckerUI(lucky_cost, lucky_cost_UI, lucky);
            PriceCheckerUI(multiplier_cost, multiplier_cost_UI);
            PriceCheckerUI(lottery_cost, lottery_cost_UI);
            PriceCheckerUI(chance_enhancer_cost, enhancer_cost_UI, chance_enhancer_lvl);
            PriceCheckerUI(lucky_add_cost, lucky_add_cost_UI, lucky_add_lvl);
            PriceCheckerUI(bribe_cost, bribe_cost_UI, bribe_lvl);

        }

        // ------------------------------------------------------------------------
        // методы для кнопок способностей

        private void DoubleCookie(object sender, RoutedEventArgs e)
        {
            if(!BuyClickChecker(double_cookie_cost))
                return;
            if(cookie_counter >= double_cookie_cost && !double_cookie)
            {
                MessageBox.Show("Вы приобрели способность Удвоитель!", "UPDATE");
                double_cookie = true;
                EatingCookies(double_cookie_cost);
                cookie_per_click *= 2;
            }
            if (double_cookie)
            {
                DisablingButton(x2_btn, d_cookie_cost_UI);
                return;
            }
        }

        private void Lucky(object sender, RoutedEventArgs e)
        {
            if (!BuyClickChecker(lucky_cost))
                return;
            if (cookie_counter >= lucky_cost && !lucky)
            {
                lucky = true;
                EatingCookies(lucky_cost);
            }
            if (double_cookie)
            {
                DisablingButton(luccky_btn, lucky_cost_UI);
                return;
            }
        }

        private void Lottery(object sender, RoutedEventArgs e)
        {
            var lotteryWindow = new LotteryWindow();

            if (!BuyClickChecker(lottery_cost))
                return;
            if (cookie_counter >= lottery_cost)
            {

                lotteryWindow.ShowDialog();
                //EatingCookies(lottery_cost);
                //double randNum = rand.NextDouble();
                //if (randNum < lottery_percent)
                //{
                //    MessageBox.Show("ВЫ ВЫИГРАЛИ 1000 ПЕЧЕНЕК!!!");
                //    cookie_counter += 1000;
                //}
            }
        }

        // ------------------------------------------------------------------------
        // методы для кнопок улучшений

        // Улучшение умножитель
        private void Multiplier(object sender, RoutedEventArgs e)
        {
            if(!BuyClickChecker(multiplier_cost))
                return;

            if (multiplier_lvl < 3 && cookie_counter >= multiplier_cost)
            {
                EatingCookies(multiplier_cost);
                multiplier_cost *= 2;
                multiplier_lvl++;
                multiplier_cost_UI.Text = $"{multiplier_cost}";
                multiplier_tooltip.Text = $@"Множитель - Умножает количество получаемых печенек за клик
Уровень 1: умножает на 0 - 3
Уровень 2: умножает на 0 - 4
Уровень 3: умножает на 0 - 5
Текущий уровень {multiplier_lvl}";
            }
            if(multiplier_lvl == 3)
                DisablingButton(multipler_btn, multiplier_cost_UI);
        }

        // Улучшение шанса счастливчик
        private void ChanceEnhancer(object sender, RoutedEventArgs e)
        {
            if (!BuyClickChecker(chance_enhancer_cost))
                return;
            if(chance_enhancer_lvl < 10 && cookie_counter > chance_enhancer_cost && lucky)
            {
                EatingCookies(chance_enhancer_cost);
                lucky_chance_percent += 0.01;
                chance_enhancer_lvl++;
                chance_enhancer_cost = Convert.ToInt32(chance_enhancer_cost * 1.5);
                enhancer_cost_UI.Text = $"{chance_enhancer_cost}";
                enhancer_tooltip.Text = $@"Усилитель шанса - Повышает базовый шанс способности удачливый на 1%
Текущий шанс: {1 + chance_enhancer_lvl}%";
            }
            if (chance_enhancer_lvl == 10)
                DisablingButton(enhancer_btn, enhancer_cost_UI);

            if (!lucky)
                MessageBox.Show("Для улучшения способности нужно ее приобрести", "Купите способность", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Улучшение награды счстливчик
        private void LuckyAddCookie(object sender, RoutedEventArgs e)
        {
            if (!BuyClickChecker(lucky_add_cost))
                return;
            if (lucky_add_lvl < 10 && cookie_counter > lucky_add_cost && lucky)
            {
                EatingCookies(lucky_add_cost);
                lucky_bonus += 10;
                lucky_add_lvl++;
                lucky_add_cost = Convert.ToInt32(lucky_add_cost * 1.5);
                lucky_cost_UI.Text = $"{lucky_add_cost}";
                lucky_add_tooltip.Text = $@"Усилитель эффекта 'Удачливый' - добавляет +10 печенек к базовому бонусу эффекта
Текущий бонус: {lucky_bonus}";
            }
            if (chance_enhancer_lvl == 10)
                DisablingButton(lucky_add_btn, lucky_cost_UI);

            if (!lucky)
                MessageBox.Show("Для улучшения способности нужно ее приобрести", "Купите способность", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Улучшение шанса на лотерею
        private void I7oDkyI7(object sender, RoutedEventArgs e)
        {
            if (!BuyClickChecker(bribe_cost))
                return;

            if (bribe_lvl < 10 && cookie_counter >= bribe_cost)
            {
                EatingCookies(bribe_cost);
                bribe_cost *= 2;
                bribe_lvl++;
                bribe_cost_UI.Text = $"{bribe_cost}";
            }
            if (bribe_lvl == 10)
            {
                DisablingButton(multipler_btn, multiplier_cost_UI);
            }

        }

        // ------------------------------------------------------------------------
        // Вспомогательные методы

        private int GetMultiplierCookie(int counter)
        {
            if (multiplier_lvl == 0)
                return counter;
            if (multiplier_lvl == 1)
                return counter * rand.Next(0, 4);
            if (multiplier_lvl == 2)
                return counter * rand.Next(0, 5);
            if (multiplier_lvl == 3)
                return counter * rand.Next(0, 6);
            return counter;
        }

        private void EatingCookies(int cost)
        {
            cookie_counter -= cost;
            counter_txt.Text = $"{cookie_counter}";
        }

        private bool BuyClickChecker(int price)
        {
            if (cookie_counter < price)
            {
                MessageBox.Show("У вас недостаточно печенек для покупки. Приходите как только накопите больше печенек!",
                    "CookieClicker", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            else return true;
        }

        private void PriceCheckerUI(int price, TextBlock price_txt)
        {
            if (cookie_counter < price)
                price_txt.Foreground = Brushes.Red;
            else price_txt.Foreground = Brushes.LightGreen;
        }

        private void PriceCheckerUI(int price, TextBlock price_txt, int lvl) {
            if (cookie_counter < price && lvl < 10)
                price_txt.Foreground = Brushes.Red;
            else price_txt.Foreground = Brushes.LightGreen;
        }

        private void PriceCheckerUI(int price, TextBlock price_txt, bool enable)
        {
            if (cookie_counter < price && !enable)
                price_txt.Foreground = Brushes.Red;
            else price_txt.Foreground = Brushes.LightGreen;
        }

        private void DisablingButton(Button btn, TextBlock cost_ui)
        {
            btn.IsEnabled = false;
            btn.Opacity = 0.6;
            cost_ui.Text = "Куплено";
        }

    }
}
