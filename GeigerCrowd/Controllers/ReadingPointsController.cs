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

        public ViewResult Index()
        {
            return View(context.ReadingPoints.ToList());
        }

        //
        // GET: /ReadingPoint/Details/5

        public ViewResult Details(int id)
        {
			ReadingPoint readingpoint = context.ReadingPoints.Single(x => x.ID == id);
            return View(readingpoint);
        }

        //
        // GET: /ReadingPoint/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ReadingPoint/Create

        [HttpPost]
        public ActionResult Create(ReadingPoint readingpoint)
        {
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
 
        public ActionResult Edit(int id)
        {
			ReadingPoint readingpoint = context.ReadingPoints.Single(x => x.ID == id);
			return View(readingpoint);
        }

        //
        // POST: /ReadingPoint/Edit/5

        [HttpPost]
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
 
        public ActionResult Delete(int id)
        {
			ReadingPoint readingpoint = context.ReadingPoints.Single(x => x.ID == id);
            return View(readingpoint);
        }

        //
        // POST: /ReadingPoint/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ReadingPoint readingpoint = context.ReadingPoints.Single(x => x.ID == id);
            context.ReadingPoints.Remove(readingpoint);
			context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}