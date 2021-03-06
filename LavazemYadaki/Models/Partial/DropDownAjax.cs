using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models {
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
    public  bool? CreatePermission { get; set; }
    public  bool? nullable { get; set; }
}
}
