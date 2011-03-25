using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeigerCrowd.Models;

namespace GeigerCrowd.Controllers
{   
    public class ReadingPointsController : Controller
    {
        private GeigerCrowdContext context = new GeigerCrowdContext();

        //
        // GET: /ReadingPoint/
        [Authorize]
        public ViewResult Index()
        {
            return View(context.ReadingPoints.ToList());
        }

        //
        // GET: /ReadingPoint/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
			ReadingPoint readingpoint = context.ReadingPoints.Single(x => x.ID == id);
            return View(readingpoint);
        }

        //
        // GET: /ReadingPoint/Create
        [Authorize]
        public ActionResult Create()
        {
            @ViewBag.UserID = this.Request.LogonUserIdentity.User;
            return View();
        } 

        //
        // POST: /ReadingPoint/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(ReadingPoint readingpoint)
        {
            @ViewBag.UserID = this.Request.LogonUserIdentity.User;
            if (ModelState.IsValid)
            {
				context.ReadingPoints.Add(readingpoint);
				context.SaveChanges();
				return RedirectToAction("Index");  
            }

            return View(readingpoint);
        }
        
        //
        // GET: /ReadingPoint/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            @ViewBag.UserID = this.Request.LogonUserIdentity.User;
			ReadingPoint readingpoint = context.ReadingPoints.Single(x => x.ID == id);
			return View(readingpoint);
        }

        //
        // POST: /ReadingPoint/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(ReadingPoint readingpoint)
        {
            if (ModelState.IsValid)
            {
				context.Entry(readingpoint).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(readingpoint);
        }

        //
        // GET: /ReadingPoint/Delete/5

        [Authorize]
        public ActionResult Delete(int id)
        {
			ReadingPoint readingpoint = context.ReadingPoints.Single(x => x.ID == id);
            return View(readingpoint);
        }

        //
        // POST: /ReadingPoint/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            ReadingPoint readingpoint = context.ReadingPoints.Single(x => x.ID == id);
            context.ReadingPoints.Remove(readingpoint);
			context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}