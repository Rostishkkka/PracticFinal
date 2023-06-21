using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLog
{
    internal static class TranslatorForDataGridColumns
    {
        public static string Translate(string name)
        {
            switch (name) 
            {
                case "TaskId":
                    {
                        return "ID Задачи";
                    }
                case "CompOemName":
                    {
                        return "Название компонента";
                    }
                case "UserName":
                    {
                        return "Создатель";
                    }
                case "DateTime":
                    {
                        return "Дата создания";
                    }
                case "EventType":
                    {
                        return "Статус";
                    }
            }
            return $"None";
        }
    }
}
