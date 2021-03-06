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
public class FactorForooshController : Controller
{
private LavazemYadakiEntities db = new LavazemYadakiEntities();
public IQueryable<FactorForoosh> Find(FactorForooshSearchModel FactorForooshSearch)
{
var FactorForooshs = db.FactorForoosh.AsQueryable<FactorForoosh>();
if (FactorForooshSearch.FactorForooshIdFrom != 0)
{
FactorForooshs = FactorForooshs.Where(x => x.FactorForooshId >= FactorForooshSearch.FactorForooshIdFrom);
ViewBag.FactorForooshIdFrom = FactorForooshSearch.FactorForooshIdFrom;
}
if (FactorForooshSearch.FactorForooshIdTo != 0)
{
FactorForooshs = FactorForooshs.Where(x => x.FactorForooshId <= FactorForooshSearch.FactorForooshIdTo);
ViewBag.FactorForooshIdTo = FactorForooshSearch.FactorForooshIdTo;
}
if (FactorForooshSearch.KalaIdFrom != 0)
{
FactorForooshs = FactorForooshs.Where(x => x.KalaId >= FactorForooshSearch.KalaIdFrom);
ViewBag.KalaIdFrom = FactorForooshSearch.KalaIdFrom;
}
if (FactorForooshSearch.KalaIdTo != 0)
{
FactorForooshs = FactorForooshs.Where(x => x.KalaId <= FactorForooshSearch.KalaIdTo);
ViewBag.KalaIdTo = FactorForooshSearch.KalaIdTo;
}
if (FactorForooshSearch.CountKalaFrom != 0)
{
FactorForooshs = FactorForooshs.Where(x => x.CountKala >= FactorForooshSearch.CountKalaFrom);
ViewBag.CountKalaFrom = FactorForooshSearch.CountKalaFrom;
}
if (FactorForooshSearch.CountKalaTo != 0)
{
FactorForooshs = FactorForooshs.Where(x => x.CountKala <= FactorForooshSearch.CountKalaTo);
ViewBag.CountKalaTo = FactorForooshSearch.CountKalaTo;
}
if (FactorForooshSearch.FiFrom != 0)
{
FactorForooshs = FactorForooshs.Where(x => x.Fi >= FactorForooshSearch.FiFrom);
ViewBag.FiFrom = FactorForooshSearch.FiFrom;
}
if (FactorForooshSearch.FiTo != 0)
{
FactorForooshs = FactorForooshs.Where(x => x.Fi <= FactorForooshSearch.FiTo);
ViewBag.FiTo = FactorForooshSearch.FiTo;
}
return FactorForooshs;
}
[HttpPost]
public ActionResult Index(FactorForooshSearchModel FactorForooshSearch)
{
if (FactorForooshSearch.sortOrder == null)
FactorForooshSearch.sortOrder = "شناسه فاکتور فروش-نزولی";
FactorForooshSearch.sortOrder = FactorForooshSearch.sortOrder.Replace("*", " ");
var FactorForooshdata = Find(FactorForooshSearch);
string a = FactorForooshSearch.sortOrder;
char[] c = { '-' };
string[] arr = a.Split(c);
var FactorForooshSorted = sort(FactorForooshdata, arr);
var data = FactorForooshSorted.ToPagedList(FactorForooshSearch.page, FactorForooshSearch.pageSize);
foreach (var item in data)
{
}
ViewBag.pageSize_text = FactorForooshSearch.pageSize;
ViewBag.pageSize = FactorForooshSearch.pageSize;
ViewBag.sortOrder_text = FactorForooshSearch.sortOrder;
ViewBag.sortOrder = FactorForooshSearch.sortOrder;
return View(data);
}
public ActionResult Index(FactorForooshSearchModel FactorForooshSearch, bool? temp)
{
if (FactorForooshSearch.sortOrder == null)
FactorForooshSearch.sortOrder = "شناسه فاکتور فروش-نزولی";
FactorForooshSearch.sortOrder = FactorForooshSearch.sortOrder.Replace("*", " ");
var FactorForooshdata = Find(FactorForooshSearch);
string a = FactorForooshSearch.sortOrder;
char[] c = { '-' };
string[] arr = a.Split(c);
var FactorForooshSorted = sort(FactorForooshdata, arr);
var data = FactorForooshSorted.ToPagedList(FactorForooshSearch.page, FactorForooshSearch.pageSize);
foreach (var item in data)
{
}
ViewBag.pageSize_text = FactorForooshSearch.pageSize;
ViewBag.pageSize = FactorForooshSearch.pageSize;
ViewBag.sortOrder_text = FactorForooshSearch.sortOrder;
ViewBag.sortOrder = FactorForooshSearch.sortOrder;
return View(data);
}
public ActionResult Delete(int? id)
{
if (id == null)
{
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
FactorForooshSearchModel FactorForooshSearch =  new FactorForooshSearchModel();
FactorForooshSearch.FactorForooshIdFrom = id.Value;
FactorForooshSearch.FactorForooshIdTo = id.Value;
var FactorForooshs = Find(FactorForooshSearch).FirstOrDefault();
if (FactorForooshs == null)
{
return HttpNotFound();
}
ViewBag.FactorForooshId = FactorForooshs.FactorForooshId;
ViewBag.KalaId = FactorForooshs.KalaId;
ViewBag.CountKala = FactorForooshs.CountKala;
ViewBag.Fi = FactorForooshs.Fi;
return View(FactorForooshs);
}
[HttpPost, ActionName("Delete")]
public ActionResult DeleteConfirmed(int id)
{
FactorForoosh Entity = db.FactorForoosh.Find(id);
db.FactorForoosh.Remove(Entity);
db.SaveChanges();
return RedirectToAction("Index");
}
[HttpPost]
public ActionResult Edit(FactorForoosh Entity)
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
FactorForooshSearchModel FactorForooshSearch =  new FactorForooshSearchModel();
FactorForooshSearch.FactorForooshIdFrom = id.Value;
FactorForooshSearch.FactorForooshIdTo = id.Value;
var FactorForooshs = Find(FactorForooshSearch).FirstOrDefault();;
if (FactorForooshs == null)
{
return HttpNotFound();
}
ViewBag.FactorForooshId = FactorForooshs.FactorForooshId;
ViewBag.KalaId = FactorForooshs.KalaId;
ViewBag.CountKala = FactorForooshs.CountKala;
ViewBag.Fi = FactorForooshs.Fi;
return View(FactorForooshs);
}
public ActionResult Create()
{
return View();
}
[HttpPost]
public ActionResult Create(FactorForoosh Entity)
{
db.FactorForoosh.Add(Entity);
db.SaveChanges();
return RedirectToAction("Index");
}
public ActionResult Create_modal(int? id)
{
    if (id == null)
    {
        return View();
    }
FactorForooshSearchModel FactorForooshSearch = new FactorForooshSearchModel();
FactorForooshSearch.FactorForooshIdFrom = id.Value;
FactorForooshSearch.FactorForooshIdTo = id.Value;
var FactorForooshs = Find(FactorForooshSearch).FirstOrDefault();
if (FactorForooshs == null)
    {
        return HttpNotFound();
    }
return View(FactorForooshs);
}
[HttpPost]
public int Create_modal(FactorForoosh Entity)
{
db.FactorForoosh.Add(Entity);
db.SaveChanges();
return Entity.FactorForooshId ;
}
public ActionResult Details(int? id)
{
if (id == null)
{
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
FactorForooshSearchModel FactorForooshSearch =  new FactorForooshSearchModel();
FactorForooshSearch.FactorForooshIdFrom = id.Value;
FactorForooshSearch.FactorForooshIdTo = id.Value;
var FactorForooshs = Find(FactorForooshSearch).FirstOrDefault();
if (FactorForooshs == null)
{
return HttpNotFound();
}
ViewBag.FactorForooshId = FactorForooshs.FactorForooshId;
ViewBag.KalaId = FactorForooshs.KalaId;
ViewBag.CountKala = FactorForooshs.CountKala;
ViewBag.Fi = FactorForooshs.Fi;
return View(FactorForooshs);
}
[HttpPost]
public JsonResult FactorForooshs(string Text)
{
    var FactorForooshs = db.FactorForoosh.AsQueryable();
    if (!string.IsNullOrEmpty(Text))
    {
    }

var data = FactorForooshs.Select(x => new Utility.DropDownListAjax()
    {
Value = x.FactorForooshId,
Text = "شناسه فاکتور فروش" + " : " +  SqlFunctions.StringConvert((double)x.FactorForooshId).Trim() + "-"  + "شناسه کالا" + " : " +  SqlFunctions.StringConvert((double)x.KalaId).Trim() + "-"  + "عنوان کالا" + " : " + x.Kala.TitleKala + "-"  + "کد کالا" + " : " + x.Kala.CodeKala + "-"  + "نقطه سفارش" + " : " +  (x.Kala.OrderPoint.HasValue ? SqlFunctions.StringConvert((double) x.Kala.OrderPoint).Trim() : "مشخص نشده است") + "-"  + "شناسه انبار" + " : " +  SqlFunctions.StringConvert((double)x.Kala.AnbarId).Trim() + "-"  + "تعداد" + " : " +  SqlFunctions.StringConvert((double)x.CountKala).Trim() + "-"  + "قیمت پایه" + " : " +  SqlFunctions.StringConvert((double)x.Fi).Trim() + "-" 

}).Distinct().ToList();

    var j = Json(new
    {
        data
    }, JsonRequestBehavior.AllowGet);
    return j;
}
private IQueryable<FactorForoosh> sort(IQueryable<FactorForoosh> FactorForooshdata, string[] arr)
{
if ("شناسه فاکتور فروش".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
FactorForooshdata = FactorForooshdata.OrderBy(x => x.FactorForooshId);
}
if (arr[1].Equals("نزولی"))
{
FactorForooshdata = FactorForooshdata.OrderByDescending(x => x.FactorForooshId);
}
}
if ("شناسه کالا".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
FactorForooshdata = FactorForooshdata.OrderBy(x => x.KalaId);
}
if (arr[1].Equals("نزولی"))
{
FactorForooshdata = FactorForooshdata.OrderByDescending(x => x.KalaId);
}
}
if ("تعداد".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
FactorForooshdata = FactorForooshdata.OrderBy(x => x.CountKala);
}
if (arr[1].Equals("نزولی"))
{
FactorForooshdata = FactorForooshdata.OrderByDescending(x => x.CountKala);
}
}
if ("قیمت پایه".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
FactorForooshdata = FactorForooshdata.OrderBy(x => x.Fi);
}
if (arr[1].Equals("نزولی"))
{
FactorForooshdata = FactorForooshdata.OrderByDescending(x => x.Fi);
}
}
return FactorForooshdata;
}
}
}
