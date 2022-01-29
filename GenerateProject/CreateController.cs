using DatabaseSchemaReader.DataSchema;
using GenerateProject.Enumeration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateProject
{
    public class CreateController
    {
        public CreateController()
        {

        }

        public CreateController(DatabaseTable table, string databaseName, string docPath, string TargetProject, bool overWrite)
        {

            string fileName = string.Format("{0}Controller", table.Name);

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
            lines.Add("using System.Data;");
            lines.Add("using System.Data.Entity;");
            lines.Add("using System.Linq;");
            lines.Add("using System.Net;");
            lines.Add("using System.Web;");
            lines.Add("using System.Web.Mvc;");
            lines.Add("using System.Data.Objects.SqlClient;");
            lines.Add(string.Format("using {0}.Models;", TargetProject));
            lines.Add("using PagedList;");
            lines.Add("using System.Security.Claims;");


            lines.Add(string.Format("namespace {0}.Controllers", TargetProject));
            lines.Add("{");
            //lines.Add("[Authorize]");
            lines.Add(string.Format("public class {0}Controller : Controller", table.Name));
            lines.Add("{");
            lines.Add(string.Format("private {0}Entities db = new {0}Entities();", databaseName));


            Find(table, lines);

            indexe_Post(table, lines);

            indexe_Get(table, lines);

            delete_get(table, lines);

            delete_post(table, lines);

            edite_post(table, lines);

            edite_Get(table, lines);

            Create_Get(table, lines);
            Create_Post(table, lines);


            Create_modal_Get(table, lines);
            Create_modal_Post(table, lines);


            Details_Get(table, lines);

            foreignKey_Post(table, lines, TargetProject);
            sort(table, lines);

            if (table.Name.Equals("UserLogin"))
                LoginLogOut(table, lines);
            /////////////////////////////////////////////////////////////////////////////////////////

            lines.Add("}");

            lines.Add("}");

            if (!System.IO.Directory.Exists(docPath))
            {
                System.IO.Directory.CreateDirectory(docPath);
            }
            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }

        private void Create_modal_Post(DatabaseTable table, List<string> lines)
        {
            lines.Add("[HttpPost]");
            lines.Add(string.Format("public int Create_modal({0} Entity)", table.Name));
            lines.Add("{");
            foreach (var column in table.Columns)
            {
                lines = ManageDatePost(column, lines, EnumActionType.CratePost);
            }
            lines.Add(string.Format("db.{0}.Add(Entity);", table.Name));
            lines.Add("db.SaveChanges();");
            foreach (var column in table.Columns)
            {
                if (column.IsPrimaryKey)
                    lines.Add(string.Format("return Entity.{0} ;", column.Name));
            }
            lines.Add("}");
        }

        private void LoginLogOut(DatabaseTable table, List<string> lines)
        {
            lines.Add("    [AllowAnonymous]");
            lines.Add("public ActionResult LogOut()");
            lines.Add("{");
            lines.Add("    var ctx = Request.GetOwinContext();");
            lines.Add("    var authManager = ctx.Authentication;");
            lines.Add("    authManager.SignOut(\"ApplicationCookie\");");
            lines.Add("    return RedirectToAction(\"index\", \"home\");");
            lines.Add("}");

            lines.Add("[HttpGet]");
            lines.Add("[AllowAnonymous]");
            lines.Add("public ActionResult LogIn(string returnUrl)");
            lines.Add("{");
            lines.Add("    var model = new LogInModel");
            lines.Add("    {");
            lines.Add("        ReturnUrl = returnUrl");
            lines.Add("    };");

            lines.Add("    return View(model);");
            lines.Add("}");
            lines.Add("[HttpPost]");
            lines.Add("[AllowAnonymous]");
            lines.Add("public ActionResult LogIn(LogInModel model)");
            lines.Add("{");
            lines.Add("    if (!ModelState.IsValid)");
            lines.Add("    {");
            lines.Add("        return View();");
            lines.Add("    }");
            lines.Add("    var user = Find(new UserLoginSearchModel { name = model.name, userpassword = model.Password }).FirstOrDefault();");

            lines.Add("    // Don't do this in production!");
            lines.Add("    //if (model.Email == \"admin@admin.com\" && model.Password == \"password\")");
            lines.Add("    if (user != null)");
            lines.Add("    {");
            lines.Add("        var identity = new ClaimsIdentity(new[] {");
            lines.Add("        new Claim(ClaimTypes.Name, user.name),");
            lines.Add("        new Claim(ClaimTypes.Email, \"a@b.c\"),");
            lines.Add("        new Claim(ClaimTypes.Country, \"Iran\"),");
            lines.Add("        new Claim(ClaimTypes.Role,user.role.cod)}, \"ApplicationCookie\");");
            lines.Add("        var ctx = Request.GetOwinContext();");
            lines.Add("        var authManager = ctx.Authentication;");
            lines.Add("        authManager.SignIn(identity);");
            lines.Add("        return Redirect(GetRedirectUrl(model.ReturnUrl));");
            lines.Add("    }");

            lines.Add("    // user authN failed");
            lines.Add("    ModelState.AddModelError(\"\", \"نام کاربری یا رمز عبور صحیح نمی باشد\");");
            lines.Add("    return View();");
            lines.Add("}");

            lines.Add("private string GetRedirectUrl(string returnUrl)");
            lines.Add("{");
            lines.Add("    if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))");
            lines.Add("    {");
            lines.Add("        return Url.Action(\"index\", \"home\");");
            lines.Add("    }");

            lines.Add("    return returnUrl;");
            lines.Add("}");

            lines.Add("private UserLogin SetUserLogin(userLoginModel model)");
            lines.Add("{");
            lines.Add("    UserLogin userLogin = new UserLogin();");
            lines.Add("    userLogin.emailid = model.emailid;");
            lines.Add("    userLogin.FlgActive = model.FlgActive;");
            lines.Add("    userLogin.name = model.name;");
            lines.Add("    userLogin.roleId = model.roleId;");
            lines.Add("    userLogin.Userid = model.Userid;");
            lines.Add("    userLogin.userpassword = model.userpassword;");
            lines.Add("    if (!string.IsNullOrEmpty(model.activeDateTo) && !model.activeDateTo.Equals(\"/\"))");
            lines.Add("        userLogin.activeDateTo = Utility.ConvertToGregorianDate(model.activeDateTo);");
            lines.Add("    if (!string.IsNullOrEmpty(model.activeDateFrom) && !model.activeDateFrom.Equals(\"/\"))");
            lines.Add("        userLogin.activeDateFrom = Utility.ConvertToGregorianDate(model.activeDateFrom);");
            lines.Add("    return userLogin;");
            lines.Add("}");
        }

        private void sort(DatabaseTable table, List<string> lines)
        {
            lines.Add(string.Format("private IQueryable<{0}> sort(IQueryable<{0}> {0}data, string[] arr)", table.Name));
            lines.Add("{");
            foreach (var Column in table.Columns)
            {

                lines.Add(string.Format("if (\"{0}\".Trim().ToLower().Equals(arr[0].Trim().ToLower()))", Column.Description));
                lines.Add("{");
                lines.Add(string.Format("if (arr[1].Equals(\"صعودی\"))"));
                lines.Add("{");
                lines.Add(string.Format("{0}data = {0}data.OrderBy(x => x.{1});", table.Name, Column.Name));
                lines.Add("}");
                lines.Add(string.Format("if (arr[1].Equals(\"نزولی\"))"));
                lines.Add("{");
                lines.Add(string.Format("{0}data = {0}data.OrderByDescending(x => x.{1});", table.Name, Column.Name));
                lines.Add("}");
                lines.Add("}");
            }


            lines.Add(string.Format("return {0}data;", table.Name));
            lines.Add("}");


        }

        private void foreignKey_Post(DatabaseTable table, List<string> lines, string TargetProject)
        {

            lines.Add("[HttpPost]");
            lines.Add(string.Format("public JsonResult {0}s(string Text)", table.Name));
            lines.Add("{");
            lines.Add(string.Format("    var {0}s = db.{0}.AsQueryable();", table.Name));
            lines.Add("    if (!string.IsNullOrEmpty(Text))");
            lines.Add("    {");
            foreach (var Column in table.Columns)
            {
                if (Column.NetDataType().Name == "String")
                {
                    lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1}.Contains(Text));", table.Name, Column.Name));
                    break;
                }
            }
            lines.Add("    }");
            lines.Add("");
            lines.Add(string.Format("var data = {0}s.Select(x => new Utility.DropDownListAjax()", table.Name));
            lines.Add("    {");

            string TextField = "";
            List<string> tempField = new List<string>();
            foreach (var Column in table.Columns)
            {
                if (tempField.Any(x => x == Column.Name))
                {
                    continue;
                }
                tempField.Add(Column.Name);

                if (Column.NetDataType().Name == "DateTime")
                {
                    continue;
                }

                if (Column.IsPrimaryKey)
                {
                    lines.Add(string.Format("Value = x.{0},", Column.Name));
                }

                TextField += " + " + DescRowTable(Column, "x");

                if (Column.IsForeignKey)
                {
                    foreach (var C in Column.ForeignKeyTable.Columns)
                    {
                        if (tempField.Any(x => x == C.Name))
                        {
                            continue;
                        }
                        tempField.Add(C.Name);
                        TextField += " + " + DescRowTable(C, $"x.{C.TableName}");
                    }
                }
            }

            lines.Add(string.Format("Text = {0}", TextField.Substring(3)));

            lines.Add("");
            lines.Add("}).Distinct().ToList();");

            foreach (var Column in table.Columns)
            {

                if (Column.NetDataType().Name == "DateTime")
                {
                    lines.Add(string.Format("foreach (var item in {0}s)", Column.TableName));
                    lines.Add("{");
                    lines.Add(string.Format("var itemData = data.Where(x => x.Value == item.{0}Id).FirstOrDefault();", Column.TableName));
                    lines.Add("if (itemData != null)");
                    lines.Add(string.Format("itemData.Text += \"{0}\" + \" : \"  + (item.{1}.HasValue ? Utility.PersianDate(item.{1}) : \"مشخص نشده است\");", Column.Description, Column.Name));
                    lines.Add("}");
                    lines.Add("");
                }
            }

            lines.Add("");
            lines.Add("    var j = Json(new");
            lines.Add("    {");
            lines.Add("        data");
            lines.Add("    }, JsonRequestBehavior.AllowGet);");
            lines.Add("    return j;");
            lines.Add("}");

            //lines.Add("[HttpPost]");
            //    lines.Add(string.Format("public JsonResult {0}s(string Text)", table.Name));
            //    lines.Add("{");
            //    lines.Add("List<Utility.DropDownListAjax> data = new List<Utility.DropDownListAjax>();");
            //    lines.Add(string.Format("var {0}s = db.{0};", table.Name));
            //    lines.Add(string.Format("foreach (var item in {0}s)", table.Name));
            //    lines.Add("{");

            //string TextField = DescRowTable(table);

            //string valueField = "";
            //foreach (var Column in table.Columns)
            //{
            //    if (Column.IsPrimaryKey)
            //        valueField = Column.Name;
            //}


            //lines.Add(string.Format("    data.Add(new Utility.DropDownListAjax {0} Value = item.{2}, Text = {3}  {1});", "{", "}", valueField, TextField));

            //    lines.Add("}");
            //    lines.Add("");
            //    lines.Add("");
            //    lines.Add("if (!string.IsNullOrEmpty(Text))");
            //    lines.Add("{");
            //    lines.Add("var temp = data;");
            //    lines.Add("    data.RemoveRange(0, data.Count - 1);");
            //    lines.Add("    data.AddRange(temp.Where(x => x.Text.Contains(Text)).ToList());");
            //    lines.Add("}");
            //    lines.Add("");
            //    lines.Add("var j = Json(new");
            //    lines.Add("{");
            //    lines.Add("    data");
            //    lines.Add("}, JsonRequestBehavior.AllowGet);");
            //    lines.Add("return j;");
            //    lines.Add("}");
        }

        public string DescRowTable(DatabaseColumn Column, string PreFiled)
        {
            string TextField = "";

            TextField += "\"" + Column.Description + "\" + \" : \" + ";
            switch (Column.NetDataType().Name)
            {
                case "Int16":
                case "Int32":
                case "Int64":
                case "Double":
                case "Decimal":
                case "Single":
                    if (Column.Nullable)

                        TextField += $" ({PreFiled}.{ Column.Name}.HasValue ? SqlFunctions.StringConvert((double) {PreFiled}.{Column.Name}).Trim() : \"مشخص نشده است\") + \"-\" + ";
                    else
                        TextField += $" SqlFunctions.StringConvert((double){PreFiled}.{ Column.Name}).Trim() + \"-\" + ";
                    break;

                //case "DateTime":            
                //    if (Column.Nullable)

                //        TextField += $" ({PreFiled}.{ Column.Name}.HasValue ? SqlFunctions.DateName(\"day\", {PreFiled}.{ Column.Name}) + \" // \" + SqlFunctions.DateName(\"month\", {PreFiled}.{ Column.Name}) + \" // \" +  SqlFunctions.DateName(\"year\", {PreFiled}.{ Column.Name}) : \"مشخص نشده است\") + \"-\" + ";
                //    else
                //        TextField += $" SqlFunctions.DateName(\"day\", {PreFiled}.{ Column.Name}) + \" // \" + SqlFunctions.DateName(\"month\", {PreFiled}.{ Column.Name}) + \" // \" +  SqlFunctions.DateName(\"year\", {PreFiled}.{ Column.Name}) + \"-\" + ";
                //    break;




                case "String":
                    TextField += $"{PreFiled}.{ Column.Name} + \"-\" + ";
                    break;
                case "DateTime":
                case "TimeSpan":
                    break;
                default:
                    break;
            }


            TextField = TextField.Remove(TextField.Length - 2);

            return TextField;
        }

        private void Details_Get(DatabaseTable table, List<string> lines)
        {
            lines.Add("public ActionResult Details(int? id)");
            lines.Add("{");
            lines.Add("if (id == null)");
            lines.Add("{");
            lines.Add("return new HttpStatusCodeResult(HttpStatusCode.BadRequest);");
            lines.Add("}");
            lines.Add(string.Format("{0}SearchModel {0}Search =  new {0}SearchModel();", table.Name));

            foreach (var column in table.Columns)
                if (column.IsPrimaryKey)
                {
                    lines.Add(string.Format("{0}Search.{1}From = id.Value;", table.Name, column.Name));
                    lines.Add(string.Format("{0}Search.{1}To = id.Value;", table.Name, column.Name));
                }

            lines.Add(string.Format("var {0}s = Find({0}Search).FirstOrDefault();", table.Name));
            lines.Add(string.Format("if ({0}s == null)", table.Name));
            lines.Add("{");
            lines.Add("return HttpNotFound();");
            lines.Add("}");

            foreach (var column in table.Columns)
            {
                if (column.NetDataType().Name == "DateTime" || column.NetDataType().Name == "DateTime?")
                {
                    lines = ManageDatePost(column, lines, EnumActionType.detailGet);
                }
                else
                {
                    lines.Add(string.Format("ViewBag.{0} = {1}s.{0};", column.Name, table.Name));
                }
            }

            lines.Add(string.Format("return View({0}s);", table.Name));
            lines.Add("}");
        }

        private void Find(DatabaseTable table, List<string> lines)
        {
            lines.Add(string.Format("public IQueryable<{0}> Find({0}SearchModel {0}Search)", table.Name));

            lines.Add("{");
            lines.Add(string.Format("var {0}s = db.{0}.AsQueryable<{0}>();", table.Name));

            foreach (var column in table.Columns)
            {
                switch (column.NetDataType().Name)
                {
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "Double":
                    case "Decimal":
                    case "Single":
                        lines.Add(string.Format("if ({0}Search.{1}From != 0)", table.Name, column.Name));
                        lines.Add("{");
                        lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} >= {0}Search.{1}From);", table.Name, column.Name));
                        lines.Add(string.Format("ViewBag.{1}From = {0}Search.{1}From;", table.Name, column.Name));
                        lines.Add("}");
                        ///////////////////////////////////////////////////////////////////////////////////////
                        lines.Add(string.Format("if ({0}Search.{1}To != 0)", table.Name, column.Name));
                        lines.Add("{");
                        lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} <= {0}Search.{1}To);", table.Name, column.Name));
                        lines.Add(string.Format("ViewBag.{1}To = {0}Search.{1}To;", table.Name, column.Name));
                        lines.Add("}");
                        ///////////////////////////////////////////////////////////////////////////////////////
                        break;
                    case "Boolean":
                        lines.Add(string.Format("if ({0}Search.{1}.HasValue)", table.Name, column.Name));
                        lines.Add("{");
                        lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} == {0}Search.{1}.Value);", table.Name, column.Name));
                        lines.Add(string.Format("ViewBag.{1} = {0}Search.{1};", table.Name, column.Name));
                        lines.Add("}");
                        break;

                    case "DateTime":
                        //case "TimeSpan":
                        lines = ManageDatePost(column, lines, EnumActionType.Find);

                        break;

                    case "String":

                        lines.Add(string.Format("if (!string.IsNullOrEmpty({0}Search.{1}))", table.Name, column.Name));
                        lines.Add("{");
                        lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1}.ToLower().Trim().Contains({0}Search.{1}.ToLower().Trim()));", table.Name, column.Name));
                        lines.Add(string.Format("ViewBag.{1} = {0}Search.{1};", table.Name, column.Name));
                        lines.Add("}");
                        break;

                    default:
                        lines.Add(string.Format("if ({0} {1} {2} {3})", table.Name, column.Name, "errrrrrrrrror", column.NetDataType().Name));
                        lines.Add("{");
                        lines.Add("}");
                        break;
                }
            }


            lines.Add(string.Format("return {0}s;", table.Name));

            lines.Add("}");

        }

        private void Create_Post(DatabaseTable table, List<string> lines)
        {
            lines.Add("[HttpPost]");
            lines.Add(string.Format("public ActionResult Create({0} Entity)", table.Name));
            lines.Add("{");
            foreach (var column in table.Columns)
            {
                lines = ManageDatePost(column, lines, EnumActionType.CratePost);
            }
            lines.Add(string.Format("db.{0}.Add(Entity);", table.Name));
            lines.Add("db.SaveChanges();");
            lines.Add("return RedirectToAction(\"Index\");");
            lines.Add("}");

        }

        private List<string> ManageDatePost(DatabaseColumn column, List<string> lines, EnumActionType actionType)
        {

            if (column.NetDataType().Name == "DateTime" || column.NetDataType().Name == "DateTime?")
            {
                String HasValueDateTime = "";
                if (column.Nullable) HasValueDateTime = ".Value";
                if (actionType == EnumActionType.CratePost || actionType == EnumActionType.EditPost)
                {
                    if (column.Nullable) lines.Add("if (Entity.Tarikh.HasValue)");
                    lines.Add("Entity." + column.Name + " = Utility.ConvertToGregorianDate(string.Format(\"{0:D4}/{1:D2}/{2:D2}\", Entity." + column.Name + HasValueDateTime + ".Year, Entity." + column.Name + HasValueDateTime + ".Month, Entity." + column.Name + HasValueDateTime + ".Day));");
                }
                if (actionType == EnumActionType.Find)
                {
                    lines.Add(string.Format("if ({0}Search.{1}From.HasValue)", column.Table.Name, column.Name));
                    lines.Add("{");
                    lines.Add(string.Format("var DateFrom = {0}Search.{1}From.Value;", column.Table.Name, column.Name));
                    lines.Add("DateFrom = Utility.ConvertToGregorianDate(string.Format(\"{0:D4}/{1:D2}/{2:D2}\", DateFrom.Year, DateFrom.Month, DateFrom.Day));");
                    lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} >= DateFrom);", column.Table.Name, column.Name));
                    lines.Add(string.Format("ViewBag.{1}From = Utility.PersianDate(DateFrom);", column.Table.Name, column.Name));
                    lines.Add("}");

                    lines.Add(string.Format("if ({0}Search.{1}To.HasValue)", column.Table.Name, column.Name));
                    lines.Add("{");
                    lines.Add(string.Format("var DateTo = {0}Search.{1}To.Value;", column.Table.Name, column.Name));
                    lines.Add("DateTo = Utility.ConvertToGregorianDate(string.Format(\"{0:D4}/{1:D2}/{2:D2}\", DateTo.Year, DateTo.Month, DateTo.Day));");
                    lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} <= DateTo);", column.Table.Name, column.Name));
                    lines.Add(string.Format("ViewBag.{1}To = Utility.PersianDate(DateTo);", column.Table.Name, column.Name));
                    lines.Add("}");
                }
                if (actionType == EnumActionType.Index)
                {
                    HasValueDateTime = "";
                    if (column.Nullable) HasValueDateTime = ".HasValue";
                    //lines.Add(string.Format("{0} = x.{0}{1} ? Utility.ConvertPersionDateStringToPersionDateDateToime(Utility.PersianDate(x.{0})) : null,", column.Name, HasValueDateTime));
                    lines.Add(string.Format("item.{0} = item.{0}{1} ? Utility.ConvertPersionDateStringToPersionDateDateToime(Utility.PersianDate(item.{0})) : null;", column.Name, HasValueDateTime));
                }

                if (actionType == EnumActionType.EditGet || actionType == EnumActionType.detailGet || actionType == EnumActionType.DeleteGet)
                {
                    HasValueDateTime = "";
                    if (column.Nullable) HasValueDateTime = ".HasValue";
                    //lines.Add(string.Format("{0} = x.{0}{1} ? Utility.ConvertPersionDateStringToPersionDateDateToime(Utility.PersianDate(x.{0})) : null,", column.Name, HasValueDateTime));
                    lines.Add(string.Format("ViewBag.{1} = {0}s.{1}.HasValue ? Utility.PersianDate({0}s.{1}) : \"\";", column.Table.Name, column.Name));
                }

            }
            return lines;
        }

        private void Create_Get(DatabaseTable table, List<string> lines)
        {
            lines.Add("public ActionResult Create()");
            lines.Add("{");
            lines.Add("return View();");
            lines.Add("}");
        }


        private void Create_modal_Get(DatabaseTable table, List<string> lines)
        {

            lines.Add("public ActionResult Create_modal(int? id)");
            lines.Add("{");
            lines.Add("    if (id == null)");
            lines.Add("    {");
            lines.Add("        return View();");
            lines.Add("    }");
            lines.Add(string.Format("{0}SearchModel {0}Search = new {0}SearchModel();",table.Name));
            lines.Add(string.Format("{0}Search.{0}IdFrom = id.Value;", table.Name));
            lines.Add(string.Format("{0}Search.{0}IdTo = id.Value;", table.Name));
            lines.Add(string.Format("var {0}s = Find({0}Search).FirstOrDefault();", table.Name));
            lines.Add(string.Format("if ({0}s == null)", table.Name));
            lines.Add("    {");
            lines.Add("        return HttpNotFound();");
            lines.Add("    }");            
            lines.Add(string.Format("return View({0}s);", table.Name));
            lines.Add("}");
        }

        private void edite_Get(DatabaseTable table, List<string> lines)
        {
            lines.Add("public ActionResult Edit(int? id)");
            lines.Add("{");
            lines.Add("if (id == null)");
            lines.Add("{");
            lines.Add("return new HttpStatusCodeResult(HttpStatusCode.BadRequest);");
            lines.Add("}");
            lines.Add(string.Format("{0}SearchModel {0}Search =  new {0}SearchModel();", table.Name));

            foreach (var column in table.Columns)
            {
                if (column.IsPrimaryKey)
                {
                    lines.Add(string.Format("{0}Search.{1}From = id.Value;", table.Name, column.Name));
                    lines.Add(string.Format("{0}Search.{1}To = id.Value;", table.Name, column.Name));
                }

                //lines.Add(string.Format("ViewBag.{0} = {0};", column.Name));
            }

            lines.Add(string.Format("var {0}s = Find({0}Search).FirstOrDefault();;", table.Name));
            lines.Add(string.Format("if ({0}s == null)", table.Name));
            lines.Add("{");
            lines.Add("return HttpNotFound();");
            lines.Add("}");


            foreach (var column in table.Columns)
            {
                if (column.NetDataType().Name == "DateTime" || column.NetDataType().Name == "DateTime?")
                {
                    lines = ManageDatePost(column, lines, EnumActionType.EditGet);
                }
                else
                {
                    lines.Add(string.Format("ViewBag.{0} = {1}s.{0};", column.Name, table.Name));
                }
            }




            lines.Add(string.Format("return View({0}s);", table.Name));
            lines.Add("}");
        }

        private void edite_post(DatabaseTable table, List<string> lines)
        {
            lines.Add("[HttpPost]");
            lines.Add(string.Format("public ActionResult Edit({0} Entity)", table.Name));
            lines.Add("{");
            foreach (var column in table.Columns)
            {
                lines = ManageDatePost(column, lines, EnumActionType.EditPost);
            }
            lines.Add("db.Entry(Entity).State = EntityState.Modified;");
            lines.Add("db.SaveChanges();");
            lines.Add("return RedirectToAction(\"Index\");");
            lines.Add("}");
        }

        private void delete_post(DatabaseTable table, List<string> lines)
        {
            lines.Add("[HttpPost, ActionName(\"Delete\")]");
            lines.Add("public ActionResult DeleteConfirmed(int id)");
            lines.Add("{");
            lines.Add(string.Format("{0} Entity = db.{0}.Find(id);", table.Name));
            lines.Add(string.Format("db.{0}.Remove(Entity);", table.Name));
            lines.Add("db.SaveChanges();");
            lines.Add("return RedirectToAction(\"Index\");");
            lines.Add("}");
        }

        private void delete_get(DatabaseTable table, List<string> lines)
        {
            lines.Add("public ActionResult Delete(int? id)");
            lines.Add("{");
            lines.Add("if (id == null)");
            lines.Add("{");
            lines.Add("return new HttpStatusCodeResult(HttpStatusCode.BadRequest);");
            lines.Add("}");
            lines.Add(string.Format("{0}SearchModel {0}Search =  new {0}SearchModel();", table.Name));

            foreach (var column in table.Columns)
                if (column.IsPrimaryKey)
                {
                    lines.Add(string.Format("{0}Search.{1}From = id.Value;", table.Name, column.Name));
                    lines.Add(string.Format("{0}Search.{1}To = id.Value;", table.Name, column.Name));
                }


            lines.Add(string.Format("var {0}s = Find({0}Search).FirstOrDefault();", table.Name));
            lines.Add(string.Format("if ({0}s == null)", table.Name));
            lines.Add("{");
            lines.Add("return HttpNotFound();");
            lines.Add("}");
            foreach (var column in table.Columns)
            {
                if (column.NetDataType().Name == "DateTime" || column.NetDataType().Name == "DateTime?")
                {
                    lines = ManageDatePost(column, lines, EnumActionType.DeleteGet);
                }
                else
                {
                    lines.Add(string.Format("ViewBag.{0} = {1}s.{0};", column.Name, table.Name));
                }
            }

            lines.Add(string.Format("return View({0}s);", table.Name));
            lines.Add("}");
        }

        private void indexe_Get(DatabaseTable table, List<string> lines)
        {
            lines.Add(string.Format("public ActionResult Index({0}SearchModel {0}Search, bool? temp)", table.Name));
            lines.Add("{");
            //lines.Add(string.Format("{0}SearchModel {0}Search =  new {0}SearchModel();", table.Name));

            foreach (var Column in table.Columns)
            {
                if (Column.IsPrimaryKey)
                {
                    lines.Add(string.Format("if ({0}Search.sortOrder == null)", table.Name));
                    lines.Add(string.Format("{0}Search.sortOrder = \"{1}-نزولی\";", table.Name, Column.Description));
                }
            }

            lines.Add(string.Format("{0}Search.sortOrder = {0}Search.sortOrder.Replace(\"*\", \" \");", table.Name));

            lines.Add(string.Format("var {0}data = Find({0}Search);", table.Name));

            lines.Add(string.Format("string a = {0}Search.sortOrder;", table.Name));
            lines.Add("char[] c = { '-' };");
            lines.Add("string[] arr = a.Split(c);");

            lines.Add(string.Format("var {0}Sorted = sort({0}data, arr);", table.Name));

            //lines.Add(string.Format("{0}data = {0}Sorted.Select(x => new {0}()", table.Name));
            //lines.Add("{");
            //foreach (var column in table.Columns)
            //{
            //    if (column.NetDataType().Name == "DateTime" || column.NetDataType().Name == "DateTime?")
            //    {
            //        ManageDatePost(column, lines, EnumActionType.Index);
            //    }
            //    else
            //    {
            //        lines.Add(string.Format("{0} = x.{0},", column.Name));
            //    }
            //}
            //lines.Add("});");
            lines.Add(string.Format("var data = {0}Sorted.ToPagedList({0}Search.page, {0}Search.pageSize);", table.Name));
            lines.Add("foreach (var item in data)");
            lines.Add("{");
            foreach (var column in table.Columns)
            {
                if (column.NetDataType().Name == "DateTime" || column.NetDataType().Name == "DateTime?")
                {
                    lines = ManageDatePost(column, lines, EnumActionType.Index);
                }
            }
            lines.Add("}");

            lines.Add(string.Format("ViewBag.pageSize_text = {0}Search.pageSize;", table.Name));
            lines.Add(string.Format("ViewBag.pageSize = {0}Search.pageSize;", table.Name));

            lines.Add(string.Format("ViewBag.sortOrder_text = {0}Search.sortOrder;", table.Name));
            lines.Add(string.Format("ViewBag.sortOrder = {0}Search.sortOrder;", table.Name));

            lines.Add("return View(data);");

            lines.Add("}");
        }

        private void indexe_Post(DatabaseTable table, List<string> lines)
        {
            lines.Add("[HttpPost]");
            lines.Add(string.Format("public ActionResult Index({0}SearchModel {0}Search)", table.Name));
            lines.Add("{");
            //lines.Add(string.Format("return View(Find({0}Search));", table.Name));
            //lines.Add(string.Format("var data = Find({0}Search).ToPagedList({0}Search.page, {0}Search.pageSize);", table.Name));
            //foreach (var Column in table.Columns)
            //{
            //    if (Column.IsPrimaryKey)
            //        lines.Add(string.Format("var data = Find({0}Search).OrderBy(x=>x.{1}).ToPagedList({0}Search.page, {0}Search.pageSize);", table.Name, Column.Name));
            //}

            foreach (var Column in table.Columns)
            {
                if (Column.IsPrimaryKey)
                {
                    lines.Add(string.Format("if ({0}Search.sortOrder == null)", table.Name));
                    lines.Add(string.Format("{0}Search.sortOrder = \"{1}-نزولی\";", table.Name, Column.Description));
                }
            }

            lines.Add(string.Format("{0}Search.sortOrder = {0}Search.sortOrder.Replace(\"*\", \" \");", table.Name));

            lines.Add(string.Format("var {0}data = Find({0}Search);", table.Name));

            lines.Add(string.Format("string a = {0}Search.sortOrder;", table.Name));
            lines.Add("char[] c = { '-' };");
            lines.Add("string[] arr = a.Split(c);");

            lines.Add(string.Format("var {0}Sorted = sort({0}data, arr);", table.Name));
            //lines.Add(string.Format("{0}data = {0}Sorted.Select(x => new {0}()", table.Name));
            //lines.Add("{");
            //foreach (var column in table.Columns)
            //{
            //    if (column.NetDataType().Name == "DateTime" || column.NetDataType().Name == "DateTime?")
            //    {
            //        ManageDatePost(column, lines, EnumActionType.Index);
            //    }
            //    else
            //    {
            //        lines.Add(string.Format("{0} = x.{0},", column.Name));
            //    }
            //}
            //lines.Add("});");
            lines.Add(string.Format("var data = {0}Sorted.ToPagedList({0}Search.page, {0}Search.pageSize);", table.Name));
            lines.Add("foreach (var item in data)");
            lines.Add("{");
            foreach (var column in table.Columns)
            {
                if (column.NetDataType().Name == "DateTime" || column.NetDataType().Name == "DateTime?")
                {
                    lines = ManageDatePost(column, lines, EnumActionType.Index);
                }
            }
            lines.Add("}");

            lines.Add(string.Format("ViewBag.pageSize_text = {0}Search.pageSize;", table.Name));
            lines.Add(string.Format("ViewBag.pageSize = {0}Search.pageSize;", table.Name));
            lines.Add(string.Format("ViewBag.sortOrder_text = {0}Search.sortOrder;", table.Name));
            lines.Add(string.Format("ViewBag.sortOrder = {0}Search.sortOrder;", table.Name));

            lines.Add("return View(data);");
            lines.Add("}");
        }
    }
}
