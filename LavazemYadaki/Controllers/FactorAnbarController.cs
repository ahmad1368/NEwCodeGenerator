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
    public class FactorAnbarController : Controller
    {
        private LavazemYadakiEntities db = new LavazemYadakiEntities();
        public IQueryable<FactorAnbar> Find(FactorAnbarSearchModel FactorAnbarSearch)
        {
            var FactorAnbars = db.FactorAnbar.AsQueryable<FactorAnbar>();
            if (FactorAnbarSearch.FactorAnbarIdFrom != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.FactorAnbarId >= FactorAnbarSearch.FactorAnbarIdFrom);
                ViewBag.FactorAnbarIdFrom = FactorAnbarSearch.FactorAnbarIdFrom;
            }
            if (FactorAnbarSearch.FactorAnbarIdTo != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.FactorAnbarId <= FactorAnbarSearch.FactorAnbarIdTo);
                ViewBag.FactorAnbarIdTo = FactorAnbarSearch.FactorAnbarIdTo;
            }
            if (FactorAnbarSearch.KalaIdFrom != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.KalaId >= FactorAnbarSearch.KalaIdFrom);
                ViewBag.KalaIdFrom = FactorAnbarSearch.KalaIdFrom;
            }
            if (FactorAnbarSearch.KalaIdTo != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.KalaId <= FactorAnbarSearch.KalaIdTo);
                ViewBag.KalaIdTo = FactorAnbarSearch.KalaIdTo;
            }
            if (FactorAnbarSearch.BestankarFrom != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.Bestankar >= FactorAnbarSearch.BestankarFrom);
                ViewBag.BestankarFrom = FactorAnbarSearch.BestankarFrom;
            }
            if (FactorAnbarSearch.BestankarTo != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.Bestankar <= FactorAnbarSearch.BestankarTo);
                ViewBag.BestankarTo = FactorAnbarSearch.BestankarTo;
            }
            if (FactorAnbarSearch.BedehkarFrom != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.Bedehkar >= FactorAnbarSearch.BedehkarFrom);
                ViewBag.BedehkarFrom = FactorAnbarSearch.BedehkarFrom;
            }
            if (FactorAnbarSearch.BedehkarTo != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.Bedehkar <= FactorAnbarSearch.BedehkarTo);
                ViewBag.BedehkarTo = FactorAnbarSearch.BedehkarTo;
            }
            if (FactorAnbarSearch.TarikhFrom.HasValue)
            {
                var DateFrom = FactorAnbarSearch.TarikhFrom.Value;
                DateFrom = Utility.ConvertToGregorianDate(string.Format("{0:D4}/{1:D2}/{2:D2}", DateFrom.Year, DateFrom.Month, DateFrom.Day));
                FactorAnbars = FactorAnbars.Where(x => x.Tarikh >= DateFrom);
                ViewBag.TarikhFrom = Utility.PersianDate(DateFrom);
            }
            if (FactorAnbarSearch.TarikhTo.HasValue)
            {
                var DateTo = FactorAnbarSearch.TarikhTo.Value;
                DateTo = Utility.ConvertToGregorianDate(string.Format("{0:D4}/{1:D2}/{2:D2}", DateTo.Year, DateTo.Month, DateTo.Day));
                FactorAnbars = FactorAnbars.Where(x => x.Tarikh <= DateTo);
                ViewBag.TarikhTo = Utility.PersianDate(DateTo);
            }
            if (FactorAnbarSearch.PersonIdFrom != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.PersonId >= FactorAnbarSearch.PersonIdFrom);
                ViewBag.PersonIdFrom = FactorAnbarSearch.PersonIdFrom;
            }
            if (FactorAnbarSearch.PersonIdTo != 0)
            {
                FactorAnbars = FactorAnbars.Where(x => x.PersonId <= FactorAnbarSearch.PersonIdTo);
                ViewBag.PersonIdTo = FactorAnbarSearch.PersonIdTo;
            }
            return FactorAnbars;
        }
        [HttpPost]
        public ActionResult Index(FactorAnbarSearchModel FactorAnbarSearch)
        {
            if (FactorAnbarSearch.sortOrder == null)
                FactorAnbarSearch.sortOrder = "شناسه فاکتور انبار-نزولی";
            FactorAnbarSearch.sortOrder = FactorAnbarSearch.sortOrder.Replace("*", " ");
            var FactorAnbardata = Find(FactorAnbarSearch);
            string a = FactorAnbarSearch.sortOrder;
            char[] c = { '-' };
            string[] arr = a.Split(c);
            var FactorAnbarSorted = sort(FactorAnbardata, arr);
            var data = FactorAnbarSorted.ToPagedList(FactorAnbarSearch.page, FactorAnbarSearch.pageSize);
            foreach (var item in data)
            {
                item.Tarikh = item.Tarikh.HasValue ? Utility.ConvertPersionDateStringToPersionDateDateToime(Utility.PersianDate(item.Tarikh)) : null;
            }
            ViewBag.pageSize_text = FactorAnbarSearch.pageSize;
            ViewBag.pageSize = FactorAnbarSearch.pageSize;
            ViewBag.sortOrder_text = FactorAnbarSearch.sortOrder;
            ViewBag.sortOrder = FactorAnbarSearch.sortOrder;
            return View(data);
        }
        public ActionResult Index(FactorAnbarSearchModel FactorAnbarSearch, bool? temp)
        {
            if (FactorAnbarSearch.sortOrder == null)
                FactorAnbarSearch.sortOrder = "شناسه فاکتور انبار-نزولی";
            FactorAnbarSearch.sortOrder = FactorAnbarSearch.sortOrder.Replace("*", " ");
            var FactorAnbardata = Find(FactorAnbarSearch);
            string a = FactorAnbarSearch.sortOrder;
            char[] c = { '-' };
            string[] arr = a.Split(c);
            var FactorAnbarSorted = sort(FactorAnbardata, arr);
            var data = FactorAnbarSorted.ToPagedList(FactorAnbarSearch.page, FactorAnbarSearch.pageSize);
            foreach (var item in data)
            {
                item.Tarikh = item.Tarikh.HasValue ? Utility.ConvertPersionDateStringToPersionDateDateToime(Utility.PersianDate(item.Tarikh)) : null;
            }
            ViewBag.pageSize_text = FactorAnbarSearch.pageSize;
            ViewBag.pageSize = FactorAnbarSearch.pageSize;
            ViewBag.sortOrder_text = FactorAnbarSearch.sortOrder;
            ViewBag.sortOrder = FactorAnbarSearch.sortOrder;
            return View(data);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactorAnbarSearchModel FactorAnbarSearch = new FactorAnbarSearchModel();
            FactorAnbarSearch.FactorAnbarIdFrom = id.Value;
            FactorAnbarSearch.FactorAnbarIdTo = id.Value;
            var FactorAnbars = Find(FactorAnbarSearch).FirstOrDefault();
            if (FactorAnbars == null)
            {
                return HttpNotFound();
            }
            ViewBag.FactorAnbarId = FactorAnbars.FactorAnbarId;
            ViewBag.KalaId = FactorAnbars.KalaId;
            ViewBag.Bestankar = FactorAnbars.Bestankar;
            ViewBag.Bedehkar = FactorAnbars.Bedehkar;
            ViewBag.Tarikh = FactorAnbars.Tarikh.HasValue ? Utility.PersianDate(FactorAnbars.Tarikh) : "";
            ViewBag.PersonId = FactorAnbars.PersonId;
            return View(FactorAnbars);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FactorAnbar Entity = db.FactorAnbar.Find(id);
            db.FactorAnbar.Remove(Entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(FactorAnbar Entity)
        {
            if (Entity.Tarikh.HasValue)
                Entity.Tarikh = Utility.ConvertToGregorianDate(string.Format("{0:D4}/{1:D2}/{2:D2}", Entity.Tarikh.Value.Year, Entity.Tarikh.Value.Month, Entity.Tarikh.Value.Day));
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
            FactorAnbarSearchModel FactorAnbarSearch = new FactorAnbarSearchModel();
            FactorAnbarSearch.FactorAnbarIdFrom = id.Value;
            FactorAnbarSearch.FactorAnbarIdTo = id.Value;
            var FactorAnbars = Find(FactorAnbarSearch).FirstOrDefault(); ;
            if (FactorAnbars == null)
            {
                return HttpNotFound();
            }
            ViewBag.FactorAnbarId = FactorAnbars.FactorAnbarId;
            ViewBag.KalaId = FactorAnbars.KalaId;
            ViewBag.Bestankar = FactorAnbars.Bestankar;
            ViewBag.Bedehkar = FactorAnbars.Bedehkar;
            ViewBag.Tarikh = FactorAnbars.Tarikh.HasValue ? Utility.PersianDate(FactorAnbars.Tarikh) : "";
            ViewBag.PersonId = FactorAnbars.PersonId;
            return View(FactorAnbars);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FactorAnbar Entity)
        {
            if (Entity.Tarikh.HasValue)
                Entity.Tarikh = Utility.ConvertToGregorianDate(string.Format("{0:D4}/{1:D2}/{2:D2}", Entity.Tarikh.Value.Year, Entity.Tarikh.Value.Month, Entity.Tarikh.Value.Day));
            db.FactorAnbar.Add(Entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Create_modal(int? id)
        {
            if (id == null)
            {
                return View();
            }
            FactorAnbarSearchModel FactorAnbarSearch = new FactorAnbarSearchModel();
            FactorAnbarSearch.FactorAnbarIdFrom = id.Value;
            FactorAnbarSearch.FactorAnbarIdTo = id.Value;
            var FactorAnbars = Find(FactorAnbarSearch).FirstOrDefault();
            if (FactorAnbars == null)
            {
                return HttpNotFound();
            }
            return View(FactorAnbars);
        }
        [HttpPost]
        public int Create_modal(FactorAnbar Entity)
        {
            if (Entity.Tarikh.HasValue)
                Entity.Tarikh = Utility.ConvertToGregorianDate(string.Format("{0:D4}/{1:D2}/{2:D2}", Entity.Tarikh.Value.Year, Entity.Tarikh.Value.Month, Entity.Tarikh.Value.Day));
            db.FactorAnbar.Add(Entity);
            db.SaveChanges();
            return Entity.FactorAnbarId;
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactorAnbarSearchModel FactorAnbarSearch = new FactorAnbarSearchModel();
            FactorAnbarSearch.FactorAnbarIdFrom = id.Value;
            FactorAnbarSearch.FactorAnbarIdTo = id.Value;
            var FactorAnbars = Find(FactorAnbarSearch).FirstOrDefault();
            if (FactorAnbars == null)
            {
                return HttpNotFound();
            }
            ViewBag.FactorAnbarId = FactorAnbars.FactorAnbarId;
            ViewBag.KalaId = FactorAnbars.KalaId;
            ViewBag.Bestankar = FactorAnbars.Bestankar;
            ViewBag.Bedehkar = FactorAnbars.Bedehkar;
            ViewBag.Tarikh = FactorAnbars.Tarikh.HasValue ? Utility.PersianDate(FactorAnbars.Tarikh) : "";
            ViewBag.PersonId = FactorAnbars.PersonId;
            return View(FactorAnbars);
        }
        [HttpPost]
        public JsonResult FactorAnbars(string Text)
        {
            var FactorAnbars = db.FactorAnbar.AsQueryable();
            if (!string.IsNullOrEmpty(Text))
            {
            }

            var data = FactorAnbars.Select(x => new Utility.DropDownListAjax()
            {
                Value = x.FactorAnbarId,
                Text = "شناسه فاکتور انبار" + " : " + SqlFunctions.StringConvert((double)x.FactorAnbarId).Trim() + "-" + "شناسه کالا" + " : " + SqlFunctions.StringConvert((double)x.KalaId).Trim() + "-" + "عنوان کالا" + " : " + x.Kala.TitleKala + "-" + "کد کالا" + " : " + x.Kala.CodeKala + "-" + "نقطه سفارش" + " : " + (x.Kala.OrderPoint.HasValue ? SqlFunctions.StringConvert((double)x.Kala.OrderPoint).Trim() : "مشخص نشده است") + "-" + "شناسه انبار" + " : " + SqlFunctions.StringConvert((double)x.Kala.AnbarId).Trim() + "-" + "بستانکار" + " : " + (x.Bestankar.HasValue ? SqlFunctions.StringConvert((double)x.Bestankar).Trim() : "مشخص نشده است") + "-" + "بدهکار" + " : " + (x.Bedehkar.HasValue ? SqlFunctions.StringConvert((double)x.Bedehkar).Trim() : "مشخص نشده است") + "-" + "شناسه فرد تحویل دهنده یا تحویل گیرنده" + " : " + SqlFunctions.StringConvert((double)x.PersonId).Trim() + "-" + "نام" + " : " + x.Person.Name + "-" + "نام خانوادگی" + " : " + x.Person.Family + "-"

            }).Distinct().ToList();
            foreach (var item in FactorAnbars)
            {
                var itemData = data.Where(x => x.Value == item.FactorAnbarId).FirstOrDefault();
                if (itemData != null)
                    itemData.Text += "تاریخ ورود یا خروج" + " : " + (item.Tarikh.HasValue ? Utility.PersianDate(item.Tarikh) : "مشخص نشده است");
            }


            var j = Json(new
            {
                data
            }, JsonRequestBehavior.AllowGet);
            return j;
        }
        private IQueryable<FactorAnbar> sort(IQueryable<FactorAnbar> FactorAnbardata, string[] arr)
        {
            if ("شناسه فاکتور انبار".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
            {
                if (arr[1].Equals("صعودی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderBy(x => x.FactorAnbarId);
                }
                if (arr[1].Equals("نزولی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderByDescending(x => x.FactorAnbarId);
                }
            }
            if ("شناسه کالا".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
            {
                if (arr[1].Equals("صعودی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderBy(x => x.KalaId);
                }
                if (arr[1].Equals("نزولی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderByDescending(x => x.KalaId);
                }
            }
            if ("بستانکار".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
            {
                if (arr[1].Equals("صعودی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderBy(x => x.Bestankar);
                }
                if (arr[1].Equals("نزولی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderByDescending(x => x.Bestankar);
                }
            }
            if ("بدهکار".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
            {
                if (arr[1].Equals("صعودی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderBy(x => x.Bedehkar);
                }
                if (arr[1].Equals("نزولی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderByDescending(x => x.Bedehkar);
                }
            }
            if ("تاریخ ورود یا خروج".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
            {
                if (arr[1].Equals("صعودی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderBy(x => x.Tarikh);
                }
                if (arr[1].Equals("نزولی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderByDescending(x => x.Tarikh);
                }
            }
            if ("شناسه فرد تحویل دهنده یا تحویل گیرنده".Trim().ToLower().Equals(arr[0].Trim().ToLower()))
            {
                if (arr[1].Equals("صعودی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderBy(x => x.PersonId);
                }
                if (arr[1].Equals("نزولی"))
                {
                    FactorAnbardata = FactorAnbardata.OrderByDescending(x => x.PersonId);
                }
            }
            return FactorAnbardata;
        }
    }
}
