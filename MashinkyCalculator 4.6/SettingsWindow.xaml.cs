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

namespace MashinkyCalculator
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private UserSettings userSettings;
        public SettingsWindow(DataManager dataManager)
        {
            InitializeComponent();
            userSettings = new UserSettings(dataManager);
            DataContext = userSettings;
        }

        private void SortByCapacityCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SortByCostCheckBox.IsChecked = false;
            SortByFuelCheckBox.IsChecked = false;
            SortByCombinedCheckBox.IsChecked = false;
            userSettings.UpdateResultPriority();
        }

        private void SortByCostCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SortByCapacityCheckBox.IsChecked = false;
            SortByFuelCheckBox.IsChecked = false;
            SortByCombinedCheckBox.IsChecked = false;
            userSettings.UpdateResultPriority();
        }

        private void SortByFuelCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SortByCostCheckBox.IsChecked = false;
            SortByCapacityCheckBox.IsChecked = false;
            SortByCombinedCheckBox.IsChecked = false;
            userSettings.UpdateResultPriority();
        }

        private void SortByCombinedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SortByCostCheckBox.IsChecked = false;
            SortByFuelCheckBox.IsChecked = false;
            SortByCapacityCheckBox.IsChecked = false;
            userSettings.UpdateResultPriority();
        }

        private void SaveSettignsButton_Click(object sender, RoutedEventArgs e)
        {
            userSettings.SaveSettings();
            Close();
        }

        private void CancelSettignsButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
