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
    /// Interaction logic for TaskViewWindow.xaml
    /// </summary>
    public partial class TaskViewWindow : Window
    {
        private long IdCurrentTask, UserId;
        private string CurrentEventType;
        public TaskViewWindow(Tasks task)
        {
            InitializeComponent();
            App app = (App)Application.Current;
            UserId = app.UserId;
            IdCurrentTask = task.TaskId;
            ValueOfEventTypeToComboBox(task);
            FillTextBlocks(task);
        }

        private void FillTextBlocks(Tasks task) 
        {
            TimeStampTB.Text = $"{DbUtils.db.EventLog.FirstOrDefault(x => x.TaskId == task.TaskId).EventTimestamp:F}";
            TaskCreatorTB.Text = task.User.UserName;
            TaskDescrTextBox.AppendText(task.TaskDescr);
            IdComponentTB.Text = task.Comp.CompOemId.ToString();
            VerComponentTB.Text = task.Comp.CompOemVer.ToString();
            NameComponentTB.Text = task.Comp.CompOemName.ToString();
            SwVerComponentTB.Text = task.Comp.SwVer.ToString();
            SerialNumberTB.Text = task.CompSn;
        }
        private void ValueOfEventTypeToComboBox(Tasks task)
        {
            foreach (TextBlock item in TaskStatusComboBox.Items)
            {
                if (item.Text == DbUtils.db.EventLog.Where(x => x.TaskId == task.TaskId)
                    .OrderByDescending(x => x.EventTimestamp)
                    .FirstOrDefault().EventType)
                {
                    TaskStatusComboBox.SelectedValue = item;
                    CurrentEventType = item.Text;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Tasks task = DbUtils.db.Tasks.FirstOrDefault(x => x.TaskId == IdCurrentTask);
            TextRange textRange = new TextRange(TaskDescrTextBox.Document.ContentStart
                        , TaskDescrTextBox.Document.ContentEnd);
            string TaskDescr = textRange.Text.Replace("\r\n", string.Empty);
            if(TaskDescr != task.TaskDescr)
            {
                if (TaskDescr.Length <= 512)
                {
                    task.TaskDescr = TaskDescr;
                    DbUtils.db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Текст описания не должен превышать 512 символов", "Предупреждение",MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (CurrentEventType != TaskStatusComboBox.Text)
            {
                try
                {
                    EventLog eventLog = new EventLog();
                    eventLog.EventType = TaskStatusComboBox.Text;
                    eventLog.EventTimestamp = DateTime.Now;
                    eventLog.TaskId = IdCurrentTask;
                    eventLog.UserId = UserId;
                    DbUtils.db.EventLog.Add(eventLog);
                    DbUtils.db.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("error");
                    return;
                }
            }
            this.Close();
        }
    }
}
