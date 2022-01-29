using DatabaseSchemaReader.DataSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateProject
{
    public class CreatePartialView
    {
        public CreatePartialView(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        {


            string fileName = table.Name;

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
            //lines.Add("public class _" + table.Name + " {");


            lines.Add(string.Format("[MetadataType(typeof({0}Metadata))]", table.Name));
            lines.Add("public partial class " + table.Name + " {");
            lines.Add("public override string ToString()");
            lines.Add("{");
            lines.Add("//TO DO: Replace with a meaningful code");
            lines.Add("return base.ToString();");
            lines.Add("}");
            lines.Add("}");
            lines.Add(string.Format("public partial class {0}Metadata", table.Name));
            lines.Add("{");

            foreach (var column in table.Columns)
            {
                //listBox1.Items.Add("Column " + column.Name + "\t" + column.DataType.TypeName + "\t" + column.Description);
                //if (column.DataType.IsString) listBox1.Items.Add("column.Length: (" + column.Length + ")");
                //if (column.IsPrimaryKey) listBox1.Items.Add("Primary key");
                //if (column.IsForeignKey) listBox1.Items.Add("Foreign key to " + column.ForeignKeyTable.Name);

                string line = "";
                line = string.Format("[Display(Name = \"{0}\", Description = \"{0}_Description\", ResourceType = typeof(Resources.{1}))]", column.Name, table.Name);
                lines.Add(line);

                if (!column.IsForeignKey)
                    line = "public " + column.DataType.NetDataTypeCSharpName + " " + column.Name + " " + "{ get; set; }";
                else
                {
                    //line = "public List<" + column.DataType.NetDataTypeCSharpName + "> " + column.Name + " " + "{ get; set; }";
                    line = "public virtual ICollection<" + column.ForeignKeyTable.Name + "> " + column.Name + " " + "{ get; set; }";

                }

                lines.Add(line);
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
