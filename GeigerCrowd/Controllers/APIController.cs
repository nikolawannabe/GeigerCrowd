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
    public class APIController : Controller
    {
        private GeigerCrowdContext context = new GeigerCrowdContext();
        //
        // GET: /API/

        public JsonResult getAllReadings()
        {
            List<ReadingPoint> readingPoints = context.ReadingPoints.ToList();
            return Json(readingPoints);
        }

    }
}
