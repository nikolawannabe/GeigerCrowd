using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeigerCrowd.Models;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace GeigerCrowd.Controllers
{
    public class APIController : Controller
    {
        private GeigerCrowdContext context = new GeigerCrowdContext();
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();
        //
        // GET: /API/

        public JsonResult getAllReadings()
        {
            List<ReadingPoint> readingPoints = context.ReadingPoints.ToList();
            return Json(readingPoints,JsonRequestBehavior.AllowGet);
        }
    }
}
