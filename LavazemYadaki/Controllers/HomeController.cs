using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LavazemYadaki.Models;
using PagedList;
namespace LavazemYadaki.Controllers
{
public class HomeController : Controller
{
public ActionResult Index()
{
return View();
}
}
}
