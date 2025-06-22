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
using System.IO;

namespace MashinkyCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //colors text #f1edca
        //color background #38372e
        // color controlbackground #25261e
        // color item dark #6a664d
        // color item light #958e72
        private UserDataContext userData;
        private DataManager dataManager;
        
        public MainWindow()
        {
            InitializeComponent();
            dataManager = new DataManager();
            userData = new UserDataContext(dataManager);
            DataContext = userData;
            
            
        }

        private void Priority2TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int input;
            if (int.TryParse(Priority2TextBox.Text, out input))
            {
                if (input > 0)
                {
                    userData.RatioCargo2 = input;
                }
                else
                {
                   /* MessageBox.Show("Please enter whole number greater then zero");
                    Priority2TextBox.Text = "1";*/
                }
            }
            else
            {
                //MessageBox.Show("Incorrect input");
                Priority2TextBox.Text = "";
            }

            if (TriggerCalculatorConditions())
                userData.CalculateTrain();
        }




        private void Priority1TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int input;
            if (int.TryParse(Priority1TextBox.Text, out input))
            {
                if (input > 0)
                {
                    userData.RatioCargo1 = input;
                }
                else
                {
                    MessageBox.Show("Please enter whole number greater then zero");
                    Priority1TextBox.Text = "1";
                }
            }
            else
            {
               // MessageBox.Show("Incorrect input");
                Priority1TextBox.Text = "";
            }

            if (TriggerCalculatorConditions())
                userData.CalculateTrain();
        }

        private void FuelTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TriggerCalculatorConditions())
                userData.CalculateTrain();
        }

        private void TrainLengthTextBox_LostFocus(object sender, TextChangedEventArgs e)
        {
            if (TriggerCalculatorConditions())
                userData.CalculateTrain();
        }

        private void FuelTypeComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (TriggerCalculatorConditions())
                userData.CalculateTrain();
        }

        private void CargoType1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userData.UpdateCargo1();
            MakeCargo1Visible();
            if (TriggerCalculatorConditions())
                userData.CalculateTrain();

        }

        private void CargoType2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userData.UpdateCargo2();
            MakeCargo2Visible();
            if (TriggerCalculatorConditions())
                userData.CalculateTrain();
        }

        private void EpochTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            int input;
            if (int.TryParse(EpochTextBox.Text, out input) & input >= 0 & input < 8)
            {
                userData.UpdateAllFilters();
                if (TriggerCalculatorConditions())
                    userData.CalculateTrain();
            }
            else if (EpochTextBox.Text == "")
            {
                userData.UpdateAllFilters();
                if (TriggerCalculatorConditions())
                    userData.CalculateTrain();
            }
            else
            {
               // MessageBox.Show(EpochTextBox.Text + " is not valid input, please enter whole number in range 0-7");
                EpochTextBox.Text = "";
            }

        }

        public bool TriggerCalculatorConditions()
        {

            return userData.MaxTrainLength > 0 && userData.Cargo1TypeFilter != null && userData.Cargo1TypeFilter is Material;
        }

        private void MakeCargo1Visible()
        {
            TokenType1TextBlock.Visibility = Visibility.Visible;
            TokenType1ComboBox.Visibility = Visibility.Visible;
            Priority1TextBlock.Visibility = Visibility.Visible;
            Priority1TextBox.Visibility = Visibility.Visible;
        }

        private void MakeCargo2Visible()
        {
            TokenType2TextBlock.Visibility = Visibility.Visible;
            TokenType2ComboBox.Visibility = Visibility.Visible;
            Priority2TextBlock.Visibility = Visibility.Visible;
            Priority2TextBox.Visibility = Visibility.Visible;
        }

        private void SpeedLimitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int input;
            if (int.TryParse(SpeedLimitTextBox.Text, out input))
            {
                if (input > 999)
                {
                    MessageBox.Show("Speed is too high!\n (in case Mr. Hračička released some ultraShinkanzen train, please contact application creator to update his app... or just knock some sense into Mr. Hračička");
                    SpeedLimitTextBox.Text = "";
                }
                else
                    if (TriggerCalculatorConditions())
                    userData.CalculateTrain();
            }
            else if (String.IsNullOrEmpty(SpeedLimitTextBox.Text))
            {
                if (TriggerCalculatorConditions())
                    userData.CalculateTrain();
            }
            else
                MessageBox.Show("Incorrect input");
        }
        private void TokenType1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TriggerCalculatorConditions())
                userData.CalculateTrain();
        }

        private void TrainLengthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TriggerCalculatorConditions())
                userData.CalculateTrain();
        }

        private void TokenType2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TriggerCalculatorConditions())
                userData.CalculateTrain();
        }

        private void TopMenuSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(dataManager);
            settingsWindow.ShowDialog();
            userData.CalculateTrain();
        }

        private void TopMenuHelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }

        private void TopMenuAboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        


    }
}

