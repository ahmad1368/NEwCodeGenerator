using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace LavazemYadaki
{

    public static class Utility
    {
      
        public class Paging
        {
            public Paging()
            {
                page = 1;
                pageSize = 5;

            }
            public int page { get; set; }
            public int pageSize { get; set; }
            public string sortOrder { get; set; }
        }

        public class DropDownListAjax
        {
            public int Value { get; set; }
            public string Text { get; set; }
            
        }

        public static string UniqName(string docPath, string fileName)
        {
            bool exist = false;

            if (!File.Exists(Path.Combine(docPath, fileName)))
            {
                return fileName;
            }

            do
            {
                fileName += DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString();

                exist = System.IO.File.Exists(Path.Combine(docPath, fileName));

            } while (exist);




            return fileName;
        }


        public static void DeleteCreateDirectory(string docPath, bool overWrite)
        {
            if (System.IO.Directory.Exists(docPath) && overWrite)
            {
                System.IO.Directory.Delete(docPath, true);
            }

            System.Threading.Thread.Sleep(100);

            if (!System.IO.Directory.Exists(docPath))
            {
                System.IO.Directory.CreateDirectory(docPath);
            }


        }

        public static string PersianDate(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return "";

            var persianCalendar = new System.Globalization.PersianCalendar();
            return string.Format("{0:0000}/{1:00}/{2:00}", persianCalendar.GetYear(dateTime.Value), persianCalendar.GetMonth(dateTime.Value), persianCalendar.GetDayOfMonth(dateTime.Value));
        }

        public static DateTime ConvertToGregorianDate(string persianDate)
        {
            string year, month, day = string.Empty;
            string tempDate = persianDate;

            year = tempDate.Substring(0, 4);
            month = tempDate.Substring(5, 2);
            day = tempDate.Substring(8, 2);
            PersianCalendar pc = new PersianCalendar();
            return pc.ToDateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day), 0, 0, 0, 0);
        }

        public static DateTime? ConvertPersionDateStringToPersionDateDateToime(string persianDate)
        {
            if (string.IsNullOrEmpty(persianDate))
                return null;

            int year, month, day = 0;
            string tempDate = persianDate;
            year = int.Parse(tempDate.Substring(0, 4));
            month = int.Parse(tempDate.Substring(5, 2));
            day = int.Parse(tempDate.Substring(8, 2));

            return new DateTime(year, month, day);
        }
    }

}