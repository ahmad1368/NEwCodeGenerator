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
public class KalaController : Controller
{
private LavazemYadakiEntities db = new LavazemYadakiEntities();
public IQueryable<Kala> Find(KalaSearchModel KalaSearch)
{
var Kalas = db.Kala.AsQueryable<Kala>();
if (KalaSearch.KalaIdFrom != 0)
{
Kalas = Kalas.Where(x => x.KalaId >= KalaSearch.KalaIdFrom);
ViewBag.KalaIdFrom = KalaSearch.KalaIdFrom;
}
if (KalaSearch.KalaIdTo != 0)
{
Kalas = Kalas.Where(x => x.KalaId <= KalaSearch.KalaIdTo);
ViewBag.KalaIdTo = KalaSearch.KalaIdTo;
}
if (!string.IsNullOrEmpty(KalaSearch.TitleKala))
{
Kalas = Kalas.Where(x => x.TitleKala.ToLower().Trim().Contains(KalaSearch.TitleKala.ToLower().Trim()));
ViewBag.TitleKala = KalaSearch.TitleKala;
}
if (!string.IsNullOrEmpty(KalaSearch.CodeKala))
{
Kalas = Kalas.Where(x => x.CodeKala.ToLower().Trim().Contains(KalaSearch.CodeKala.ToLower().Trim()));
ViewBag.CodeKala = KalaSearch.CodeKala;
}
if (KalaSearch.OrderPointFrom != 0)
{
Kalas = Kalas.Where(x => x.OrderPoint >= KalaSearch.OrderPointFrom);
ViewBag.OrderPointFrom = KalaSearch.OrderPointFrom;
}
if (KalaSearch.OrderPointTo != 0)
{
Kalas = Kalas.Where(x => x.OrderPoint <= KalaSearch.OrderPointTo);
ViewBag.OrderPointTo = KalaSearch.OrderPointTo;
}
if (KalaSearch.AnbarIdFrom != 0)
{
Kalas = Kalas.Where(x => x.AnbarId >= KalaSearch.AnbarIdFrom);
ViewBag.AnbarIdFrom = KalaSearch.AnbarIdFrom;
}
if (KalaSearch.AnbarIdTo != 0)
{
Kalas = Kalas.Where(x => x.AnbarId <= KalaSearch.AnbarIdTo);
ViewBag.AnbarIdTo = KalaSearch.AnbarIdTo;
}
return Kalas;
}
[HttpPost]
public ActionResult Index(KalaSearchModel KalaSearch)
{
if (KalaSearch.sortOrder == null)
KalaSearch.sortOrder = "شناسه کالا-نزولی";
KalaSearch.sortOrder = KalaSearch.sortOrder.Replace("*", " ");
var Kaladata = Find(KalaSearch);
string a = KalaSearch.sortOrder;
char[] c = { '-' };
string[] arr = a.Split(c);
var KalaSorted = sort(Kaladata, arr);
var data = KalaSorted.ToPagedList(KalaSearch.page, KalaSearch.pageSize);
foreach (var item in data)
{
}
ViewBag.pageSize_text = KalaSearch.pageSize;
ViewBag.pageSize = KalaSearch.pageSize;
ViewBag.sortOrder_text = KalaSearch.sortOrder;
ViewBag.sortOrder = KalaSearch.sortOrder;
return View(data);
}
public ActionResult Index(KalaSearchModel KalaSearch, bool? temp)
{
if (KalaSearch.sortOrder == null)
KalaSearch.sortOrder = "شناسه کالا-نزولی";
KalaSearch.sortOrder = KalaSearch.sortOrder.Replace("*", " ");
var Kaladata = Find(KalaSearch);
string a = KalaSearch.sortOrder;
char[] c = { '-' };
string[] arr = a.Split(c);
var KalaSorted = sort(Kaladata, arr);
var data = KalaSorted.ToPagedList(KalaSearch.page, KalaSearch.pageSize);
foreach (var item in data)
{
}
ViewBag.pageSize_text = KalaSearch.pageSize;
ViewBag.pageSize = KalaSearch.pageSize;
ViewBag.sortOrder_text = KalaSearch.sortOrder;
ViewBag.sortOrder = KalaSearch.sortOrder;
return View(data);
}
public ActionResult Delete(int? id)
{
if (id == null)
{
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
KalaSearchModel KalaSearch =  new KalaSearchModel();
KalaSearch.KalaIdFrom = id.Value;
KalaSearch.KalaIdTo = id.Value;
var Kalas = Find(KalaSearch).FirstOrDefault();
if (Kalas == null)
{
return HttpNotFound();
}
ViewBag.KalaId = Kalas.KalaId;
ViewBag.TitleKala = Kalas.TitleKala;
ViewBag.CodeKala = Kalas.CodeKala;
ViewBag.OrderPoint = Kalas.OrderPoint;
ViewBag.AnbarId = Kalas.AnbarId;
return View(Kalas);
}
[HttpPost, ActionName("Delete")]
public ActionResult DeleteConfirmed(int id)
{
Kala Entity = db.Kala.Find(id);
db.Kala.Remove(Entity);
db.SaveChanges();
return RedirectToAction("Index");
}
[HttpPost]
public ActionResult Edit(Kala Entity)
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
KalaSearchModel KalaSearch =  new KalaSearchModel();
KalaSearch.KalaIdFrom = id.Value;
KalaSearch.KalaIdTo = id.Value;
var Kalas = Find(KalaSearch).FirstOrDefault();;
if (Kalas == null)
{
return HttpNotFound();
}
ViewBag.KalaId = Kalas.KalaId;
ViewBag.TitleKala = Kalas.TitleKala;
ViewBag.CodeKala = Kalas.CodeKala;
ViewBag.OrderPoint = Kalas.OrderPoint;
ViewBag.AnbarId = Kalas.AnbarId;
return View(Kalas);
}
public ActionResult Create()
{
return View();
}
[HttpPost]
public ActionResult Create(Kala Entity)
{
db.Kala.Add(Entity);
db.SaveChanges();
return RedirectToAction("Index");
}
public ActionResult Create_modal(int? id)
{
    if (id == null)
    {
        return View();
    }
KalaSearchModel KalaSearch = new KalaSearchModel();
KalaSearch.KalaIdFrom = id.Value;
KalaSearch.KalaIdTo = id.Value;
var Kalas = Find(KalaSearch).FirstOrDefault();
if (Kalas == null)
    {
        return HttpNotFound();
    }
return View(Kalas);
}
[HttpPost]
public int Create_modal(Kala Entity)
{
db.Kala.Add(Entity);
db.SaveChanges();
return Entity.KalaId ;
}
public ActionResult Details(int? id)
{
if (id == null)
{
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
KalaSearchModel KalaSearch =  new KalaSearchModel();
KalaSearch.KalaIdFrom = id.Value;
KalaSearch.KalaIdTo = id.Value;
var Kalas = Find(KalaSearch).FirstOrDefault();
if (Kalas == null)
{
return HttpNotFound();
}
ViewBag.KalaId = Kalas.KalaId;
ViewBag.TitleKala = Kalas.TitleKala;
ViewBag.CodeKala = Kalas.CodeKala;
ViewBag.OrderPoint = Kalas.OrderPoint;
ViewBag.AnbarId = Kalas.AnbarId;
return View(Kalas);
}
[HttpPost]
public JsonResult Kalas(string Text)
{
    var Kalas = db.Kala.AsQueryable();
    if (!string.IsNullOrEmpty(Text))
    {
Kalas = Kalas.Where(x => x.TitleKala.Contains(Text));
    }

var data = Kalas.Select(x => new Utility.DropDownListAjax()
    {
Value = x.KalaId,
Text = "شناسه کالا" + " : " +  SqlFunctions.StringConvert((double)x.KalaId).Trim() + "-"  + "عنوان کالا" + " : " + x.TitleKala + "-"  + "کد کالا" + " : " + x.CodeKala + "-"  + "نقطه سفارش" + " : " +  (x.OrderPoint.HasValue ? SqlFunctions.StringConvert((double) x.OrderPoint).Trim() : "مشخص نشده است") + "-"  + "شناسه انبار" + " : " +  SqlFunctions.StringConvert((double)x.AnbarId).Trim() + "-"  + "عنوان انبار" + " : " + x.Anbar.TitleAnbar + "-" 

}).Distinct().ToList();

    var j = Json(new
    {
        data
    }, JsonRequestBehavior.AllowGet);
    return j;
}
private IQueryable<Kala> sort(IQueryable<Kala> Kaladata, string[] arr)
{
if ("شناسه کالا".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Kaladata = Kaladata.OrderBy(x => x.KalaId);
}
if (arr[1].Equals("نزولی"))
{
Kaladata = Kaladata.OrderByDescending(x => x.KalaId);
}
}
if ("عنوان کالا".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Kaladata = Kaladata.OrderBy(x => x.TitleKala);
}
if (arr[1].Equals("نزولی"))
{
Kaladata = Kaladata.OrderByDescending(x => x.TitleKala);
}
}
if ("کد کالا".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Kaladata = Kaladata.OrderBy(x => x.CodeKala);
}
if (arr[1].Equals("نزولی"))
{
Kaladata = Kaladata.OrderByDescending(x => x.CodeKala);
}
}
if ("نقطه سفارش".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Kaladata = Kaladata.OrderBy(x => x.OrderPoint);
}
if (arr[1].Equals("نزولی"))
{
Kaladata = Kaladata.OrderByDescending(x => x.OrderPoint);
}
}
if ("شناسه انبار".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Kaladata = Kaladata.OrderBy(x => x.AnbarId);
}
if (arr[1].Equals("نزولی"))
{
Kaladata = Kaladata.OrderByDescending(x => x.AnbarId);
}
}
return Kaladata;
}
}
}
