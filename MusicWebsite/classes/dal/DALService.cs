using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Configuration;
using MusicWebsite.Models;


namespace MusicWebsite.classes.dal
{
    public class DALService : IDisposable
    {
        #region "Base Class"
        private SheetMusicEntities context;
        public SheetMusicEntities DbContext
        {
            get
            {
                if (context == null)
                    context = new SheetMusicEntities();

                return context;
            }
        }

        public bool Disposed = false;

        private HttpContext mCtx;

        public DALService(ref HttpContext ctx)
        {
            initialize();
            if (ctx != null)
                mCtx = ctx;
        }

        public DALService()
        {
            initialize();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.Disposed == false)
            {
                if (disposing)
                {
                    //try
                    //{
                    //    sh.Dispose();
                    //    sh = null;
                    //}
                    //catch (Exception ex)
                    //{
                    //    System.Diagnostics.Debug.WriteLine("Exception sh in DAL");
                    //}
                }

                try
                {
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception disposing DBContext in DAL {0}".FormatWith(ex.Message));
                }
            }
            this.Disposed = true;
        }

        private void initialize()
        {
            var connStr = ConfigurationManager.ConnectionStrings["SheetMusicEntities"].ToString();
            if (string.IsNullOrEmpty(connStr))
                throw new Exception("Missing the connectionstring in the web.config of HomerConnection.");
        }
        #endregion

        //public int getNextUserID()
        //{
        //    return DbContext.AspNetUsers.Select(s => s.Id_num).Max() + 1;
        //}

        //public string GetUserFullName(string username)
        //{
        //    return DbContext.AspNetUsers.Where(w => w.UserName == username).Select(s => new { FullName = s.FirstName + " " + s.LastName }).FirstOrDefault().FullName;
        //}

        //public string GetUserID(string username)
        //{
        //    return DbContext.AspNetUsers.Where(w => w.UserName == username).Select(s => s.Id).FirstOrDefault();
        //}
    }
}