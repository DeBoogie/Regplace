using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Regplace
{
    public partial class MainWindow : Window
    {
        public bool subWindow = false;
        public string search = "";
        public string replace = "";
        public RegistryView registryView = RegistryView.Default;
        public SearchReplace searchReplace;

        public MainWindow()
        {
            InitializeComponent();
        }

        private RegistryHive HiveResolve(string h)
        {
            switch (h)
            {
                case "HKEY_LOCAL_MACHINE":
                    return RegistryHive.LocalMachine;

                case "HKEY_CURRENT_USER":
                    return RegistryHive.CurrentUser;

                case "HKEY_CLASSES_ROOT":
                    return RegistryHive.ClassesRoot;

                case "HKEY_USERS":
                    return RegistryHive.Users;
            }
            return 0;
        }

        private static string[] PathPart(string p)
        {
            try
            {
                p = p.Replace("Computer\\", String.Empty);
                string[] pr = p.Split('\\');
                return pr;
            } catch (Exception eX)
            {
                throw eX;
            }
        }

        private static string PathJoin(string[] pr, int sIndex)
        {
            string f = String.Empty;
            for(int i = sIndex; i < pr.Length; i++)
            {
                if(i == pr.Length-1)
                {
                    f += pr[i];
                } else
                {
                    f += pr[i] + "\\";
                }
            }
            return f;
        }

        public void ReplaceValues()
        {
            string[] pr = PathPart(textBox1.Text);
            string fp = PathJoin(pr, 1);
            RegistryKey root = RegistryKey.OpenBaseKey(HiveResolve(pr[0]), registryView);
            RegistryKey ch = root.OpenSubKey(fp, true);

            foreach (var v in ch.GetValueNames())
            {
                if (ch.GetValueKind(v) == RegistryValueKind.String)
                {
                    if (!String.IsNullOrEmpty(search))
                    {
                        ch.SetValue(v, ch.GetValue(v).ToString().Replace(search, replace));
                    }
                }
            }
            Visualize(false);
        }

        public void Visualize(bool preview)
        {
            try
            {
                treeView1.Items.Clear();

                string[] pr = PathPart(textBox1.Text);
                string fp = PathJoin(pr, 1);
                RegistryKey root = RegistryKey.OpenBaseKey(HiveResolve(pr[0]), registryView);
                RegistryKey ch = root.OpenSubKey(fp, true);

                if (ch != null)
                {
                    foreach (var v in ch.GetValueNames())
                    {
                        if (ch.GetValueKind(v) == RegistryValueKind.String)
                        {
                            TreeViewItem treeViewItem1 = new TreeViewItem();
                            TreeViewItem treeViewItem2 = new TreeViewItem();
                            if (preview)
                            {
                                treeViewItem1.Header = "(Preview) " + v;
                                treeViewItem2.Header = "(Preview) " + ch.GetValue(v).ToString().Replace(search, replace);
                            }
                            else
                            {
                                treeViewItem1.Header = v;
                                treeViewItem2.Header = ch.GetValue(v);
                            }
                            treeViewItem1.Items.Add(treeViewItem2);

                            treeView1.Items.Add(treeViewItem1);
                        }
                    }
                }
            }
            catch (Exception eX)
            {
                // Nothin'
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bit1.Items.Add("Default");
            bit1.Items.Add("x86");
            bit1.Items.Add("x64");
            bit1.SelectedIndex = 0;
            Visualize(false);
        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            Visualize(false);
        }

        private void MenuReplace1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!subWindow)
            {
                searchReplace = new SearchReplace(this);
                searchReplace.Show();
            }
        }

        private void Bit1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(bit1.SelectedItem.ToString())
            {
                case "x86":
                    registryView = RegistryView.Registry32;
                    break;

                case "x64":
                    registryView = RegistryView.Registry64;
                    break;

                case "Default":
                    registryView = RegistryView.Default;
                    break;
            }
            Visualize(false);
        }

        private void SearchReplaceMenuBtn1_Click(object sender, RoutedEventArgs e)
        {
            if (!subWindow)
            {
                searchReplace = new SearchReplace(this);
                searchReplace.Show();
            }
        }

        private void QuitMenuBtn1_Click(object sender, RoutedEventArgs e)
        {
            searchReplace.Close();
            this.Close();
        }

        private void AboutMenuBtn1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Source: https://github.com/DeBoogie\n\nRegplace is a super lightweight super-simple-functioning program that makes your life easier when replacing multiple strings in the registry view. A good example is changing some nonfunctioning environment variables to plain text.", "About", MessageBoxButton.OK);
        }
    }
}
