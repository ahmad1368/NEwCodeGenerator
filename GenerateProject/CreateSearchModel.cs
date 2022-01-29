using DatabaseSchemaReader.DataSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateProject
{
    public class CreateSearchModel
    {
        public CreateSearchModel(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        {
            

            string fileName = table.Name + "SearchModel";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }


            fileName = string.Format("{0}.cs", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }



            List<string> lines = new List<string>();

            lines.Add("using System;");
            lines.Add("using System.Collections.Generic;");
            lines.Add("using System.Linq;");
            lines.Add("using System.Web;");
            lines.Add("using System.ComponentModel.DataAnnotations;");

            lines.Add("namespace " + TargetProject + ".Models {");
            lines.Add("public class " + table.Name + "SearchModel : Utility.Paging" + " {");


            foreach (var column in table.Columns)
            {
                //listBox1.Items.Add("Column " + column.Name + "\t" + column.DataType.TypeName + "\t" + column.Description);
                //if (column.DataType.IsString) listBox1.Items.Add("column.Length: (" + column.Length + ")");
                //if (column.IsPrimaryKey) listBox1.Items.Add("Primary key");
                //if (column.IsForeignKey) listBox1.Items.Add("Foreign key to " + column.ForeignKeyTable.Name);

                

                switch (column.NetDataType().Name)
                {
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "Double":
                    case "Decimal":
                    case "Single":
                        lines.Add(string.Format("[Display(Name = \"{0}\", Description = \"{0}_Description\", ResourceType = typeof(Resources.{1}))]", column.Name + "From", table.Name));
                        lines.Add("public " + column.DataType.NetDataTypeCSharpName + " " + column.Name + "From" + " " + "{ get; set; }");

                        lines.Add(string.Format("[Display(Name = \"{0}\", Description = \"{0}_Description\", ResourceType = typeof(Resources.{1}))]", column.Name + "To", table.Name));
                        lines.Add("public " + column.DataType.NetDataTypeCSharpName + " " + column.Name + "To" + " " + "{ get; set; }");
                        break;
                    case "Boolean":
                        lines.Add(string.Format("[Display(Name = \"{0}\", Description = \"{0}_Description\", ResourceType = typeof(Resources.{1}))]", column.Name, table.Name));
                        lines.Add("public " + column.DataType.NetDataTypeCSharpName + "? " + column.Name + " " + "{ get; set; }");
                        break;

                    case "DateTime":
                    case "TimeSpan":
                        lines.Add(string.Format("[Display(Name = \"{0}\", Description = \"{0}_Description\", ResourceType = typeof(Resources.{1}))]", column.Name + "From", table.Name));
                        lines.Add("public " + column.DataType.NetDataTypeCSharpName + "? " + column.Name + "From" + " " + "{ get; set; }");

                        lines.Add(string.Format("[Display(Name = \"{0}\", Description = \"{0}_Description\", ResourceType = typeof(Resources.{1}))]", column.Name + "To", table.Name));
                        lines.Add("public " + column.DataType.NetDataTypeCSharpName + "? " + column.Name + "To" + " " + "{ get; set; }");

                        break;

                    case "String":
                    default:

                        lines.Add(string.Format("[Display(Name = \"{0}\", Description = \"{0}_Description\", ResourceType = typeof(Resources.{1}))]", column.Name , table.Name));
                        lines.Add("public " + column.DataType.NetDataTypeCSharpName + " " + column.Name + " " + "{ get; set; }");

                        break;
                }
            }


            lines.Add("}");
            lines.Add("}");

            if (!System.IO.Directory.Exists(docPath))
            {
                System.IO.Directory.CreateDirectory(docPath);
            }
            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }
    }
}
