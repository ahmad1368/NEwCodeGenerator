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
    public class CreateViews
    {
        public CreateViews()
        {

        }

        public CreateViews(DatabaseTable table, string docPath, string TargetProject, List<EnumApplicationsPart> ApplicationParts, bool overWrite)
        {
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || (x == EnumApplicationsPart.ViewCreate && x == EnumApplicationsPart.DeleteDirectory)))
                Utility.DeleteCreateDirectory(docPath, overWrite);
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.ViewCreate))
                Create(table, docPath, TargetProject, ApplicationParts, overWrite);
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.ViewCreate_Modal))
                Create_modal(table, docPath, TargetProject, ApplicationParts, overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.ViewEdit))
                Edit(table, docPath, TargetProject, ApplicationParts, overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.ViewDelete))
                Delete(table, docPath, TargetProject, overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.ViewIndex))
                Index(table, docPath, TargetProject, overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.ViewElement))
                Element(table, docPath, TargetProject, overWrite, true);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.ViewDetails))
                Details(table, docPath, TargetProject, overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.ViewList))
                List(table, docPath, TargetProject, overWrite);

            if (ApplicationParts.Any(x => x == EnumApplicationsPart.All || x == EnumApplicationsPart.ViewSearch))
                Search(table, docPath, TargetProject, overWrite, false);
        }



        private void Create(DatabaseTable table, string docPath, string TargetProject, List<EnumApplicationsPart> ApplicationParts, bool overWrite)
        {
            string fileName = "Create";

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

            lines.Add(string.Format("@model {0}.Models.{1}", TargetProject, table.Name));

            lines.Add("@{");
            lines.Add("ViewBag.Title = \"Create\";");
            lines.Add("Layout = \"~/Views/Shared/_Layout.cshtml\";");
            lines.Add("}");

            lines.Add("<br />");
            lines.Add("<ol class=\"breadcrumb\">");
            lines.Add("<li><a href=\"#\">پیمایش</a></li>");
            lines.Add("<li><a href=\"/home\">خانه</a></li>");
            lines.Add(string.Format("<li><a href=\"/{0}\">{1}</a></li>", table.Name, table.Description));
            lines.Add(string.Format("<li class=\"active\">ایجاد {0}</li>", table.Description));
            lines.Add("</ol>");
            lines.Add(" <div class=\"panel panel-danger transparent\">");
            lines.Add(" <div class=\"panel-heading\">");
            lines.Add("     ایجاد");
            lines.Add(" </div>");
            lines.Add(" <div class=\"panel-body\">");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("{");
                lines.Add(string.Format("{0}.Controllers.UserLoginController userLogin = new {0}.Controllers.UserLoginController();", TargetProject, table.Name));
                lines.Add("var user = userLogin.Find(new " + TargetProject + ".Models.UserLoginSearchModel { name = User.Identity.Name }).FirstOrDefault();");
                lines.Add("if (user == null)");
                lines.Add("{");
                lines.Add("    Html.RenderPartial(\"_AccessDenide\");");
                lines.Add("}");
                lines.Add("else if (user.role.ins.HasValue)");
                lines.Add("{");
                lines.Add("if (!user.role.ins.Value)");
                lines.Add("{");
                lines.Add("Html.RenderPartial(\"_AccessDenide\");");
                lines.Add("}");
                lines.Add("else");
                lines.Add("{");
                lines.Add("using (Html.BeginForm())");
            }
            else
            {
                lines.Add("@using (Html.BeginForm())");
            }

            lines.Add("{");
            //lines.Add("@Html.Partial(\"Element\")");
            lines.Add(string.Format("@Html.Partial(\"Element\",new {0}.Models.{1}())", TargetProject, table.Name));
            lines.Add("<div class=\"form-group\">");
            lines.Add("<input type=\"submit\" id=\"submit\" name=\"submit\" accesskey=\"s\" value=\"(alt+s) ثبت\" class=\"btn btn-success btn-block\" />");
            lines.Add("</div>");
            lines.Add("}");

            lines.Add("<div>");
            lines.Add("@Html.ActionLink(\"برگشت به لیست\", \"Index\")");
            lines.Add("</div>");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("}");
                lines.Add("}");
                lines.Add("}");
            }
            lines.Add(" </div>");
            lines.Add(" </div>");
            lines.Add("<script>");
            lines.Add("    $('form').submit(function () {");
            lines.Add("$('#modal_div').empty();");
            lines.Add("        var foundElement = false;");
            lines.Add("        var elements = $('input:hidden');");
            lines.Add("        jQuery.each(elements, function (index, itemData) {");
            lines.Add("            if (itemData.hasAttribute('required') && (itemData.getAttribute('value') == '' || itemData.hasAttribute('value')== false)) {                ");
            lines.Add("                var itemShouldFocus = itemData.getAttribute('shouldFocus');");
            lines.Add("                var item = document.getElementById(itemShouldFocus);                 ");
            lines.Add("                alert('لطفا مقدار ' + item.getAttribute('placeholder') + ' را وارد نمایید ');");
            lines.Add("                item.focus();");
            lines.Add("                foundElement = true;");
            lines.Add("                return false;");
            lines.Add("            }");
            lines.Add("        });");
            lines.Add("        if (foundElement)");
            lines.Add("            return false;");
            lines.Add("    });");
            lines.Add("</script>");


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }

        private void Create_modal(DatabaseTable table, string docPath, string TargetProject, List<EnumApplicationsPart> ApplicationParts, bool overWrite)
        {
            string fileName = "Create_modal";

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

            lines.Add(string.Format("@model {0}.Models.{1}", TargetProject, table.Name));

            lines.Add("@{");
            lines.Add("ViewBag.Title = \"Create\";");
            lines.Add("Layout = \"~/Views/Shared/_Layout_modal.cshtml\";");
            lines.Add("}");

            lines.Add("<br />");
            lines.Add("<ol class=\"breadcrumb\">");
            lines.Add("<li class=\"active\">پیمایش</li>");
            lines.Add("<li class=\"active\">خانه</li>");
            lines.Add(string.Format("<li class=\"active\">{0}</li>", table.Description));
            lines.Add(string.Format("<li class=\"active\">ایجاد {0}</li>", table.Description));
            lines.Add("</ol>");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("@if (!string.IsNullOrEmpty(User.Identity.Name))");
                lines.Add("{");
                lines.Add(string.Format("{0}.Controllers.UserLoginController userLogin = new {0}.Controllers.UserLoginController();", TargetProject, table.Name));
                lines.Add("var user = userLogin.Find(new " + TargetProject + ".Models.UserLoginSearchModel { name = User.Identity.Name }).FirstOrDefault();");
                lines.Add("if (user == null)");
                lines.Add("{");
                lines.Add("    Html.RenderPartial(\"_AccessDenide\");");
                lines.Add("}");
                lines.Add("else if (user.role.ins.HasValue)");
                lines.Add("{");
                lines.Add("if (!user.role.ins.Value)");
                lines.Add("{");
                lines.Add("Html.RenderPartial(\"_AccessDenide\");");
                lines.Add("}");
                lines.Add("else");
                lines.Add("{");
                lines.Add("using (Html.BeginForm(null,null,FormMethod.Post, new {id = \"Create_modal_form\" }))");
            }
            else
            {
                lines.Add("@using (Html.BeginForm(null,null,FormMethod.Post, new {id = \"Create_modal_form\" }))");
            }
            lines.Add("{");
            //lines.Add("@Html.Partial(\"Element\")");
            lines.Add(string.Format("@Html.Partial(\"Element\",new {0}.Models.{1}())", TargetProject, table.Name));
            lines.Add("<div class=\"form-group\">");
            lines.Add("<input type=\"button\" id=\"Create_modal_form_submit\" name=\"Create_modal_form_submit\" accesskey=\"s\" value=\"(alt+s) ثبت\" class=\"btn btn-success btn-block\" />");
            lines.Add("<button type=\"button\" class=\"btn btn-default  btn-block\" data-dismiss=\"modal\">بستن</button>");
            lines.Add("</div>");
            lines.Add("}");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("}");
                lines.Add("}");
                lines.Add("}");
            }


            lines.Add("<script>");
            lines.Add("           $('#Create_modal_form_submit').click(function (e) {");
            lines.Add("       var foundElement = false;");
            lines.Add("       var elements = $('#Create_modal_form >>> input:text');");
            lines.Add("       jQuery.each(elements, function (index, itemData) {");
            lines.Add("           if (itemData.hasAttribute('required'))");
            lines.Add("               if ($(this).val() == '') {");
            lines.Add("                   alert('لطفا مقدار ' + $(this).attr('placeholder') + ' را وارد نمایید ');");
            lines.Add("                   $(this).focus();");
            lines.Add("                   foundElement = true;");
            lines.Add("                   return false;");
            lines.Add("               }");
            lines.Add("       });");
            lines.Add("       if (foundElement)");
            lines.Add("           return false;");

            lines.Add("       foundElement = false;");
            lines.Add("       elements = $('#Create_modal_form >> input:hidden');");
            lines.Add("       jQuery.each(elements, function (index, itemData) {");
            lines.Add("           if (itemData.hasAttribute('required') && (itemData.getAttribute('value') == '' || itemData.hasAttribute('value') == false)) {");
            lines.Add("               var itemShouldFocus = itemData.getAttribute('shouldFocus');");
            lines.Add("               var item = document.getElementById(itemShouldFocus);");
            lines.Add("               alert('لطفا مقدار ' + item.getAttribute('placeholder') + ' را وارد نمایید ');");
            lines.Add("               item.focus();");
            lines.Add("               foundElement = true;");
            lines.Add("               return false;");
            lines.Add("           }");
            lines.Add("       });");
            lines.Add("       if (foundElement)");
            lines.Add("           return false;");

            lines.Add("  $.ajax({");
            lines.Add(string.Format("    url: \"/{0}/Create_modal\",", table.Name));
            lines.Add("    data: $(\"#Create_modal_form\").serialize(),");
            lines.Add("    type: \"POST\",");
            lines.Add("    dataType: 'json',");
            lines.Add("    success: function (e) {");
            lines.Add("alert('ثبت با شماره '+ e + ' انجام گرفت ');");
            lines.Add("$('#Create_modal_form >>  input:text').val('');");
            lines.Add("$('#Create_modal_form >>>  input:text').val('');");
            lines.Add("$(\"#Create_modal_form input:text, #Create_modal_form textarea\").first().focus();");
            lines.Add("    },");
            lines.Add("    error: function (e) {");
            lines.Add("        alert('خطا در ثبت داده.لطفا مجدد امتحان کنید');");
            lines.Add("    }");
            lines.Add("});");

            lines.Add("   });");

            lines.Add("$(document).ready(function () {");
            lines.Add("$('#Create_modal_form .glyphicon-plus').remove();");
            lines.Add("$('#Create_modal_form .pipe').remove();");
            lines.Add("$(\"#Create_modal_form input:text, #Create_modal_form textarea\").first().focus();");
            lines.Add("});");
            lines.Add("</script>");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }


        private void Simple_Input(string Description, List<string> lines, string columnName, bool IsPrimaryKey, bool checkPrimaryKey, bool isNullable, int? maxLength)
        {
            string TypeInput = "";
            string required = "";
            string StarTag = "";
            string lenght = "";
            string PlaceHolderlenght = "";

            if (maxLength.HasValue)
                if (maxLength.Value > 0)
                {
                    lenght = string.Format("maxlength=\"{0}\"", maxLength);
                    PlaceHolderlenght = string.Format("حداکثر کاراکتر مجاز {0}", maxLength);
                }

            if (IsPrimaryKey && checkPrimaryKey)
            {
                TypeInput = "hidden";
                lines.Add(string.Format("  <input id=\"{0}\" name=\"{0}\"  shouldFocus\"{0}\" type=\"{2}\"  value=\"@Model.{0}\" />", columnName, Description, TypeInput));
            }
            else
            {
                TypeInput = "text";

                if (!isNullable)
                {
                    required = " required ";
                    StarTag = "<span class=\"glyphicon glyphicon-asterisk\"></span>";
                }

                lines.Add("<div class=\"form-group\">");
                lines.Add(" <div class=\"input-group\" >");
                lines.Add(string.Format("  <span class=\"input-group-addon\" style=\"min-width: 150px;\" id=\"Span1\">{0}{1}</span>", Description, StarTag));
                lines.Add(string.Format("  <input id=\"{0}\" name=\"{0}\" shouldFocus\"{0}\"  type=\"{2}\" class=\"form-control textRight\" placeholder=\"{1} {5}\" autocomplete=\"off\" aria-describedby=\"basic-addon2\" value=\"@ViewBag.{0}\" {3} {4} />", columnName, Description, TypeInput, required, lenght, PlaceHolderlenght));
                lines.Add(" </div>");
                lines.Add("</div>");
            }


        }

        private void Radio_Input(string Description, List<string> lines, string columnName, bool isNullable)
        {
            string required = "";
            string StarTag = "";
            if (!isNullable)
            {
                required = " required ";
                StarTag = "<span class=\"glyphicon glyphicon-asterisk\"></span>";
            }

            lines.Add("<div class=\"input-group\">");
            lines.Add("<span class=\"input-group-addon\">");
            lines.Add(string.Format("<input type=\"checkbox\"  style=\"min-width: 125px;\" value=\"true\" id=\"{0}\" name=\"{0}\" aria-label=\"...\" @(ViewBag.{0} == true ? \"checked\" : \" \" ) {1} >", columnName, required));
            lines.Add("</span>");
            lines.Add(string.Format("<input autocomplete=\"off\"  class=\"form-control\" aria-label=\"...\" placeholder=\"{0}{1}\" readonly>", StarTag, Description));
            lines.Add("</div>");
        }



        private void Input_Search(DatabaseColumn column, List<string> lines)
        {
            switch (column.NetDataType().Name)
            {
                case "Int16":
                case "Int32":
                case "Int64":
                case "Double":
                case "Decimal":
                case "Single":
                    lines.Add(string.Format("if ({0}Search.{1}From != 0)", column.TableName, column.Name));
                    lines.Add("{");
                    lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} >= {0}Search.{1}From);", column.TableName, column.Name));
                    lines.Add(string.Format("ViewBag.{1} = {0}Search.{1}From;", column.TableName, column.Name));
                    lines.Add("}");
                    ///////////////////////////////////////////////////////////////////////////////////////
                    lines.Add(string.Format("if ({0}Search.{1}To != 0)", column.TableName, column.Name));
                    lines.Add("{");
                    lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} <= {0}Search.{1}To);", column.TableName, column.Name));
                    lines.Add(string.Format("ViewBag.{1} = {0}Search.{1}To;", column.TableName, column.Name));
                    lines.Add("}");
                    ///////////////////////////////////////////////////////////////////////////////////////
                    break;
                case "Boolean":
                    lines.Add(string.Format("if ({0}Search.{1}.HasValue)", column.TableName, column.Name));
                    lines.Add("{");
                    lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} == {0}Search.{1}.Value);", column.TableName, column.Name));
                    lines.Add(string.Format("ViewBag.{1} = {0}Search.{1};", column.TableName, column.Name));
                    lines.Add("}");
                    break;

                case "DateTime":
                case "TimeSpan":
                    lines.Add(string.Format("if ({0}Search.{1}From.HasValue)", column.TableName, column.Name));
                    lines.Add("{");
                    lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} >= {0}Search.{1}From.Value);", column.TableName, column.Name));
                    lines.Add(string.Format("ViewBag.{1}From = {0}Search.{1}From;", column.TableName, column.Name));
                    lines.Add("}");

                    lines.Add(string.Format("if ({0}Search.{1}To.HasValue)", column.TableName, column.Name));
                    lines.Add("{");
                    lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1} <= {0}Search.{1}To.Value);", column.TableName, column.Name));
                    lines.Add(string.Format("ViewBag.{1}To = {0}Search.{1}To;", column.TableName, column.Name));
                    lines.Add("}");

                    break;

                case "String":

                    lines.Add(string.Format("if (!string.IsNullOrEmpty({0}Search.{1}))", column.TableName, column.Name));
                    lines.Add("{");
                    lines.Add(string.Format("{0}s = {0}s.Where(x => x.{1}.ToLower().Trim().Contains({0}Search.{1}.ToLower().Trim()));", column.TableName, column.Name));
                    lines.Add(string.Format("ViewBag.{1} = {0}Search.{1};", column.TableName, column.Name));
                    lines.Add("}");
                    break;

                default:
                    lines.Add(string.Format("if ({0} {1} {2} {3})", column.TableName, column.Name, "errrrrrrrrror", column.NetDataType().Name));
                    lines.Add("{");
                    lines.Add("}");
                    break;
            }
        }

        private void Search(DatabaseTable table, string docPath, string TargetProject, bool overWrite, bool CreatePermission)
        {
            string fileName = "Search";

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

            lines.Add(string.Format("@model {0}.Models.{1}SearchModel", TargetProject, table.Name));

            foreach (var column in table.Columns)
            {
                if (column.IsForeignKey)
                {
                    DropDown_input_FromTo(string.Format("{0}{1}", column.Description, " از "), lines, column.Name, "From", column.ForeignKeyTable.Name, TargetProject, CreatePermission);
                    DropDown_input_FromTo(string.Format("{0}{1}", column.Description, " تا "), lines, column.Name, "To", column.ForeignKeyTable.Name, TargetProject, CreatePermission);
                }
                else if (column.IsPrimaryKey)
                {
                    DropDown_input_FromTo(string.Format("{0}{1}", column.Description, " از "), lines, column.Name, "From", column.TableName, TargetProject, CreatePermission);
                    DropDown_input_FromTo(string.Format("{0}{1}", column.Description, " تا "), lines, column.Name, "To", column.TableName, TargetProject, CreatePermission);
                }
                else
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
                            Simple_Input(string.Format("{0}{1}", column.Description, " از"), lines, string.Format("{0}{1}", column.Name, "From"), column.IsPrimaryKey, false, true, column.Length);
                            Simple_Input(string.Format("{0}{1}", column.Description, " تا"), lines, string.Format("{0}{1}", column.Name, "To"), column.IsPrimaryKey, false, true, column.Length);
                            break;
                        case "String":
                            Simple_Input(column.Description, lines, column.Name, column.IsPrimaryKey, false, true, column.Length);
                            break;
                        case "Boolean":
                            Radio_Input(column.Description, lines, column.Name, true);

                            break;
                        default:
                            break;
                    }
            }

            lines.Add("<script>");
            lines.Add("    $(document).ready(function () {");
            lines.Add("        $('#orderColumndropdownmenu').empty();");
            foreach (var Column in table.Columns)
            {
                //lines.Add(string.Format("$('#orderColumndropdownmenu').append('<li><a onclick= orderColumnSelect(\\'{0}\\',\\'{1}\\');>{2}-نزولی</a></li>');", Column.Name + "-desc", (Column.Description != null ? Column.Description.Replace(" ", "*") : "") + "-نزولی", Column.Description));
                //lines.Add(string.Format("$('#orderColumndropdownmenu').append('<li><a onclick= orderColumnSelect(\\'{0}\\',\\'{1}\\');>{2}-صعودی</a></li>');", Column.Name + "-asc", (Column.Description != null ? Column.Description.Replace(" ", "*") : "") + "-صعودی", Column.Description));

                lines.Add(string.Format("$('#orderColumndropdownmenu').append('<li><a onclick= orderColumnSelect(\\'{0}\\',\\'{0}\\');>{0}</a></li>');", (Column.Description != null ? Column.Description.Replace(" ", "*") : "") + "-نزولی"));
                lines.Add(string.Format("$('#orderColumndropdownmenu').append('<li><a onclick= orderColumnSelect(\\'{0}\\',\\'{0}\\');>{0}</a></li>');", (Column.Description != null ? Column.Description.Replace(" ", "*") : "") + "-صعودی"));
            }
            //lines.Add("        alert('see');");
            lines.Add("    });");
            lines.Add("</script>");

            //lines.Add("</div>");


            //lines.Add(string.Format("@Html.Partial(\"Element\", new {0}.Models.{1}())",TargetProject,table.Name));

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void List(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        {
            string fileName = "List";

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

            lines.Add("@using PagedList.Mvc");


            lines.Add(string.Format("@model  PagedList.IPagedList<{0}.Models.{1}>  ", TargetProject, table.Name));

            lines.Add("<table class=\"table table-bordered table-hover table-responsive\">");
            lines.Add("<tr>");
            foreach (var Column in table.Columns)
            {
                lines.Add("<th>");
                lines.Add(Column.Description);
                lines.Add("</th>");
            }

            lines.Add("<th>عملیات</th>");
            lines.Add("</tr>");

            lines.Add("@foreach (var item in Model)");
            lines.Add("{");
            lines.Add("<tr>");
            CreateController createController = new CreateController();
            foreach (var Column in table.Columns)
            {
                lines.Add("<td>");


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
                    case "String":
                        lines.Add(string.Format("@Html.DisplayFor(modelItem => item.{0})", Column.Name));
                        break;
                    case "Boolean":
                        lines.Add(string.Format(" @if (item.{0}.HasValue)", Column.Name));
                        lines.Add("{");
                        lines.Add(string.Format("    if (item.{0}.Value == true)", Column.Name));
                        lines.Add("{");
                        lines.Add(string.Format("        <p>بله</p>"));
                        lines.Add("}");
                        lines.Add(string.Format("    else"));
                        lines.Add("{");
                        lines.Add(string.Format("         <p>خیر</p>"));
                        lines.Add("}");
                        lines.Add("}");
                        lines.Add(string.Format("else"));
                        lines.Add("{");
                        lines.Add(string.Format("    <p>مشخص نشده</p>"));
                        lines.Add("}");

                        break;

                    default:
                        break;
                }


                lines.Add("</td>");
            }

            lines.Add("<td>");


            foreach (var Column in table.Columns)
                if (Column.IsPrimaryKey)
                {
                    lines.Add("@Html.Partial(\"_OperationList\",  new LavazemYadaki.Models.OperationList { idRecord = item." + Column.Name + ",Controller =\"" + Column.TableName + "\"" + " })");


                }

            lines.Add("</td>");
            //lines.Add("</tr>");


            lines.Add("</tr>");
            lines.Add("}");

            lines.Add("</table>");


            lines.Add("<div id=\"container\" style=\"margin-left: 20px\">");
            lines.Add("<p></p>");
            lines.Add("<p></p>");
            lines.Add("<div class=\"pagination\" style=\"margin-left: 400px\">");
            lines.Add("<table>");
            lines.Add("<tr>");
            lines.Add("<td>");
            lines.Add("صفحه @(Model.PageCount < Model.PageNumber ? 0 : Model.PageNumber)");
            lines.Add("از @Model.PageCount");

            lines.Add("</td>");
            lines.Add("<td>");

            string Fields = "";
            foreach (var column in table.Columns)
            {
                if (column.IsForeignKey || column.IsPrimaryKey)
                {
                    Fields += column.Name + "From = ViewBag." + column.Name + "From , ";
                    Fields += column.Name + "To = ViewBag." + column.Name + "To , ";
                }
                else
                    Fields += column.Name + " = ViewBag." + column.Name + " , ";
            }

            Fields = Fields.Remove(Fields.Length - 4, 3);

            lines.Add("@Html.PagedListPager(Model, page => Url.Action(\"Index\", new { page, pageSize = ViewBag.pageSize , sortOrder = ViewBag.sortOrder , " + Fields + " }))");

            lines.Add("</td>");
            lines.Add("</tr>");
            lines.Add("</table>");


            lines.Add("</div>");
            lines.Add("</div>");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }

        //private void Details(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        //{
        //    string fileName = "Details";

        //    if (!overWrite)
        //    {
        //        fileName = Utility.UniqName(docPath, fileName);
        //    }


        //    fileName = string.Format("{0}.cshtml", fileName);


        //    if (File.Exists(Path.Combine(docPath, fileName)) && overWrite)
        //    {
        //        File.Delete(Path.Combine(docPath, fileName));
        //    }

        //    List<string> lines = new List<string>();

        //    lines.Add(string.Format("@model {0}.Models.{1}", TargetProject, table.Name));

        //    lines.Add("@{");
        //    lines.Add("ViewBag.Title = \"Details\";");
        //    lines.Add("Layout = \"~/Views/Shared/_Layout.cshtml\";");
        //    lines.Add("}");
        //    lines.Add("<br />");
        //    lines.Add("<ol class=\"breadcrumb\">");
        //    lines.Add("<li><a href=\"#\">پیمایش</a></li>");
        //    lines.Add("<li><a href=\"/home\">خانه</a></li>");
        //    lines.Add(string.Format("<li><a href=\"/{0}\">{1}</a></li>", table.Name, table.Description));
        //    lines.Add(string.Format("<li class=\"active\">حذف {0}</li>", table.Description));
        //    lines.Add("</ol>");

        //    lines.Add("@Html.Partial(\"_Details\")");

        //    System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        //}
        private void Details(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        {
            string fileName = "Details";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }


            fileName = string.Format("{0}.cshtml", fileName);


            if (File.Exists(Path.Combine(docPath, fileName)) && overWrite)
            {
                File.Delete(Path.Combine(docPath, fileName));
            }

            List<string> lines = new List<string>();

            lines.Add(string.Format("@model {0}.Models.{1}", TargetProject, table.Name));

            lines.Add("@{");
            lines.Add("ViewBag.Title = \"Details\";");
            lines.Add("Layout = \"~/Views/Shared/_Layout_modal.cshtml\";");
            lines.Add("}");
            lines.Add("<br />");
            lines.Add("<ol class=\"breadcrumb\">");
            lines.Add("<li><a href=\"#\">پیمایش</a></li>");
            lines.Add("<li><a href=\"/home\">خانه</a></li>");
            lines.Add(string.Format("<li><a href=\"/{0}\">{1}</a></li>", table.Name, table.Description));
            lines.Add(string.Format("<li class=\"active\">جزئیات {0}</li>", table.Description));
            lines.Add("</ol>");

            lines.Add("            <div id = \"DetailLayoutModal\" >");
            lines.Add(string.Format("     @Html.Partial(\"Element\", new LavazemYadaki.Models.{0}())", table.Name));
            lines.Add("     <div class=\"form-group\">");
            lines.Add("        <button type = \"button\" class=\"btn btn-default  btn-block\" data-dismiss=\"modal\">بستن</button>");
            lines.Add("    </div>");
            lines.Add("</div>");
            lines.Add("<script>");
            lines.Add("   ");
            lines.Add("    function ConfigModalForm()");
            lines.Add("        {");
            lines.Add("        $('#DetailLayoutModal').find('input').each((i, o) => {");
            lines.Add("            $(o).prop(\"disabled\", true);");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $('#DetailLayoutModal').find('.glyphicon-plus').each((i, o) => {");
            lines.Add("            $(o).remove();");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $('#DetailLayoutModal').find('.caret').each((i, o) => {");
            lines.Add("            $(o).remove();");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $('#DetailLayoutModal').find('.glyphicon-asterisk').each((i, o) => {");
            lines.Add("            $(o).remove();");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $('#DetailLayoutModal').find('.pipe').each((i, o) => {");
            lines.Add("            $(o).remove();");
            lines.Add("         });");
            lines.Add("");
            lines.Add("");
            lines.Add("        $('#DetailLayoutModal').find('.pipe').each((i, o) => {");
            lines.Add("            $(o).remove();");
            lines.Add("         });");
            lines.Add("");
            lines.Add("        $(\"#DetailLayoutModal\").find(\"[data-toggle='dropdown']\").each((i, o) => {");
            lines.Add("            $(o).prop('disabled', true);");
            lines.Add("         });");
            lines.Add("        }");
            lines.Add("");
            lines.Add("    $(document).ready(function ()");
            lines.Add("        {");
            lines.Add("            ConfigModalForm();");
            lines.Add("        });");
            lines.Add("</script>");


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);

        }

        private void Element(DatabaseTable table, string docPath, string TargetProject, bool overWrite, bool CreatePermission)
        {
            string fileName = "Element";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }


            fileName = string.Format("{0}.cshtml", fileName);


            if (File.Exists(Path.Combine(docPath, fileName)) && overWrite)
            {
                File.Delete(Path.Combine(docPath, fileName));
            }

            List<string> lines = new List<string>();



            lines.Add(string.Format("@model {0}.Models.{1}", TargetProject, table.Name));

            foreach (var column in table.Columns)
            {

                if (column.IsForeignKey)
                    DropDown_input(column.Description, lines, column.Name, column.ForeignKeyTable.Name, TargetProject, CreatePermission, column.Nullable);
                else
                    switch (column.NetDataType().Name)
                    {
                        case "Int16":
                        case "Int32":
                        case "Int64":
                        case "Double":
                        case "Decimal":
                        case "Single":
                        case "String":
                        case "DateTime":
                        case "TimeSpan":
                            Simple_Input(column.Description, lines, column.Name, column.IsPrimaryKey, true, column.Nullable, column.Length);
                            break;
                        case "Boolean":
                            Radio_Input(column.Description, lines, column.Name, column.Nullable);
                            break;
                        default:
                            break;
                    }
            }



            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void DropDown_input(string Description, List<string> lines, string columnName, string TableName, string targetProject, bool CreatePermission, bool nullable)
        {
            lines.Add(string.Format("@Html.Partial(\"_dropDowAjax\", new {0}.Models.DropDownAjax() {4} Caption = \"{1}\", Controller = \"{2}\", action = \"{2}s\", idSearch = (ViewBag.{3}!=null? ViewBag.{3}:0) , ElementTextName = \"{3}sText\", ElementTextId = \"{3}\" ,nullable ={6} , CreatePermission= {7} {5})", targetProject, Description, TableName, columnName, "{", "}", nullable.ToString().ToLower(), CreatePermission.ToString().ToLower()));

        }

        private void DropDown_input_FromTo(string Description, List<string> lines, string columnName, string FromTo, string TableName, string targetProject, bool CreatePermission)
        {
            lines.Add(string.Format("@Html.Partial(\"_dropDowAjax\", new {0}.Models.DropDownAjax() {4} Caption = \"{1}\", Controller = \"{2}\", action = \"{2}s\", idSearch = (ViewBag.{3}{6}!=null? ViewBag.{3}{6}:0) , ElementTextName = \"{3}sText{6}\", ElementTextId = \"{3}{6}\",CreatePermission = {7} {5})", targetProject, Description, TableName, columnName, "{", "}", FromTo, CreatePermission.ToString().ToLower()));

        }

        //private void DropDown_input(DatabaseColumn column, List<string> lines, string targetProject)
        //{
        //    lines.Add(string.Format("@Html.Partial(\"_dropDowAjax\", new {0}.Models.DropDownAjax() {4} Caption = \"{1}\", Controller = \"{2}\", action = \"{2}s\", idSearch = (ViewBag.{3}!=null? ViewBag.{3}:0) , ElementTextName = \"{3}sText\", ElementTextId = \"{3}\" {5})", targetProject, column.Description, column.ForeignKeyTable.Name, column.Name, "{", "}"));

        //}

        private void Index(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        {
            string fileName = "Index";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }


            fileName = string.Format("{0}.cshtml", fileName);


            if (File.Exists(Path.Combine(docPath, fileName)) && overWrite)
            {
                File.Delete(Path.Combine(docPath, fileName));
            }

            List<string> lines = new List<string>();


            lines.Add(string.Format("@model IEnumerable<{0}.Models.{1}>", TargetProject, table.Name));


            lines.Add("@{");
            lines.Add("ViewBag.Title = \"Index\";");
            lines.Add("Layout = \"~/Views/Shared/_Layout.cshtml\";");
            lines.Add("}");

            lines.Add("<br />");
            lines.Add("<ol class=\"breadcrumb\">");
            lines.Add("<li><a href=\"#\">پیمایش</a></li>");
            lines.Add("<li><a href=\"/home\">خانه</a></li>");
            lines.Add(string.Format("<li class=\"active\">{0}</li>", table.Description));
            lines.Add("</ol>");

            lines.Add("            <div class=\"panel-group\" id=\"accordion\" role=\"tablist\" aria-multiselectable=\"true\">");
            lines.Add("    <div class=\"panel panel-default\">");
            lines.Add("        <div class=\"panel-heading\" role=\"tab\" id=\"headingOne\">");
            lines.Add("            <h4 class=\"panel-title\">");
            lines.Add("                <a role=\"button\" data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#collapseOne\" aria-expanded=\"true\" aria-controls=\"collapseOne\">");
            lines.Add("                    عملیات");
            lines.Add("                </a>");
            lines.Add("            </h4>");
            lines.Add("        </div>");
            lines.Add("        <div id=\"collapseOne\" class=\"panel-collapse collapse\" role=\"tabpanel\" aria-labelledby=\"headingOne\">");
            lines.Add("            <div class=\"panel-body\">");
            lines.Add(string.Format("@Html.Partial(\"_Search\", new {0}.Models.{1}SearchModel())", TargetProject, table.Name));
            lines.Add("            </div>");
            lines.Add("        </div>");
            lines.Add("    </div>");
            lines.Add("    <div class=\"panel panel-default\">");
            lines.Add("        <div class=\"panel-heading\" role=\"tab\" id=\"headingTwo\">");
            lines.Add("            <h4 class=\"panel-title\">");
            lines.Add("                <a class=\"collapsed\" role=\"button\" data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#collapseTwo\" aria-expanded=\"false\" aria-controls=\"collapseTwo\">");
            lines.Add("                    لیست داده ها");
            lines.Add("                </a>");
            lines.Add("            </h4>");
            lines.Add("        </div>");
            lines.Add("        <div id=\"collapseTwo\" class=\"panel-collapse  collapse in\" role=\"tabpanel\" aria-labelledby=\"headingTwo\">");
            lines.Add("            <div class=\"panel-body  Grid-panel\">");
            //lines.Add("<div class=\"panel panel-default\">");
            //lines.Add("<div class=\"panel-body\">");
            //foreach (var column in table.Columns)
            //    if (column.IsForeignKey)
            //    {
            //        lines.Add("<a class=\"btn btn-default btn-lg\" href=\"\\" + column.ForeignKeyTableName + "\">" + column.ForeignKeyTable.Description + "</a>");
            //    }

            //lines.Add("  </div>");
            //lines.Add("</div>");
            lines.Add("@Html.Partial(\"_List\")");
            lines.Add("            </div>");
            lines.Add("        </div>");
            lines.Add("    </div>    ");
            lines.Add("</div>");


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void Delete(DatabaseTable table, string docPath, string TargetProject, bool overWrite)
        {

            string fileName = "Delete";

            if (!overWrite)
            {
                fileName = Utility.UniqName(docPath, fileName);
            }


            fileName = string.Format("{0}.cshtml", fileName);


            if (File.Exists(Path.Combine(docPath, fileName)) && overWrite)
            {
                File.Delete(Path.Combine(docPath, fileName));
            }

            List<string> lines = new List<string>();

            lines.Add(string.Format("@model {0}.Models.{1}", TargetProject, table.Name));
            lines.Add("@{");
            lines.Add("ViewBag.Title = \"حذف\";");
            lines.Add("Layout = \"~/Views/Shared/_Layout_modal.cshtml\";");
            lines.Add("}");
            lines.Add("<br />");
            lines.Add("<ol class=\"breadcrumb\">");
            lines.Add("<li><a href=\"#\">پیمایش</a></li>");
            lines.Add("<li><a href=\"/home\">خانه</a></li>");
            lines.Add(string.Format("<li><a href=\"/{0}\">{1}</a></li>", table.Name, table.Description));
            lines.Add(string.Format("<li class=\"active\">حذف {0}</li>", table.Description));
            lines.Add("</ol>");
            lines.Add("@Html.Partial(\"_Delete\")");

            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }

        private void Edit(DatabaseTable table, string docPath, string TargetProject, List<EnumApplicationsPart> ApplicationParts, bool overWrite)
        {
            string fileName = "Edit";

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

            lines.Add(string.Format("@model {0}.Models.{1}", TargetProject, table.Name));

            lines.Add("@{");
            lines.Add("ViewBag.Title = \"Edit\";");
            lines.Add("Layout = \"~/Views/Shared/_Layout.cshtml\";");
            lines.Add("}");

            lines.Add("<br />");
            lines.Add("<ol class=\"breadcrumb\">");
            lines.Add("<li><a href=\"#\">پیمایش</a></li>");
            lines.Add("<li><a href=\"/home\">خانه</a></li>");
            lines.Add(string.Format("<li><a href=\"/{0}\">{1}</a></li>", table.Name, table.Description));
            lines.Add(string.Format("<li class=\"active\">ایجاد {0}</li>", table.Description));
            lines.Add("</ol>");

            lines.Add(" <div class=\"panel panel-danger transparent\">");
            lines.Add(" <div class=\"panel-heading\">");
            lines.Add("     ویرایش");
            lines.Add(" </div>");
            lines.Add(" <div class=\"panel-body\">");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("@if (!string.IsNullOrEmpty(User.Identity.Name))");
                lines.Add("{");
                lines.Add(string.Format("{0}.Controllers.UserLoginController userLogin = new {0}.Controllers.UserLoginController();", TargetProject, table.Name));
                lines.Add("var user = userLogin.Find(new " + TargetProject + ".Models.UserLoginSearchModel { name = User.Identity.Name }).FirstOrDefault();");
                lines.Add("if (user == null)");
                lines.Add("{");
                lines.Add("    Html.RenderPartial(\"_AccessDenide\");");
                lines.Add("}");
                lines.Add("else if (user.role.upd.HasValue)");
                lines.Add("{");
                lines.Add("if (!user.role.upd.Value)");
                lines.Add("{");
                lines.Add("Html.RenderPartial(\"_AccessDenide\");");
                lines.Add("}");
                lines.Add("else");
                lines.Add("{");
                lines.Add("using (Html.BeginForm())");
            }
            else
            {
                lines.Add("@using (Html.BeginForm())");
            }
            lines.Add("{");
            lines.Add("@Html.Partial(\"Element\")");
            lines.Add("<div class=\"form-group\">");
            lines.Add("<input type=\"submit\" id=\"submit\" name=\"submit\" accesskey=\"s\" value=\"(alt+s) ثبت\" class=\"btn btn-success btn-block\" />");
            lines.Add("</div>");
            lines.Add("}");

            lines.Add("<div>");
            lines.Add("@Html.ActionLink(\"برگشت به لیست\", \"Index\")");
            lines.Add("</div>");
            if (ApplicationParts.Any(x => x == EnumApplicationsPart.Access))
            {
                lines.Add("}");
                lines.Add("}");
                lines.Add("}");
            }
            lines.Add(" </div>");
            lines.Add(" </div>");
            lines.Add("            <script>");
            lines.Add("    $('form').submit(function () {");
            lines.Add("$('#modal_div').empty();");
            lines.Add("        var foundElement = false;");
            lines.Add("        var elements = $('input:hidden');");
            lines.Add("        jQuery.each(elements, function (index, itemData) {");
            lines.Add("            if (itemData.hasAttribute('required') && (itemData.getAttribute('value') == '' || itemData.hasAttribute('value')== false)) {");
            lines.Add("                var itemShouldFocus = itemData.getAttribute('shouldFocus');");
            lines.Add("                var item = document.getElementById(itemShouldFocus);");
            lines.Add("                alert('لطفا مقدار ' + item.getAttribute('placeholder') + ' را وارد نمایید ');");
            lines.Add("                item.focus();");
            lines.Add("                foundElement = true;");
            lines.Add("                return false;");
            lines.Add("            }");
            lines.Add("        });");
            lines.Add("        if (foundElement)");
            lines.Add("            return false;");
            lines.Add("    });");
            lines.Add("</script>");


            System.IO.File.WriteAllLines(Path.Combine(docPath, fileName), lines, Encoding.Unicode);
        }


    }

}
