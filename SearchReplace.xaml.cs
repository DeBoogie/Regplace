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

namespace Regplace
{
    /// <summary>
    /// Interaction logic for SearchReplace.xaml
    /// </summary>
    public partial class SearchReplace : Window
    {
        private MainWindow mainHandle;

        public SearchReplace(MainWindow mainHandle)
        {
            InitializeComponent();
            this.mainHandle = mainHandle;
        }

        private void drag1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void topBar1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void searchReplaceWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.Topmost = true;
            mainHandle.subWindow = true;
        }

        private void searchReplaceWindow1_Unloaded(object sender, RoutedEventArgs e)
        {
            mainHandle.subWindow = false;
            mainHandle.search = "";
            mainHandle.replace = "";
        }

        private void searchText1_KeyUp(object sender, KeyEventArgs e)
        {
            mainHandle.search = searchText1.Text;
        }

        private void replaceText1_KeyUp(object sender, KeyEventArgs e)
        {
            mainHandle.replace = replaceText1.Text;
        }

        private void previewButton1_Click(object sender, RoutedEventArgs e)
        {
            mainHandle.Visualize(true);
        }

        private void resetButton1_Click(object sender, RoutedEventArgs e)
        {
            mainHandle.Visualize(false);
        }

        private void replaceAllButton1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Remember make a Backup and Preview.\n\nContinue Replacing?", "Reminder", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                mainHandle.ReplaceValues();
                MessageBox.Show("Ready.", "Regplace", MessageBoxButton.OK);
            } else
            {
                MessageBox.Show("No changes were made.", "Abort", MessageBoxButton.OK);
            }
        }
    }
}
