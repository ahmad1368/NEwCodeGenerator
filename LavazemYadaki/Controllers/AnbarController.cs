using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using LavazemYadaki.Models;
using PagedList;
using System.Security.Claims;
namespace LavazemYadaki.Controllers
{
public class AnbarController : Controller
{
private LavazemYadakiEntities db = new LavazemYadakiEntities();
public IQueryable<Anbar> Find(AnbarSearchModel AnbarSearch)
{
var Anbars = db.Anbar.AsQueryable<Anbar>();
if (AnbarSearch.AnbarIdFrom != 0)
{
Anbars = Anbars.Where(x => x.AnbarId >= AnbarSearch.AnbarIdFrom);
ViewBag.AnbarIdFrom = AnbarSearch.AnbarIdFrom;
}
if (AnbarSearch.AnbarIdTo != 0)
{
Anbars = Anbars.Where(x => x.AnbarId <= AnbarSearch.AnbarIdTo);
ViewBag.AnbarIdTo = AnbarSearch.AnbarIdTo;
}
if (!string.IsNullOrEmpty(AnbarSearch.TitleAnbar))
{
Anbars = Anbars.Where(x => x.TitleAnbar.ToLower().Trim().Contains(AnbarSearch.TitleAnbar.ToLower().Trim()));
ViewBag.TitleAnbar = AnbarSearch.TitleAnbar;
}
return Anbars;
}
[HttpPost]
public ActionResult Index(AnbarSearchModel AnbarSearch)
{
if (AnbarSearch.sortOrder == null)
AnbarSearch.sortOrder = "شناسه انبار-نزولی";
AnbarSearch.sortOrder = AnbarSearch.sortOrder.Replace("*", " ");
var Anbardata = Find(AnbarSearch);
string a = AnbarSearch.sortOrder;
char[] c = { '-' };
string[] arr = a.Split(c);
var AnbarSorted = sort(Anbardata, arr);
var data = AnbarSorted.ToPagedList(AnbarSearch.page, AnbarSearch.pageSize);
foreach (var item in data)
{
}
ViewBag.pageSize_text = AnbarSearch.pageSize;
ViewBag.pageSize = AnbarSearch.pageSize;
ViewBag.sortOrder_text = AnbarSearch.sortOrder;
ViewBag.sortOrder = AnbarSearch.sortOrder;
return View(data);
}
public ActionResult Index(AnbarSearchModel AnbarSearch, bool? temp)
{
if (AnbarSearch.sortOrder == null)
AnbarSearch.sortOrder = "شناسه انبار-نزولی";
AnbarSearch.sortOrder = AnbarSearch.sortOrder.Replace("*", " ");
var Anbardata = Find(AnbarSearch);
string a = AnbarSearch.sortOrder;
char[] c = { '-' };
string[] arr = a.Split(c);
var AnbarSorted = sort(Anbardata, arr);
var data = AnbarSorted.ToPagedList(AnbarSearch.page, AnbarSearch.pageSize);
foreach (var item in data)
{
}
ViewBag.pageSize_text = AnbarSearch.pageSize;
ViewBag.pageSize = AnbarSearch.pageSize;
ViewBag.sortOrder_text = AnbarSearch.sortOrder;
ViewBag.sortOrder = AnbarSearch.sortOrder;
return View(data);
}
public ActionResult Delete(int? id)
{
if (id == null)
{
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
AnbarSearchModel AnbarSearch =  new AnbarSearchModel();
AnbarSearch.AnbarIdFrom = id.Value;
AnbarSearch.AnbarIdTo = id.Value;
var Anbars = Find(AnbarSearch).FirstOrDefault();
if (Anbars == null)
{
return HttpNotFound();
}
ViewBag.AnbarId = Anbars.AnbarId;
ViewBag.TitleAnbar = Anbars.TitleAnbar;
return View(Anbars);
}
[HttpPost, ActionName("Delete")]
public ActionResult DeleteConfirmed(int id)
{
Anbar Entity = db.Anbar.Find(id);
db.Anbar.Remove(Entity);
db.SaveChanges();
return RedirectToAction("Index");
}
[HttpPost]
public ActionResult Edit(Anbar Entity)
{
db.Entry(Entity).State = EntityState.Modified;
db.SaveChanges();
return RedirectToAction("Index");
}
public ActionResult Edit(int? id)
{
if (id == null)
{
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
AnbarSearchModel AnbarSearch =  new AnbarSearchModel();
AnbarSearch.AnbarIdFrom = id.Value;
AnbarSearch.AnbarIdTo = id.Value;
var Anbars = Find(AnbarSearch).FirstOrDefault();;
if (Anbars == null)
{
return HttpNotFound();
}
ViewBag.AnbarId = Anbars.AnbarId;
ViewBag.TitleAnbar = Anbars.TitleAnbar;
return View(Anbars);
}
public ActionResult Create()
{
return View();
}
[HttpPost]
public ActionResult Create(Anbar Entity)
{
db.Anbar.Add(Entity);
db.SaveChanges();
return RedirectToAction("Index");
}
public ActionResult Create_modal(int? id)
{
    if (id == null)
    {
        return View();
    }
AnbarSearchModel AnbarSearch = new AnbarSearchModel();
AnbarSearch.AnbarIdFrom = id.Value;
AnbarSearch.AnbarIdTo = id.Value;
var Anbars = Find(AnbarSearch).FirstOrDefault();
if (Anbars == null)
    {
        return HttpNotFound();
    }
return View(Anbars);
}
[HttpPost]
public int Create_modal(Anbar Entity)
{
db.Anbar.Add(Entity);
db.SaveChanges();
return Entity.AnbarId ;
}
public ActionResult Details(int? id)
{
if (id == null)
{
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
AnbarSearchModel AnbarSearch =  new AnbarSearchModel();
AnbarSearch.AnbarIdFrom = id.Value;
AnbarSearch.AnbarIdTo = id.Value;
var Anbars = Find(AnbarSearch).FirstOrDefault();
if (Anbars == null)
{
return HttpNotFound();
}
ViewBag.AnbarId = Anbars.AnbarId;
ViewBag.TitleAnbar = Anbars.TitleAnbar;
return View(Anbars);
}
[HttpPost]
public JsonResult Anbars(string Text)
{
    var Anbars = db.Anbar.AsQueryable();
    if (!string.IsNullOrEmpty(Text))
    {
Anbars = Anbars.Where(x => x.TitleAnbar.Contains(Text));
    }

var data = Anbars.Select(x => new Utility.DropDownListAjax()
    {
Value = x.AnbarId,
Text = "شناسه انبار" + " : " +  SqlFunctions.StringConvert((double)x.AnbarId).Trim() + "-"  + "عنوان انبار" + " : " + x.TitleAnbar + "-" 

}).Distinct().ToList();

    var j = Json(new
    {
        data
    }, JsonRequestBehavior.AllowGet);
    return j;
}
private IQueryable<Anbar> sort(IQueryable<Anbar> Anbardata, string[] arr)
{
if ("شناسه انبار".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Anbardata = Anbardata.OrderBy(x => x.AnbarId);
}
if (arr[1].Equals("نزولی"))
{
Anbardata = Anbardata.OrderByDescending(x => x.AnbarId);
}
}
if ("عنوان انبار".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Anbardata = Anbardata.OrderBy(x => x.TitleAnbar);
}
if (arr[1].Equals("نزولی"))
{
Anbardata = Anbardata.OrderByDescending(x => x.TitleAnbar);
}
}
return Anbardata;
}
}
}
