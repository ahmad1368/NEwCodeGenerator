using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using GenerateProject.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            string DirectoryPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string TargetProject = "LavazemYadaki";
            DirectoryPath = Path.Combine(DirectoryPath, TargetProject);
            string tablesShouldGenerate = "*";//"Customer,Car,staff,Person"; //table1,table2,table3  or * for all
            bool overWrite = true;
            string databaseName = "LavazemYadaki";
            string providername = "System.Data.SqlClient";
            string connectionString = string.Format("Server = .; Database = {0}; UID = sa; PWD = ks123ks;", databaseName);
            var dbReader = new DatabaseReader(connectionString, providername);
            var schema = dbReader.ReadAll();
            List<EnumApplicationsPart> ApplicationParts = new List<EnumApplicationsPart>();
            //ApplicationParts.Add(EnumApplicationsPart.Access);
            //ApplicationParts.Add(EnumApplicationsPart.business);
            //ApplicationParts.Add(EnumApplicationsPart.controller);
            //ApplicationParts.Add(EnumApplicationsPart.Partial);
            //ApplicationParts.Add(EnumApplicationsPart.resource);
            //ApplicationParts.Add(EnumApplicationsPart.SearchModel);
            //ApplicationParts.Add(EnumApplicationsPart.Shared);
            //ApplicationParts.Add(EnumApplicationsPart.ViewCreate);
            //ApplicationParts.Add(EnumApplicationsPart.ViewCreate_Modal);
            //ApplicationParts.Add(EnumApplicationsPart.ViewDelete);
            //ApplicationParts.Add(EnumApplicationsPart.ViewDetails);
            //ApplicationParts.Add(EnumApplicationsPart.ViewEdit);
            //ApplicationParts.Add(EnumApplicationsPart.ViewElement);
            //ApplicationParts.Add(EnumApplicationsPart.ViewIndex);
            ApplicationParts.Add(EnumApplicationsPart.ViewList);
            //ApplicationParts.Add(EnumApplicationsPart.ViewSearch);
            //ApplicationParts.Add(EnumApplicationsPart.DeleteDirectory);
            //ApplicationParts.Add(EnumApplicationsPart.All);

            GenerateCode g = new GenerateCode(DirectoryPath, tablesShouldGenerate, schema, databaseName,TargetProject,ApplicationParts, overWrite);

            //MessageBox.Show("end successfuly");

            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
