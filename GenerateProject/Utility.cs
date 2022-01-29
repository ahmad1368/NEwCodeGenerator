using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateProject
{
    public static class Utility
    {
        public class DropDownAjax
        {
            public DropDownAjax()
            {
                Caption = "";

                idSearch = -1;
                ElementTextName = "";
                ElementTextNameValue = "";
                ElementTextId = "";
                ElementTextIdValue = "";

            }
            public string Caption { get; set; }
            public string Controller { get; set; }
            public string action { get; set; }
            public int idSearch { get; set; }

            public string ElementTextName { get; set; }
            public string ElementTextNameValue { get; set; }
            public string ElementTextId { get; set; }

            public string ElementTextIdValue { get; set; }

        }
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

    }
}
