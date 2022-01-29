using DatabaseSchemaReader.DataSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Resources;

namespace GenerateProject
{
    public class CreateResource
    {
        struct twiceValue
        {
            public string name { get; set; }
            public string value { get; set; }
        }
        public CreateResource(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        {

            CreateResourceResx(table, docPath, TargetProject, overWrite);

            CreateResourceDesigner(table, docPath, TargetProject, overWrite);

        }

        private void CreateResourceDesigner(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        {
            string fileName = table.Name;

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }


            fileName = string.Format("{0}.Designer.cs", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }

            List<string> lines = new List<string>();

            lines.Add("namespace " + TargetProject + ".Resources {");
            lines.Add("using System;");


            lines.Add("[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"System.Resources.Tools.StronglyTypedResourceBuilder\", \"4.0.0.0\")]");
            lines.Add("[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]");
            lines.Add("[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]");
            lines.Add("public class " + table.Name + " {");

            lines.Add("private static global::System.Resources.ResourceManager resourceMan;");

            lines.Add("private static global::System.Globalization.CultureInfo resourceCulture;");

            lines.Add("[global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(\"Microsoft.Performance\", \"CA1811:AvoidUncalledPrivateCode\")]");
            lines.Add("public " + table.Name + "() {");
            lines.Add("}");


            lines.Add("[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]");
            lines.Add("public static global::System.Resources.ResourceManager ResourceManager {");
            lines.Add("get {");
            lines.Add("if (object.ReferenceEquals(resourceMan, null)) {");
            lines.Add(string.Format("global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(\"{0}.Resources.{1}\", typeof({1}).Assembly);",
                TargetProject, table.Name, table.Name));
            lines.Add("resourceMan = temp;");
            lines.Add("}");
            lines.Add("return resourceMan;");
            lines.Add("}");
            lines.Add("}");


            lines.Add("[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]");
            lines.Add("public static global::System.Globalization.CultureInfo Culture {");
            lines.Add("get {");
            lines.Add("return resourceCulture;");
            lines.Add("}");
            lines.Add("set {");
            lines.Add("resourceCulture = value;");
            lines.Add("}");
            lines.Add("}");


            lines.Add("/// <summary>");
            lines.Add(string.Format("///   Looks up a localized string similar to {0}", table.Description));
            lines.Add("/// </summary>");
            lines.Add("public static string " + table.Name + "_Description {");
            lines.Add("get {");
            lines.Add(string.Format("return ResourceManager.GetString(\"{0}_Description\", resourceCulture);", table.Name));
            lines.Add("}");
            lines.Add("}");

            lines.Add("/// <summary>");
            lines.Add(string.Format("///   Looks up a localized string similar to {0}", table.Description));
            lines.Add("/// </summary>");
            lines.Add("public static string " + table.Name + "_Name {");
            lines.Add("get {");
            lines.Add(string.Format("return ResourceManager.GetString(\"{0}_Name\", resourceCulture);", table.Name));
            lines.Add("}");
            lines.Add("}");

            List<twiceValue> elseColumns = new List<twiceValue>();

            foreach (var Column in table.Columns)
            {

                lines.Add("/// <summary>");
                lines.Add(string.Format("///   Looks up a localized string similar to {0}", Column.Description));
                lines.Add("/// </summary>");

                lines.Add("public static string " + Column.Name + " {");
                lines.Add("get {");
                lines.Add(string.Format("return ResourceManager.GetString(\"{0}\", resourceCulture);", Column.Name));
                lines.Add("}");
                lines.Add("}");

                lines.Add("/// <summary>");
                lines.Add(string.Format("///   Looks up a localized string similar to {0}", Column.Description));
                lines.Add("/// </summary>");

                lines.Add("public static string " + Column.Name + "_Description {");
                lines.Add("get {");
                lines.Add(string.Format("return ResourceManager.GetString(\"{0}_Description\", resourceCulture);", Column.Name));
                lines.Add("}");
                lines.Add("}");


                lines.Add("/// <summary>");
                lines.Add(string.Format("///   Looks up a localized string similar to {0}", Column.Description));
                lines.Add("/// </summary>");

                lines.Add("public static string " + Column.Name + "_Name {");
                lines.Add("get {");
                lines.Add(string.Format("return ResourceManager.GetString(\"{0}_Name\", resourceCulture);", Column.Name));
                lines.Add("}");
                lines.Add("}");

                switch (Column.NetDataType().Name)
                {
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "Double":
                    case "Decimal":
                    case "Single":
                    case "DateTime":
                    case "TimeSpan":

                        twiceValue tv = new twiceValue();
                        tv.name = string.Format("{0}{1}", Column.Name, "From");
                        tv.value = Column.Description;
                        elseColumns.Add(tv);

                        tv = new twiceValue();
                        tv.name = string.Format("{0}{1}", Column.Name, "To");
                        tv.value = Column.Description;
                        elseColumns.Add(tv);



                        break;
                }

            }

            
            elseColumns.Add(new twiceValue() { name = "To", value = "تا" });

            foreach (var column in elseColumns)
            {

                lines.Add("/// <summary>");
                lines.Add(string.Format("///   Looks up a localized string similar to {0}", column.value));
                lines.Add("/// </summary>");

                lines.Add("public static string " + column.name + " {");
                lines.Add("get {");
                lines.Add(string.Format("return ResourceManager.GetString(\"{0}\", resourceCulture);", column.name));
                lines.Add("}");
                lines.Add("}");

            }

            lines.Add("}");
            lines.Add("}");


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }


        public void CreateResourceResx(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        {

            string fileName = table.Name;

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.resx", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }

            List<twiceValue> resources = new List<twiceValue>();

            twiceValue tv = new twiceValue();

            tv.name = string.Format("{0}_Description", table.Name);
            tv.value = string.Format("{0}", table.Description);
            resources.Add(tv);
            tv.name = string.Format("{0}_Name", table.Name);
            tv.value = string.Format("{0}", table.Description);
            resources.Add(tv);

            List<twiceValue> elseColumns = new List<twiceValue>();

            foreach (var column in table.Columns)
            {

                tv.name = string.Format("{0}", column.Name);
                tv.value = column.Description;
                resources.Add(tv);

                tv.name = string.Format("{0}_Description", column.Name);
                tv.value = column.Description;
                resources.Add(tv);

                switch (column.NetDataType().Name)
                {
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "Double":
                    case "Decimal":
                    case "Single":
                    case "DateTime":
                    case "TimeSpan":

                        tv = new twiceValue();
                        tv.name = string.Format("{0}{1}",column.Name,"From");
                        tv.value = column.Description;
                        elseColumns.Add(tv);

                        tv = new twiceValue();
                        tv.name = string.Format("{0}{1}",column.Name,"To");
                        tv.value = column.Description;
                        elseColumns.Add(tv);

                        

                        break;
                }

            }


            
            elseColumns.Add(new twiceValue() { name = "To", value = "تا" });

            foreach (var column in elseColumns)
            {

                tv.name = string.Format("{0}", column.name);
                tv.value = column.value;
                resources.Add(tv);               

            }


            addResourceRow(resources, Path.Combine(docPath, fileName));
        }

        void addResourceRow(List<twiceValue> resources, string path)
        {
            using (ResXResourceWriter resx = new ResXResourceWriter(path))
            {
                foreach (var res in resources)
                {
                    resx.AddResource(res.name, res.value);
                }

            }
        }
    }
}
