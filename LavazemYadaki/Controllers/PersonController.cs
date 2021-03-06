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
public class PersonController : Controller
{
private LavazemYadakiEntities db = new LavazemYadakiEntities();
public IQueryable<Person> Find(PersonSearchModel PersonSearch)
{
var Persons = db.Person.AsQueryable<Person>();
if (PersonSearch.PersonIdFrom != 0)
{
Persons = Persons.Where(x => x.PersonId >= PersonSearch.PersonIdFrom);
ViewBag.PersonIdFrom = PersonSearch.PersonIdFrom;
}
if (PersonSearch.PersonIdTo != 0)
{
Persons = Persons.Where(x => x.PersonId <= PersonSearch.PersonIdTo);
ViewBag.PersonIdTo = PersonSearch.PersonIdTo;
}
if (!string.IsNullOrEmpty(PersonSearch.Name))
{
Persons = Persons.Where(x => x.Name.ToLower().Trim().Contains(PersonSearch.Name.ToLower().Trim()));
ViewBag.Name = PersonSearch.Name;
}
if (!string.IsNullOrEmpty(PersonSearch.Family))
{
Persons = Persons.Where(x => x.Family.ToLower().Trim().Contains(PersonSearch.Family.ToLower().Trim()));
ViewBag.Family = PersonSearch.Family;
}
return Persons;
}
[HttpPost]
public ActionResult Index(PersonSearchModel PersonSearch)
{
if (PersonSearch.sortOrder == null)
PersonSearch.sortOrder = "شناسه افراد-نزولی";
PersonSearch.sortOrder = PersonSearch.sortOrder.Replace("*", " ");
var Persondata = Find(PersonSearch);
string a = PersonSearch.sortOrder;
char[] c = { '-' };
string[] arr = a.Split(c);
var PersonSorted = sort(Persondata, arr);
var data = PersonSorted.ToPagedList(PersonSearch.page, PersonSearch.pageSize);
foreach (var item in data)
{
}
ViewBag.pageSize_text = PersonSearch.pageSize;
ViewBag.pageSize = PersonSearch.pageSize;
ViewBag.sortOrder_text = PersonSearch.sortOrder;
ViewBag.sortOrder = PersonSearch.sortOrder;
return View(data);
}
public ActionResult Index(PersonSearchModel PersonSearch, bool? temp)
{
if (PersonSearch.sortOrder == null)
PersonSearch.sortOrder = "شناسه افراد-نزولی";
PersonSearch.sortOrder = PersonSearch.sortOrder.Replace("*", " ");
var Persondata = Find(PersonSearch);
string a = PersonSearch.sortOrder;
char[] c = { '-' };
string[] arr = a.Split(c);
var PersonSorted = sort(Persondata, arr);
var data = PersonSorted.ToPagedList(PersonSearch.page, PersonSearch.pageSize);
foreach (var item in data)
{
}
ViewBag.pageSize_text = PersonSearch.pageSize;
ViewBag.pageSize = PersonSearch.pageSize;
ViewBag.sortOrder_text = PersonSearch.sortOrder;
ViewBag.sortOrder = PersonSearch.sortOrder;
return View(data);
}
public ActionResult Delete(int? id)
{
if (id == null)
{
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
PersonSearchModel PersonSearch =  new PersonSearchModel();
PersonSearch.PersonIdFrom = id.Value;
PersonSearch.PersonIdTo = id.Value;
var Persons = Find(PersonSearch).FirstOrDefault();
if (Persons == null)
{
return HttpNotFound();
}
ViewBag.PersonId = Persons.PersonId;
ViewBag.Name = Persons.Name;
ViewBag.Family = Persons.Family;
return View(Persons);
}
[HttpPost, ActionName("Delete")]
public ActionResult DeleteConfirmed(int id)
{
Person Entity = db.Person.Find(id);
db.Person.Remove(Entity);
db.SaveChanges();
return RedirectToAction("Index");
}
[HttpPost]
public ActionResult Edit(Person Entity)
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
PersonSearchModel PersonSearch =  new PersonSearchModel();
PersonSearch.PersonIdFrom = id.Value;
PersonSearch.PersonIdTo = id.Value;
var Persons = Find(PersonSearch).FirstOrDefault();;
if (Persons == null)
{
return HttpNotFound();
}
ViewBag.PersonId = Persons.PersonId;
ViewBag.Name = Persons.Name;
ViewBag.Family = Persons.Family;
return View(Persons);
}
public ActionResult Create()
{
return View();
}
[HttpPost]
public ActionResult Create(Person Entity)
{
db.Person.Add(Entity);
db.SaveChanges();
return RedirectToAction("Index");
}
public ActionResult Create_modal(int? id)
{
    if (id == null)
    {
        return View();
    }
PersonSearchModel PersonSearch = new PersonSearchModel();
PersonSearch.PersonIdFrom = id.Value;
PersonSearch.PersonIdTo = id.Value;
var Persons = Find(PersonSearch).FirstOrDefault();
if (Persons == null)
    {
        return HttpNotFound();
    }
return View(Persons);
}
[HttpPost]
public int Create_modal(Person Entity)
{
db.Person.Add(Entity);
db.SaveChanges();
return Entity.PersonId ;
}
public ActionResult Details(int? id)
{
if (id == null)
{
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
PersonSearchModel PersonSearch =  new PersonSearchModel();
PersonSearch.PersonIdFrom = id.Value;
PersonSearch.PersonIdTo = id.Value;
var Persons = Find(PersonSearch).FirstOrDefault();
if (Persons == null)
{
return HttpNotFound();
}
ViewBag.PersonId = Persons.PersonId;
ViewBag.Name = Persons.Name;
ViewBag.Family = Persons.Family;
return View(Persons);
}
[HttpPost]
public JsonResult Persons(string Text)
{
    var Persons = db.Person.AsQueryable();
    if (!string.IsNullOrEmpty(Text))
    {
Persons = Persons.Where(x => x.Name.Contains(Text));
    }

var data = Persons.Select(x => new Utility.DropDownListAjax()
    {
Value = x.PersonId,
Text = "شناسه افراد" + " : " +  SqlFunctions.StringConvert((double)x.PersonId).Trim() + "-"  + "نام" + " : " + x.Name + "-"  + "نام خانوادگی" + " : " + x.Family + "-" 

}).Distinct().ToList();

    var j = Json(new
    {
        data
    }, JsonRequestBehavior.AllowGet);
    return j;
}
private IQueryable<Person> sort(IQueryable<Person> Persondata, string[] arr)
{
if ("شناسه افراد".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Persondata = Persondata.OrderBy(x => x.PersonId);
}
if (arr[1].Equals("نزولی"))
{
Persondata = Persondata.OrderByDescending(x => x.PersonId);
}
}
if ("نام".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Persondata = Persondata.OrderBy(x => x.Name);
}
if (arr[1].Equals("نزولی"))
{
Persondata = Persondata.OrderByDescending(x => x.Name);
}
}
if ("نام خانوادگی".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
{
if (arr[1].Equals("صعودی"))
{
Persondata = Persondata.OrderBy(x => x.Family);
}
if (arr[1].Equals("نزولی"))
{
Persondata = Persondata.OrderByDescending(x => x.Family);
}
}
return Persondata;
}
}
}
