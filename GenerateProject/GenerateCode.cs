using DatabaseSchemaReader;
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
    public class GenerateCode
    {
        private string DirectoryPath;
        private string tables;
        private DatabaseSchemaReader.DataSchema.DatabaseSchema schema;
        private bool overWrite;

        public GenerateCode()
        { }
        public GenerateCode(string docPath, string tablesShouldGenerate, DatabaseSchemaReader.DataSchema.DatabaseSchema schema, string databaseName, string TargetProject, List<EnumApplicationsPart> ApplicationParts, bool overWrite)
        {
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || (x == EnumApplicationsPart.resource && x == EnumApplicationsPart.DeleteDirectory)))
                Utility.DeleteCreateDirectory(Path.Combine(docPath, "Resources"), overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || (x == EnumApplicationsPart.controller && x == EnumApplicationsPart.DeleteDirectory)))
                Utility.DeleteCreateDirectory(Path.Combine(docPath, "Controllers"), overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || (x == EnumApplicationsPart.Shared && x == EnumApplicationsPart.DeleteDirectory)))
                Utility.DeleteCreateDirectory(Path.Combine(docPath, "Views", "Shared"), overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || (x == EnumApplicationsPart.Partial && x == EnumApplicationsPart.DeleteDirectory)))
                Utility.DeleteCreateDirectory(Path.Combine(docPath, "Models", "Partial"), overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || (x == EnumApplicationsPart.SearchModel && x == EnumApplicationsPart.DeleteDirectory)))
                Utility.DeleteCreateDirectory(Path.Combine(docPath, "Models", "SearchModel"), overWrite);

            string[] tablesAccept = tablesShouldGenerate.Split(',');

            foreach (var table in schema.Tables)
            {

                #region CheckTable

                if (!tablesShouldGenerate.ToLower().Trim().Equals("*"))
                {
                    bool found = false;
                    foreach (var tableAccept in tablesAccept)
                    {
                        if (tableAccept.ToLower().Trim().Equals(table.Name.ToLower().Trim()))
                        {
                            found = true;
                            break;
                        }

                    }

                    if (found == false)
                    {
                        continue;
                    }
                }


                if (table.Name.ToLower().Trim().Contains("sysdiagrams"))
                    continue;

                #endregion

                if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.resource))
                {
                    CreateResource createResource = new CreateResource(table, Path.Combine(docPath, "Resources"), TargetProject, overWrite);
                }

                if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.Partial))
                {
                    CreatePartialView createPartialView = new CreatePartialView(table, Path.Combine(docPath, "Models", "Partial"), TargetProject, overWrite);
                }


                if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.SearchModel))
                {
                    CreateSearchModel createSearchModel = new CreateSearchModel(table, Path.Combine(docPath, "Models", "SearchModel"), TargetProject, overWrite);
                }

                if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.controller))
                {
                    CreateController a = new CreateController(table, databaseName, Path.Combine(docPath, "Controllers"), TargetProject, overWrite);
                }


                CreateViews createViews = new CreateViews(table, Path.Combine(docPath, "Views", table.Name), TargetProject, ApplicationParts, overWrite);


            }


            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                AccessDenideView(Path.Combine(docPath, "Views", "Shared"), overWrite);
                LoginView(Path.Combine(docPath, "Views", "Shared"), overWrite, TargetProject);
                LogInModelModel(Path.Combine(docPath, "Models", "Partial"), overWrite, TargetProject);
                userLoginModel(Path.Combine(docPath, "Models", "Partial"), overWrite, TargetProject);
                StartUpClass(Path.Combine(docPath, "Models"), TargetProject, overWrite);

            }

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.Shared))
            {
                LayoutView(schema, tablesShouldGenerate, Path.Combine(docPath, "Views", "Shared"), ApplicationParts, overWrite);
                Layout_modalView(schema, tablesShouldGenerate, Path.Combine(docPath, "Views", "Shared"), overWrite);
                SearchShareView(Path.Combine(docPath, "Views", "Shared"), TargetProject, ApplicationParts, overWrite);
                HomeController(Path.Combine(docPath, "Controllers"), TargetProject, overWrite);
                HomeView(Path.Combine(docPath, "Views", "Home"), overWrite);
                DetailsShareView(Path.Combine(docPath, "Views", "Shared"), overWrite);
                DeleteShareView(Path.Combine(docPath, "Views", "Shared"), TargetProject, ApplicationParts, overWrite);
                ListShareView(Path.Combine(docPath, "Views", "Shared"), overWrite);
                OperationListShareView(Path.Combine(docPath, "Views", "Shared"), ApplicationParts, overWrite);
                dropDowAjaxShareView(Path.Combine(docPath, "Views", "Shared"), overWrite, TargetProject);
                dropDowAjaxModel(Path.Combine(docPath, "Models", "Partial"), overWrite, TargetProject);
                OperationListModel(Path.Combine(docPath, "Models", "Partial"), overWrite, TargetProject);
            }
        }

        private void StartUpClass(string docPath, string TargetProject, bool overWrite)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "Startup";

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

            lines.Add("using Microsoft.Owin;");
            lines.Add("using Microsoft.Owin.Security.Cookies;");
            lines.Add("using Owin;");
            lines.Add("using System;");
            lines.Add("using System.Collections.Generic;");
            lines.Add("using System.Linq;");
            lines.Add("using System.Web;");
            lines.Add(string.Format("[assembly: OwinStartupAttribute(typeof({0}.Models.Startup))]", TargetProject));
            lines.Add(string.Format("namespace {0}.Models", TargetProject));
            lines.Add("{    ");
            lines.Add("    public class Startup");
            lines.Add("    {");
            lines.Add("        public void Configuration(IAppBuilder app)");
            lines.Add("        {");
            lines.Add("            app.UseCookieAuthentication(new CookieAuthenticationOptions");
            lines.Add("            {");
            lines.Add("                AuthenticationType = \"ApplicationCookie\",");
            lines.Add("                LoginPath = new PathString(\"/UserLogin/LogIn\")");
            lines.Add("            });");
            lines.Add("        }");
            lines.Add("    }");
            lines.Add("}");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void LoginView(string docPath, bool overWrite, string TargetProject)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "LogIn";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();

            lines.Add(string.Format("@model {0}.Models.LogInModel", TargetProject));
            lines.Add("@{");
            lines.Add("    ViewBag.Title = \"Log In\";");
            lines.Add("}");
            lines.Add("<br />");
            lines.Add("<div class=\"col-lg-6 col-md-6 col-lg-offset-3 col-md-offset-3\">");
            lines.Add("    @using (Html.BeginForm())");
            lines.Add("    {");
            lines.Add("        <div class=\"panel panel-danger transparent\">");
            lines.Add("            <div class=\"panel-heading\">");
            lines.Add("                ورود");
            lines.Add("            </div>");
            lines.Add("            <div class=\"panel-body\">");
            lines.Add("                <div class=\"col-sm-12 col-md-12\">");
            lines.Add("                    <div class=\"form-group\">");
            lines.Add("                        <div class=\"input-group\" id=\"Div1\">");
            lines.Add("                            <span class=\"input-group-addon\" style=\"min-width: 150px;\" id=\"Span1\">نام</span>");
            lines.Add("                            <input id=\"name\" name=\"name\" type=\"text\" class=\"form-control textRight\" placeholder=\"نام\" autocomplete=\"off\" aria-describedby=\"basic-addon2\" value=\"@ViewBag.name\" />");
            lines.Add("                        </div>");
            lines.Add("                    </div>");
            lines.Add("                    <div class=\"form-group\">");
            lines.Add("                        <div class=\"input-group\" id=\"Div1\">");
            lines.Add("                            <span class=\"input-group-addon\" style=\"min-width: 150px;\" id=\"Span1\">رمزعبور</span>");
            lines.Add("                            <input id=\"Password\" name=\"Password\" type=\"password\" class=\"form-control textRight\" placeholder=\"رمزعبور\" autocomplete=\"off\" aria-describedby=\"basic-addon2\" value=\"@ViewBag.Password\" />");
            lines.Add("                        </div>");
            lines.Add("                    </div>");
            lines.Add("                    <input type=\"hidden\" id=\"ReturnUrl\" name=\"ReturnUrl\" />");
            lines.Add("                    <p>");
            lines.Add("                        <button type=\"submit\" class=\"btn btn-success btn-lg\" >ورود</button>");
            lines.Add("                    </p>");
            lines.Add("                    @Html.ValidationSummary(true)");
            lines.Add("                </div>");
            lines.Add("            </div>");
            lines.Add("        </div>");
            lines.Add("    }");
            lines.Add("</div>");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }

        private void userLoginModel(string docPath, bool overWrite, string TargetProject)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "userLoginModel";

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
            lines.Add("using System.Web.Mvc;");
            lines.Add("namespace " + TargetProject + ".Models {");
            lines.Add("public class userLoginModel");
            lines.Add("{");
            lines.Add("    public userLoginModel()");
            lines.Add("    {");
            lines.Add("        this.HistoryLogins = new HashSet<HistoryLogin>();");
            lines.Add("    }");
            lines.Add("    public int Userid { get; set; }");
            lines.Add("    public string name { get; set; }");
            lines.Add("    public string emailid { get; set; }");
            lines.Add("    public string userpassword { get; set; }");
            lines.Add("    public int roleId { get; set; }");
            lines.Add("    public string activeDateFrom { get; set; }");
            lines.Add("    public string activeDateTo { get; set; }");
            lines.Add("    public Nullable<bool> FlgActive { get; set; }");
            lines.Add("    public virtual role role { get; set; }");
            lines.Add("    public virtual ICollection<HistoryLogin> HistoryLogins { get; set; }");
            lines.Add("}");
            lines.Add("}");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void LogInModelModel(string docPath, bool overWrite, string TargetProject)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "LogInModel";

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
            lines.Add("using System.Web.Mvc;");
            lines.Add("namespace " + TargetProject + ".Models {");
            lines.Add("public class LogInModel");
            lines.Add("{");
            lines.Add("    [Required] ");
            lines.Add("    public string name { get; set; }");
            lines.Add("    [Required]");
            lines.Add("    [DataType(DataType.Password)]");
            lines.Add("    public string Password { get; set; }");
            lines.Add("    [HiddenInput]");
            lines.Add("    public string ReturnUrl { get; set; }");
            lines.Add("}");
            lines.Add("}");
            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void dropDowAjaxModel(string docPath, bool overWrite, string TargetProject)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "DropDownAjax";

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

            lines.Add("public class DropDownAjax");
            lines.Add("{");
            lines.Add("    public DropDownAjax()");
            lines.Add("    {");
            lines.Add("        Caption = \"\";");
            lines.Add("        idSearch = -1;");
            lines.Add("        ElementTextName = \"\";");
            lines.Add("        ElementTextNameValue = \"\";");
            lines.Add("        ElementTextId = \"\";");
            lines.Add("        ElementTextIdValue = \"\";");
            lines.Add("    }");
            lines.Add("    public string Caption { get; set; }");
            lines.Add("    public string Controller { get; set; }");
            lines.Add("    public string action { get; set; }");
            lines.Add("    public int idSearch { get; set; }");
            lines.Add("    public string ElementTextName { get; set; }");
            lines.Add("    public string ElementTextNameValue { get; set; }");
            lines.Add("    public string ElementTextId { get; set; }");
            lines.Add("    public string ElementTextIdValue { get; set; }");
            lines.Add("    public  bool? CreatePermission { get; set; }");
            lines.Add("    public  bool? nullable { get; set; }");
            lines.Add("}");
            lines.Add("}");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }


        private void OperationListModel(string docPath, bool overWrite, string TargetProject)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "OperationList";

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

            lines.Add("public class OperationList");
            lines.Add("{");
            lines.Add("    public int idRecord { get; set; }");
            lines.Add("public string Controller { get; set; }");
            lines.Add("public string Action { get; set; }");
            lines.Add("}");

            lines.Add("}");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }


        private void dropDowAjaxShareView(string docPath, bool overWrite, string TargetProject)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "_dropDowAjax";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();


            lines.Add(string.Format("@model {0}.Models.DropDownAjax", TargetProject));

            lines.Add("            @{");
            lines.Add("                string required = \"\", StarTag = \"\", value = \"\";");
            lines.Add("                if (Model.nullable.HasValue)");
            lines.Add("                {");
            lines.Add("                    if (!Model.nullable.Value)");
            lines.Add("                    {");
            lines.Add("                        required = \" required \";");
            lines.Add("                        StarTag = \"<span class=\\\"glyphicon glyphicon-asterisk\\\"></span>\";");
            lines.Add("                    }");
            lines.Add("                }");
            lines.Add("                if (!string.IsNullOrEmpty(Model.ElementTextIdValue))");
            lines.Add("                {");
            lines.Add("                    value = \"value = \\\"\" + Model.ElementTextIdValue + \"\\\"\";");
            lines.Add("                }");
            lines.Add("    <div class=\"input-group\">");
            lines.Add("        <div class=\"input-group-btn\" id=@string.Format(\"{0}{1}{2}\", \"input-group-btn\", \"-\", @Model.ElementTextName)>");
            lines.Add("            <button type = \"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\" style=\"min-width: 150px;\">");
            lines.Add("                @if(Model.CreatePermission.HasValue)");
            lines.Add("        {");
            lines.Add("            if (Model.CreatePermission.Value)");
            lines.Add("            {");
            lines.Add("                        <span class=\"glyphicon glyphicon-plus\" onclick='@string.Format(\"click_loadPartial('{0}', 'Create_modal'); \", @Model.Controller)' style=\"color:#0094ff;\" data-toggle=\"modal\" data-target=\".bs-example-modal-lg\"></span>");
            lines.Add("                        <span class=\"pipe\">|</span>");
            lines.Add("                    }");
            lines.Add("}");
            lines.Add("@string.Format(\"{0}\", @Model.Caption)");
            lines.Add("                <span class=\"caret\"></span>");
            lines.Add("                @Html.Raw(@StarTag)");
            lines.Add("            </button>");
            lines.Add("            <ul class=\"dropdown-menu\" id=@string.Format(\"{0}{1}{2}\", \"dropdown-menu\",\"-\", @Model.ElementTextName)></ul>");
            lines.Add("        </div><!-- /btn-group -->");
            lines.Add("        <input autocomplete = \"off\"");
            lines.Add("               controller='@Model.Controller' action='@Model.action' placeholder='@Model.Caption'               ");
            lines.Add("               id=\"@Model.ElementTextName\" name=\"@Model.ElementTextName\" class=\"form-control\" value=\"@Model.ElementTextNameValue\"");
            lines.Add("               data-ElementTextId =\"@Model.ElementTextId\" data-idSearch =\"@Model.idSearch\"");
            lines.Add("               onkeyup='@string.Format(\"_keydown('{0}','{1}');\", @Model.ElementTextId, @Model.ElementTextName)'     ");
            lines.Add("                onmouseup='@string.Format(\"_keydown('{0}','{1}');\", @Model.ElementTextId, @Model.ElementTextName)'   />");
            lines.Add("        <input id = \"@Model.ElementTextId\" type=\"hidden\" name=\"@Model.ElementTextId\" @value @required shouldFocus=\"@Model.ElementTextName\" />");
            lines.Add("    </div>");
            lines.Add("}");
            lines.Add("<script>");
            lines.Add("$().ready(() => {");
            lines.Add("_keydown('@Model.ElementTextId', '@Model.ElementTextName', '@Model.idSearch',false);");
            lines.Add("})");
            lines.Add("</script>");


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void DeleteShareView(string docPath, string TargetProject, List<EnumApplicationsPart> ApplicationParts, bool overWrite)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "_Delete";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();
            lines.Add("@model dynamic");
            lines.Add(" <div class=\"panel panel-danger transparent\">");
            lines.Add(" <div class=\"panel-heading\">");
            lines.Add("     حذف");
            lines.Add(" </div>");
            lines.Add(" <div class=\"panel-body\">");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("@if (!string.IsNullOrEmpty(User.Identity.Name))");
                lines.Add("{");
                lines.Add(string.Format("    {0}.Controllers.UserLoginController userLogin = new {0}.Controllers.UserLoginController();", TargetProject));
                lines.Add("var user = userLogin.Find(new " + TargetProject + ".Models.UserLoginSearchModel { name = User.Identity.Name }).FirstOrDefault();");
                lines.Add("if (user == null)");
                lines.Add("{");
                lines.Add("    Html.RenderPartial(\"_AccessDenide\");");
                lines.Add("}");
                lines.Add("else if (user.role.del.HasValue)");
                lines.Add("    {");
                lines.Add("        if (!user.role.del.Value)");
                lines.Add("        {");
                lines.Add("            Html.RenderPartial(\"_AccessDenide\");");
                lines.Add("        }");
                lines.Add("        else");
                lines.Add("        {");
            }
            lines.Add("            <div id = \"DeleteLayoutModal\">");
        
            lines.Add("            @using(Html.BeginForm())");
            lines.Add("            {");
            lines.Add("                <div class=\"alert alert-warning alert-dismissible\" role=\"alert\">");
            lines.Add("                    <button type = \"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
            lines.Add("                    <strong>توجه!</strong> آیا از حذف اطلاعات زیر مطمئن هستید؟");
            lines.Add("");
            lines.Add("                    <button type =\"button\" class=\"btn btn-success\" data-dismiss=\"modal\">بستن</button>");
            lines.Add("                    <input id =\"submit\" type=\"submit\" value=\"بله\" class=\"btn btn-danger\" />");
            lines.Add("                </div>");
            lines.Add("                            @Html.Partial(\"Element\")");
            lines.Add("                            ");
            lines.Add("            }");
            lines.Add("        </div>");



            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("        }");
                lines.Add("    }");
                lines.Add("}");
            }
            lines.Add("<script type =\"text/javascript\" >");
            lines.Add("");
            lines.Add("    function ConfigModalForm()");
            lines.Add("{");
            lines.Add("        $('#DeleteLayoutModal').find('input').each((i, o) => {");
            lines.Add("            $(o).prop(\"disabled\", true);");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $('#submit').prop(\"disabled\", false);");
            lines.Add("");
            lines.Add("        $('#DeleteLayoutModal').find('.glyphicon-plus').each((i, o) => {");
            lines.Add("            $(o).remove();");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $('#DeleteLayoutModal').find('.caret').each((i, o) => {");
            lines.Add("            $(o).remove();");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $('#DeleteLayoutModal').find('.glyphicon-asterisk').each((i, o) => {");
            lines.Add("            $(o).remove();");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $('#DeleteLayoutModal').find('.pipe').each((i, o) => {");
            lines.Add("            $(o).remove();");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $(\"#DeleteLayoutModal\").find(\"[data-toggle='dropdown']\").each((i, o) => {");
            lines.Add("$(o).prop('disabled', true);");
            lines.Add("         });");
            lines.Add("}");
            lines.Add("");
            lines.Add("    $(document).ready(function ()");
            lines.Add("{");
            lines.Add("    ConfigModalForm();");
            lines.Add("});");
            lines.Add("</script>");


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }

        private void DetailsShareView(string docPath, bool overWrite)
        {

            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "_Details";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();


            lines.Add("@model dynamic");
            lines.Add("@{");
            lines.Add("    ViewBag.Title = \"Details\";");
            lines.Add("    Layout = \"~/Views/Shared/_Layout.cshtml\";");
            lines.Add("}");
            lines.Add(" <div class=\"panel panel-danger transparent\">");
            lines.Add(" <div class=\"panel-heading\">");
            lines.Add("     جزئیات");
            lines.Add(" </div>");
            lines.Add(" <div class=\"panel-body\">");
            lines.Add(" @Html.Partial(\"Element\")");
            lines.Add("<p>");
            lines.Add("@Html.ActionLink(\"برگشت به لیست\", \"Index\")");
            lines.Add("</p>");
            lines.Add(" </div>");
            lines.Add(" </div>");
            lines.Add("<script type=\"text/javascript\">");
            lines.Add("    $(\"* :input\").prop(\"disabled\", true);");
            lines.Add("    $('#submit').removeAttr('disabled');");
            lines.Add("            $(document).ready(function () {");
            lines.Add("$('.glyphicon-plus').remove();");
            lines.Add("$('.caret').remove();");
            lines.Add("$('.glyphicon-asterisk').remove();");
            lines.Add("$('.pipe').remove();");
            lines.Add("});");
            lines.Add("</script>");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void HomeController(string docPath, string TargetProject, bool overWrite)
        {
            string fileName = string.Format("{0}Controller", "Home");

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
            lines.Add(string.Format("using {0}.Models;", TargetProject));
            lines.Add("using PagedList;");


            lines.Add(string.Format("namespace {0}.Controllers", TargetProject));
            lines.Add("{");
            //lines.Add("[Authorize]");
            lines.Add(string.Format("public class {0}Controller : Controller", "Home"));
            lines.Add("{");



            lines.Add("public ActionResult Index()");
            lines.Add("{");
            lines.Add("return View();");
            lines.Add("}");
            lines.Add("}");
            lines.Add("}");

            if (!System.IO.Directory.Exists(docPath))
            {
                System.IO.Directory.CreateDirectory(docPath);
            }
            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }


        private void OperationListShareView(string docPath, List<EnumApplicationsPart> ApplicationParts, bool overWrite)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "_OperationList";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();

            lines.Add("@model LavazemYadaki.Models.OperationList");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("@if (!string.IsNullOrEmpty(User.Identity.Name))");
                lines.Add("{");
                lines.Add("    hamidSamane.Controllers.UserLoginController userLogin = new hamidSamane.Controllers.UserLoginController();");
                lines.Add("    var user = userLogin.find(new hamidSamane.Models.UserLogin { name = User.Identity.Name });");
                lines.Add("");
                lines.Add("    if (user.role.upd.HasValue)");
                lines.Add("    {");
                lines.Add("        if (user.role.upd.Value)");
                lines.Add("        {");
                lines.Add("            <a>");
                lines.Add("                @Html.ActionLink(\" \", \"Edit\", new { id = @Model.idRecord }, new { @class = \"glyphicon glyphicon-edit\" }) |");
                lines.Add("            </a>");
                lines.Add("        }");
                lines.Add("    }");
                lines.Add("");
                lines.Add("    if (user.role.show.HasValue)");
                lines.Add("    {");
                lines.Add("        if (user.role.show.Value)");
                lines.Add("        {");
                lines.Add("<span class=\"glyphicon glyphicon-list-alt\" onclick=\"@string.Format(\"click_loadPartial('{0}', 'Details','{1}'); \", @Model.Controller,@Model.idRecord)\" style=\"color:#0094ff;\" data-toggle=\"modal\" data-target=\".bs-example-modal-lg\"></span> |");
                lines.Add("        }");
                lines.Add("    }");
                lines.Add("");
                lines.Add("    if (user.role.del.HasValue)");
                lines.Add("    {");
                lines.Add("        if (user.role.del.Value)");
                lines.Add("        {");
                lines.Add("            <a>");
                lines.Add("                <span class=\"glyphicon glyphicon-trash\" onclick=\"@string.Format(\"click_loadPartial('{0}', 'Delete','{1}'); \", @Model.Controller, @Model.idRecord)\" style=\"color:#0094ff;\" data-toggle=\"modal\" data-target=\".bs-example-modal-lg\"></span> |");
                lines.Add("            </a>");
                lines.Add("        }");
                lines.Add("    }");
                lines.Add("");
                lines.Add("}");
            }
            else
            {
                lines.Add("            <a>");
                lines.Add("                @Html.ActionLink(\" \", \"Edit\", new { id = @Model.idRecord }, new { @class = \"glyphicon glyphicon-edit\" }) |");
                lines.Add("            </a>");
                lines.Add("<span class=\"glyphicon glyphicon-list-alt\" onclick=\"@string.Format(\"click_loadPartial('{0}', 'Details','{1}'); \", @Model.Controller,@Model.idRecord)\" style=\"color:#0094ff;\" data-toggle=\"modal\" data-target=\".bs-example-modal-lg\"></span> |");
                lines.Add("<span class=\"glyphicon glyphicon-trash\" onclick=\"@string.Format(\"click_loadPartial('{0}', 'Delete','{1}'); \", @Model.Controller, @Model.idRecord)\" style=\"color:#0094ff;\" data-toggle=\"modal\" data-target=\".bs-example-modal-lg\"></span> |");

            }


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void ListShareView(string docPath, bool overWrite)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "_List";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();



            //lines.Add("<div class=\"panel panel-danger transparent\">");
            //lines.Add("    <div class=\"panel-heading\">");
            //lines.Add("        لیست داده ها");
            //lines.Add("    </div>");
            //lines.Add("    <div class=\"panel-body Grid-panel\">");
            //lines.Add("        <div class=\"col-sm-12 col-md-12\">");
            lines.Add("            @Html.Partial(\"List\")");
            //lines.Add("        </div>");
            //lines.Add("    </div>");
            //lines.Add("</div>");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }

        private void SearchShareView(string docPath, string targetProject, List<EnumApplicationsPart> ApplicationParts, bool overWrite)
        {

            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "_Search";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();


            //lines.Add("          <div class=\"panel panel-danger transparent\">");
            //lines.Add("    <div class=\"panel-heading\">");
            //lines.Add("        عملیات");
            //lines.Add("    </div>");
            //lines.Add("    <div class=\"panel-body\">");
            //lines.Add("        <div class=\"col-sm-12 col-md-12\">");
            lines.Add("            @using (Html.BeginForm())");
            lines.Add("            {");
            lines.Add("                <div class=\"col-lg-10 col-md-12\">");
            lines.Add("                    @Html.Partial(\"Search\")");
            lines.Add("                    @*<select class=\"form-control\" id=\"pageSize\" name=\"pageSize\">");
            lines.Add("                            <option>2</option>");
            lines.Add("                            <option>5</option>");
            lines.Add("                            <option>10</option>");
            lines.Add("                            <option>20</option>");
            lines.Add("                            <option>1000</option>");
            lines.Add("                        </select>*@");
            lines.Add("                    <div class=\"col-lg-6 col-md-12\">");
            lines.Add("                        @{");
            lines.Add("                            <div class=\"input-group\">");
            lines.Add("                                <div class=\"input-group-btn\">");
            lines.Add("                                    <button type=\"button\" id=\"pageSize_btn\" name=\"pageSize_btn\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\" style=\"min-width: 150px;\">");
            lines.Add("                                        تعداد ردیف در صفحه");
            lines.Add("                                        <span class=\"caret\"></span>");
            lines.Add("                                    </button>");
            lines.Add("                                    <ul class=\"dropdown-menu\">");
            lines.Add("                                        <li><a onclick=@string.Format(\"pageSizeSelect('{0}','{1}');\",  2, 2)>2</a></li>");
            lines.Add("                                        <li><a onclick=@string.Format(\"pageSizeSelect('{0}','{1}');\",  5, 5)>5</a></li>");
            lines.Add("                                        <li><a onclick=@string.Format(\"pageSizeSelect('{0}','{1}');\",  20, 20)>20</a></li>");
            lines.Add("                                        <li><a onclick=@string.Format(\"pageSizeSelect('{0}','{1}');\",  100, 100)>100</a></li>");
            lines.Add("                                        <li><a onclick=@string.Format(\"pageSizeSelect('{0}','{1}');\",  500, 500)>500</a></li>");
            lines.Add("                                        <li><a onclick=@string.Format(\"pageSizeSelect('{0}','{1}');\",  10000, 10000)>10000</a></li>");
            lines.Add("                                    </ul>");
            lines.Add("                                </div><!-- /btn-group -->");
            lines.Add("                                <input autocomplete=\"off\" id=\"pageSize_text\" name=\"pageSize_text\" class=\"form-control\" aria-label=\"...\" disabled value=\"@ViewBag.pageSize_text\" />");
            lines.Add("                                <input autocomplete=\"off\" id=\"pageSize\" name=\"pageSize\" class=\"form-control\" aria-label=\"...\" type=\"hidden\" value=@ViewBag.pageSize>");
            lines.Add("                            </div>");
            lines.Add("                        }");
            lines.Add("                    </div>");
            lines.Add("                    <div class=\"col-lg-6 col-md-12\">");
            lines.Add("                        @{");
            lines.Add("                            <div class=\"input-group\">");
            lines.Add("                                <div class=\"input-group-btn\">");
            lines.Add("                                    <button type=\"button\" id=\"orderColumn_btn\" name=\"orderColumn_btn\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\" style=\"min-width: 150px;\">");
            lines.Add("                                        مرتب سازی براساس");
            lines.Add("                                        <span class=\"caret\"></span>");
            lines.Add("                                    </button>");
            lines.Add("                                    <ul class=\"dropdown-menu\" id=\"orderColumndropdownmenu\">");
            lines.Add("                                        <li><a onclick=@string.Format(\"orderColumnSelect('{0}','{1}');\", 2, 2)>2</a></li>");
            lines.Add("                                        <li><a onclick=@string.Format(\"orderColumnSelect('{0}','{1}');\", 2, 2)>2</a></li>");
            lines.Add("                                        <li><a onclick=@string.Format(\"orderColumnSelect('{0}','{1}');\", 2, 2)>2</a></li>");
            lines.Add("                                    </ul>");
            lines.Add("                                </div><!-- /btn-group -->");
            lines.Add("                                <input autocomplete=\"off\" id=\"sortOrder_text\" name=\"sortOrder_text\" class=\"form-control\" aria-label=\"...\" disabled value=\"@ViewBag.sortOrder_text\" />");
            lines.Add("                                <input autocomplete=\"off\" id=\"sortOrder\" name=\"sortOrder\" class=\"form-control\" aria-label=\"...\" type=\"hidden\" value=\"@ViewBag.sortOrder\" />");
            lines.Add("                            </div>");
            lines.Add("                        }");
            lines.Add("                    </div>");
            lines.Add("                </div>");
            lines.Add("                <div class=\"col-lg-2 col-md-12\">");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("                    @if (!string.IsNullOrEmpty(User.Identity.Name))");
                lines.Add("                    {");
                lines.Add(string.Format("                        {0}.Controllers.UserLoginController userLogin = new {0}.Controllers.UserLoginController();", targetProject));
                lines.Add("                        var user = userLogin.Find(new " + targetProject + ".Models.UserLoginSearchModel { name = User.Identity.Name }).FirstOrDefault();");
                lines.Add("                        if (user.role.ins.HasValue)");
                lines.Add("                        {");
                lines.Add("                            if (user.role.ins.Value)");
                lines.Add("                            {");
                lines.Add("                                <p>");
                lines.Add("                                    @Html.ActionLink(\"افزودن داده (alt+n)\", \"Create\", null, new { @class = \"btn btn-success btn-block\", accesskey = \"n\" })");
                lines.Add("                                </p>");
                lines.Add("                            }");
                lines.Add("                        }");
                lines.Add("                    }");
            }
            else
            {
                lines.Add("                                <p>");
                lines.Add("                                    @Html.ActionLink(\"افزودن داده (alt+n)\", \"Create\", null, new { @class = \"btn btn-success btn-block\", accesskey = \"n\" })");
                lines.Add("                                </p>");
            }
            lines.Add("                    <p>");
            lines.Add("                        <input type=\"submit\" value=\"جستجو\" class=\"btn btn-success btn-block\" />");
            lines.Add("                    </p>");
            lines.Add("                    <p>");
            lines.Add("                        <a id=\"btnEmptySearch\" class=\"btn btn-success btn-block\">نمایش همه</a>");
            lines.Add("                    </p>");
            lines.Add("                </div>");
            lines.Add("            }");
            //lines.Add("        </div>");
            //lines.Add("    </div>");
            //lines.Add("</div>");

            lines.Add("  <script>");
            lines.Add("    $('#btnEmptySearch').click(function () {");
            lines.Add("        $('input:text').val('');");
            lines.Add("        $('input:hidden').val('');");
            lines.Add("        $(\"form\").submit();");
            lines.Add("    });");
            lines.Add("    function pageSizeSelect(id, text) {");
            lines.Add("        text = text.replace(/\\*/g, ' ');");
            lines.Add("        $('#pageSize_text').val(text);");
            lines.Add("        $('#pageSize').val(id);");
            lines.Add("        $(\"form\").submit();");
            lines.Add("    }");
            lines.Add("    function orderColumnSelect(id, text) {");
            lines.Add("        text = text.replace(/\\*/g, ' ');");
            lines.Add("        $('#sortOrder_text').val(text);");
            lines.Add("        $('#sortOrder').val(id);");
            lines.Add("        $(\"form\").submit();");
            lines.Add("    }");
            lines.Add("</script>");


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);


        }

        private void HomeView(string docPath, bool overWrite)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "Index";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }

            List<string> lines = new List<string>();




            lines.Add("@{");
            lines.Add("ViewBag.Title = \"Home Page\";");
            lines.Add("Layout = \"~/Views/Shared/_Layout.cshtml\";");
            lines.Add("}");

            lines.Add("<br />");
            lines.Add("<ol class=\"breadcrumb\">");
            lines.Add("<li><a href=\"#\">پیمایش</a></li>");
            lines.Add("<li class=\"active\">خانه</li>");
            lines.Add("</ol>");


            lines.Add("<p>");
            lines.Add("</p>");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }


        private void LayoutView(DatabaseSchema schema, string tablesShouldGenerate, string docPath, List<EnumApplicationsPart> ApplicationParts, bool overWrite)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "_Layout";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();


            lines.Add("<!DOCTYPE html>");
            lines.Add("<html>");
            lines.Add("<head>");
            lines.Add("    <meta charset=\"utf-8\" />");
            lines.Add("    <meta name=\"viewport\" content=\"width=device-width\" />");
            lines.Add("    @Styles.Render(\"~/Content/css\")");
            lines.Add("    @Scripts.Render(\"~/bundles/modernizr\")");
            lines.Add("");
            lines.Add("    <link href=\"~/Content/bootstrap.min.css\" rel=\"stylesheet\" />");
            lines.Add("    <link href=\"~/Content/bootstrap-rtl.min_.css\" rel=\"stylesheet\" />");
            lines.Add("    <link href=\"~/Content/Site.css\" rel=\"stylesheet\" />");
            lines.Add("    <link href=\"~/Content/pretty-checkbox.min.css\" rel=\"stylesheet\" />");
            lines.Add("");
            lines.Add("    <script src=\"~/Scripts/jquery-1.10.2.min.js\"></script>");
            lines.Add("    <script src=\"~/Scripts/bootstrap.min.js\"></script>");
            lines.Add("    <script src=\"~/Scripts/Custome.js\"></script>");
            lines.Add("    <title>سامانه ...</title>");
            lines.Add("</head>");
            lines.Add("<body>");
            lines.Add("    <div class=\"navbar navbar-default navbar-fixed-top\">");
            lines.Add("        <div class=\"container\">");
            lines.Add("            <div class=\"navbar-header\">");
            lines.Add("                <button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\".navbar-collapse\">");
            lines.Add("                    <span class=\"icon-bar\"></span>");
            lines.Add("                    <span class=\"icon-bar\"></span>");
            lines.Add("                    <span class=\"icon-bar\"></span>");
            lines.Add("                </button>");
            lines.Add("                @Html.ActionLink(\"(سامانه ...)\", \"Index\", \"Home\", null, new { @class = \"navbar-brand\" })");
            lines.Add("            </div>");
            lines.Add("            <div class=\"navbar-collapse collapse\" style=\"color:#f65656;font-size:larger;\">");
            lines.Add("");
            lines.Add("                <ul class=\"nav navbar-nav\">");
            lines.Add("                    <li>@Html.ActionLink(\"خانه\", \"Index\", \"Home\")</li>");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("                    @if (!string.IsNullOrEmpty(User.Identity.Name))");
                lines.Add("                    {");
            }

            lines.Add("                            <li class=\"dropdown\">");
            lines.Add("                                <a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" role=\"button\" aria-haspopup=\"true\" aria-expanded=\"false\">");
            lines.Add("                                    مدیریت اطلاعات");
            lines.Add("                                    <span class=\"caret\"></span>");
            lines.Add("                                </a>");
            lines.Add("                                <ul class=\"dropdown-menu\">");
            lines.Add("");

            string[] tablesAccept = tablesShouldGenerate.Split(',');
            foreach (var table in schema.Tables)
            {

                #region CheckTable

                if (table.Name.ToLower().Trim().Contains("sysdiagrams"))
                {
                    continue;
                }

                if (!tablesShouldGenerate.ToLower().Trim().Equals("*"))
                {
                    bool found = false;
                    foreach (var tableAccept in tablesAccept)
                    {
                        if (tableAccept.ToLower().Trim().Equals(table.Name.ToLower().Trim()))
                        {
                            found = true;
                            break;
                        }

                    }

                    if (found == false)
                    {
                        continue;
                    }
                }

                #endregion

                lines.Add(string.Format("                                    <li>@Html.ActionLink(\"{0}\", \"Index\", \"{1}\")</li>", table.Description, table.Name));
                lines.Add("                                    <li role=\"separator\" class=\"divider\"></li>");
            }

            lines.Add("");
            lines.Add("                                </ul>");
            lines.Add("                            </li>");

            lines.Add("");

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("                        <li>");
                lines.Add("                            <a>");
                lines.Add("                                @User.Identity.Name");
                lines.Add("                                خوش آمدید");
                lines.Add("                            </a>");
                lines.Add("");
                lines.Add("                        </li>");

                lines.Add("                        <li>");
                lines.Add("                            <a href=\"@Url.Action(\"logout\", \"UserLogin\")\">خروج</a>");
                lines.Add("");
                lines.Add("                        </li>");

                lines.Add("");
                lines.Add("                    }");
                lines.Add("                    else");
                lines.Add("                    {");
                lines.Add("                        <li>");
                lines.Add("                            <a href=\"@Url.Action(\"LogIn\", \"UserLogin\")\">ورود</a>");
                lines.Add("");
                lines.Add("                        </li>");
                lines.Add("                    }");

            }
            lines.Add("                </ul>");
            lines.Add("");
            lines.Add("            </div>");
            lines.Add("        </div>");
            lines.Add("    </div>");
            lines.Add("    <div class=\"container body-content\">");
            lines.Add("<div  id=\"modal_immadiatly\" class=\"modal fade bs-example-modal-lg\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"myLargeModalLabel\">");
            lines.Add("<div class=\"modal-dialog modal-lg\" role=\"document\">");
            lines.Add("        <div class=\"modal-content\">");
            lines.Add("            <div class=\"row\">");
            lines.Add("                <div class=\"col-lg-12\">");
            lines.Add("                    <div id=\"modal_div\"></div>");
            lines.Add("                </div>");
            lines.Add("            </div>");
            lines.Add("        </div>");
            lines.Add("    </div>");
            lines.Add("</div>");
            lines.Add("");
            lines.Add("        @RenderBody()");
            lines.Add("");
            lines.Add("        @Scripts.Render(\"~/bundles/jquery\")");
            lines.Add("        @RenderSection(\"scripts\", required: false)");
            lines.Add("    </div>");
            lines.Add("");
            lines.Add("</body>");
            lines.Add("</html>");

            lines.Add("<script>");
            lines.Add("$(\"input[required], select[required]\").attr(\"oninvalid\", \"this.setCustomValidity('لطفا اطلاع مورد نظر را پر نمایید')\");");
            lines.Add("$(\"input[required], select[required]\").attr(\"oninput\", \"setCustomValidity('')\");");
            lines.Add("</script>");


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }


        private void Layout_modalView(DatabaseSchema schema, string tablesShouldGenerate, string docPath, bool overWrite)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "_Layout_modal";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();


            lines.Add("<!DOCTYPE html>");
            lines.Add("<html>");
            lines.Add("<head>");
            lines.Add("    <meta charset=\"utf-8\" />");
            lines.Add("    <meta name=\"viewport\" content=\"width=device-width\" />");
            lines.Add("    @Styles.Render(\"~/Content/css\")");
            lines.Add("    @Scripts.Render(\"~/bundles/modernizr\")");
            lines.Add("    <link href=\"~/Content/bootstrap.min.css\" rel=\"stylesheet\" />");
            lines.Add("    <link href=\"~/Content/bootstrap-rtl.min_.css\" rel=\"stylesheet\" />");
            lines.Add("    <link href=\"~/Content/Site.css\" rel=\"stylesheet\" />");
            lines.Add("    <link href=\"~/Content/pretty-checkbox.min.css\" rel=\"stylesheet\" />");
            lines.Add("    <script src=\"~/Scripts/jquery-1.10.2.min.js\"></script>");
            lines.Add("    <script src=\"~/Scripts/bootstrap.min.js\"></script>");
            lines.Add("    <script src=\"~/Scripts/Custome.js\"></script>");
            lines.Add("    <title></title>");
            lines.Add("</head>");
            lines.Add("<body>");
            lines.Add("    <div class=\"container\">");
            lines.Add("        @RenderBody()");
            lines.Add("        @Scripts.Render(\"~/bundles/jquery\")");
            lines.Add("        @RenderSection(\"scripts\", required: false)");
            lines.Add("    </div>");
            lines.Add("</body>");
            lines.Add("</html>");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void AccessDenideView(string docPath, bool overWrite)
        {
            if (!System.IO.Directory.Exists(docPath))
                System.IO.Directory.CreateDirectory(docPath);

            string fileName = "_AccessDenide";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }

            fileName = string.Format("{0}.cshtml", fileName);

            if (File.Exists(Path.Combine(docPath, fileName)))
            {
                File.Delete(Path.Combine(docPath, fileName));
            }


            List<string> lines = new List<string>();



            lines.Add("<div class=\"col-lg-6 col-md-6 col-lg-offset-3 col-md-offset-3\">");
            lines.Add("@using (Html.BeginForm())");
            lines.Add("{");
            lines.Add("<div class=\"panel panel-danger transparent\">");
            lines.Add("<div class=\"panel-heading\">");
            lines.Add("  خطای دسترسی");
            lines.Add("</div>");
            lines.Add("<div class=\"panel-body\">");
            lines.Add("<div class=\"col-sm-12 col-md-12\">");
            lines.Add("<p class=\"text-center\">");
            lines.Add(" شما اجازه دسترسی به این صفحه را ندارید");
            lines.Add("</p>");
            lines.Add("</div>");
            lines.Add("</div>");
            lines.Add("</div>");
            lines.Add("}");
            lines.Add("</div>");

            //System.Security.AccessControl.DirectorySecurity DS= new System.Security.AccessControl.DirectorySecurity();


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }







    }
}
