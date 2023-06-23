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
using TaskLog.Entities;

namespace TaskLog
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public long UserId { get; set; }
        private long CurrentCompId;
        public AddTaskWindow()
        {
            InitializeComponent();
        }

        public AddTaskWindow(Components component)
        {
            InitializeComponent();
            IdComponentTB.Text = component.CompOemId;
            VerComponentTB.Text = component.CompOemVer;
            NameComponentTB.Text = component.CompOemName;
            SwVerComponentTB.Text = component.SwVer;
            CurrentCompId = component.CompId;
            CreateButton.IsEnabled = true;
        }

        private void SelectComponentButton_Click(object sender, RoutedEventArgs e)
        {
            ComponentWindow componentWindow = new ComponentWindow();
            componentWindow.Show();
            this.Close();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(TaskDescrTextBox.Document.ContentStart
                        , TaskDescrTextBox.Document.ContentEnd);
            string TaskDescr = textRange.Text.Replace("\r\n", string.Empty);
            if (SerialNumberTextBox.Text.Length > 0 && TaskDescr.Length > 0)
            {
                try
                {
                    Tasks task = new Tasks();
                    task.CompId = CurrentCompId;
                    task.TaskDescr = TaskDescr;
                    task.CompSn = SerialNumberTextBox.Text;
                    task.UserId = 1;
                    DbUtils.db.Tasks.Add(task);
                    //DbUtils.db.SaveChanges();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            TasksWindow tasksWindow = new TasksWindow();
            tasksWindow.Show();
            this.Close();
        }
    }
}
