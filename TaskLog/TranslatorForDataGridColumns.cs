using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLog
{
    internal static class TranslatorForDataGridColumns
    {
        public static string Translate(string name) // Функция принимает в себя Header столбца DataGrid и через Switch возвращает в него нужное значение
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
                case "CompOemId":
                    {
                        return "OEM Id компонента";
                    }
                case "CompOemVer":
                    {
                        return "Версия компонента";
                    }
                case "SwVer":
                    {
                        return "Прошивка";
                    }
                case "CompId":
                    {
                        return "";
                    }
            }
            return $"{name}";
        }
    }
}
