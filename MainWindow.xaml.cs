using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
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

namespace Anket_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Person> people = new();

        public ObservableCollection<Person> People { get => people; set { people = value; OnPropertyChanged(); } }






        public MainWindow()
        {

            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (add_btn.Content.ToString() == "Add")
            {
                if (tb_Name.Text.Length > 0 && tb_Surname.Text.Length > 0 || tb_Email.Text.Length > 0 || tb_Tel.Text.Length > 0 || dt.Text.Length > 0)
                {
                    Person person = new Person();
                    person.Name = tb_Name.Text;
                    person.Surname = tb_Surname.Text;
                    person.Email = tb_Email.Text;
                    person.Phone = tb_Tel.Text;
                    person.bDay = dt.Text;
                    People.Add(person);
                    tb_Name.Text = null;
                    tb_Surname.Text = null;
                    tb_Email.Text = null;
                    tb_Tel.Text = null;
                    dt.SelectedDate = DateTime.Now;
                }
                else
                {
                    MessageBox.Show("Melumat bosh buraxila bilmez", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (add_btn.Content == "Change")
            {

                if (lb.SelectedItem is Person person)
                {

                    person.Name = tb_Name.Text;
                    person.Surname = tb_Surname.Text;
                    person.Email = tb_Email.Text;
                    person.Phone = tb_Tel.Text;
                    person.bDay = dt.Text.ToString();
                    tb_Name.Text = null;
                    tb_Surname.Text = null;
                    tb_Email.Text = null;
                    tb_Tel.Text = null;
                    dt.SelectedDate = DateTime.Now;
                    add_btn.Content = "Add";
                    MessageBox.Show("Melumat deyisdiirildi", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

            if (tb_n.Text.Length > 0 && tb_n.Text != "File Name")
            {
                if (lb.Items.Count == 0)
                {
                    if (tb_Name.Text.Length == 0 && tb_Surname.Text.Length == 0 || tb_Email.Text.Length == 0 || tb_Tel.Text.Length == 0 || dt.Text.Length == 0)
                    {
                        MessageBox.Show("Melumatlar Boshdur","",MessageBoxButton.OK, MessageBoxImage.Information);
                        tb_n.Text = null;

                    }
                }

                else if (lb.ItemsSource is ObservableCollection<Person>)
                {
                    var json = JsonSerializer.Serialize(People);
                    File.WriteAllText(tb_n.Text, json);
                    MessageBox.Show("Save olundu", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    People.Clear();
                    tb_n.Text = null;
                    tb_Name.Text = null;
                    tb_Surname.Text = null;
                    tb_Email.Text = null;
                    tb_Tel.Text = null;
                    dt.SelectedDate = DateTime.Now;
                    add_btn.Content = "Add";
                }
            }
            else
            {
                MessageBox.Show("File adi bosh buraxila bilmez", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }




        private void load_Click(object sender, RoutedEventArgs e)
        {
            if (tb_n.Text.Length > 0)
            {
                var text = File.ReadAllText(tb_n.Text);
                var json = JsonSerializer.Deserialize<ObservableCollection<Person>>(text);
                People = json;
            }
            else
            {
                MessageBox.Show("File adi bosh buraxila bilmez", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }



        private void lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lb.SelectedItem is Person person)
            {

                add_btn.Content = "Change";
                tb_Name.Text = person.Name;
                tb_Surname.Text = person.Surname;
                tb_Email.Text = person.Email;
                tb_Tel.Text = person.Phone;
                dt.Text = person.bDay;
            }

        }

        private void tb_n_MouseEnter(object sender, MouseEventArgs e)
        {
            if (tb_n.Text == "File Name")
            {

                tb_n.Text = null;
                tb_n.Foreground = Brushes.Black;
            }

        }

        private void tb_n_MouseLeave(object sender, MouseEventArgs e)
        {
            if (tb_n.Text.Length == 0 || tb_n.Text == "File Name")
            {
                tb_n.Foreground = Brushes.Gray;
                tb_n.Text = "File Name";
            }
        }
    }
}
