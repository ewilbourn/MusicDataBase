using MusicWebsite.classes.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicWebsite.Controllers
{
    public class BaseController : AsyncController
    {
        protected DALService dal = new DALService();

        protected override void Dispose(bool disposing)
        {
            dal.Dispose();
            base.Dispose(disposing);
        }
    }
}