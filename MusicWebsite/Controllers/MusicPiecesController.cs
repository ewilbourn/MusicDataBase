using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MusicWebsite.classes;
using MusicWebsite.Models;
using MusicWebsite.ViewModels;

namespace MusicWebsite.Controllers
{
    public class MusicPiecesController : BaseController
    {
        //private SheetMusicEntities db = new SheetMusicEntities();

        // GET: MusicPieces
        public ActionResult Index()
        {
            var musicPieces = dal.DbContext.MusicPieces.Include(m => m.Composer).Include(m => m.OpusNamingConvention);
            setFilters(0, 0, 0);
            return View(musicPieces.ToList());
        }

        private void setFilters(int composerDefault = 0, int yearDefault = 0, int levelDefault = 0)
        {
            List<ListItem> compItems = new List<ListItem>();
            var compList = dal.DbContext.Composers.ToList();
            compItems = compList.Select(s => new ListItem(s.FirstName + ' ' + s.LastName, s.ComposerID.ToString())).ToList();
            compItems.Insert(0, new ListItem("All", ""));
            SelectList compSelectList = new SelectList(compItems, "Value", "Text", composerDefault > 0 ? composerDefault.ToString() : null);
            ViewBag.ComposerFilter = compSelectList;

            List<ListItem> yearsItems = new List<ListItem>();
            var yearsList = dal.DbContext.MusicPieces.Where(w => w.Year != null).Select(s => new { MusicYear = (int)s.Year }).Distinct().ToList();
            yearsItems = yearsList.Select(s => new ListItem(s.MusicYear.ToString(), s.MusicYear.ToString())).ToList();
            yearsItems.Insert(0, new ListItem("All", ""));
            SelectList yearsSelectList = new SelectList(yearsItems, "Value", "Text", yearDefault > 0 ? yearDefault.ToString() : null);
            ViewBag.YearFilter = yearsSelectList;

            List<ListItem> diffItems = new List<ListItem>();
            var diffList = dal.DbContext.MusicPieces.Where(w => w.Difficulty != null).Select(s => new { MusicDifficulty = (int)s.Difficulty }).Distinct().ToList();
            diffItems = diffList.Select(s => new ListItem(s.MusicDifficulty.ToString(), s.MusicDifficulty.ToString())).ToList();
            diffItems.Insert(0, new ListItem("All", ""));
            SelectList difficultySelectList = new SelectList(diffItems, "Value", "Text", levelDefault > 0 ? levelDefault.ToString() : null);
            ViewBag.DifficultyFilter = difficultySelectList;
        }

        /// <summary>
        /// AJAX method to query the music pieces table
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public JsonResult GetPieces(MusicTableParameters p)
        {
            try
            {
                IQueryable<view_MusicPieces> raw = (from d in this.dal.DbContext.view_MusicPieces
                                           where 
                                                (p.ComposerID == 0 || d.ComposerID == p.ComposerID) &&
                                                (p.Year == 0 || p.Year == d.Year) &&
                                                (p.Difficulty == 0 || p.Difficulty == d.Difficulty) &&
                                                (p.Search.Value == null ||
                                                    (
                                                    (d.FullName.Contains(p.Search.Value)) ||
                                                    d.PieceName.Contains(p.Search.Value)
                                                    )
                                                )
                                           select d)
               .AsQueryable();

                switch (p.Columns[p.Order[0].Column].Data)
                {
                    case "LastName":
                        if (p.Order[0].Dir == DataTableViewModel.DataTablesOrderDir.DESC)
                            raw = raw.OrderByDescending(o => o.LastName);
                        else
                            raw = raw.OrderBy(o => o.LastName);
                        break;
                    case "FirstName":
                        if (p.Order[0].Dir == DataTableViewModel.DataTablesOrderDir.DESC)
                            raw = raw.OrderByDescending(o => o.FirstName);
                        else
                            raw = raw.OrderBy(o => o.FirstName);
                        break;
                    case "PieceName":
                        if (p.Order[0].Dir == DataTableViewModel.DataTablesOrderDir.DESC)
                            raw = raw.OrderByDescending(o => o.PieceName);
                        else
                            raw = raw.OrderBy(o => o.PieceName);
                        break;

                    default:
                        raw = raw.SortBy(p.SortOrder);
                        break;
                }
                List<view_MusicPieces> parsedResults;
                if (p.Length > 0)
                    parsedResults = raw.Skip(p.Start).Take(p.Length).ToList();
                else
                    parsedResults = raw.ToList();
                var data = parsedResults.Select(s => new MusicGridViewModel(s)).ToList();
                var totalCount = raw.Count();
                DataTableViewModel.DataTablesResult<MusicGridViewModel> result = new DataTableViewModel.DataTablesResult<MusicGridViewModel>()
                {
                    draw = p.Draw,
                    recordsFiltered = totalCount,
                    recordsTotal = totalCount,
                    data = data
                };

                return Json(result);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }

        }

        // GET: MusicPieces/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicPiece musicPiece = dal.DbContext.MusicPieces.Find(id);
            if (musicPiece == null)
            {
                return HttpNotFound();
            }
            return View(musicPiece);
        }

        //GET: Hover agent
        public ActionResult HoverAgent(int? id)
        {
            Composer composerDetails = dal.DbContext.Composers.Find(id);
            if (composerDetails == null)
            {
                return HttpNotFound();
            }
            return View(composerDetails);
        }

        // GET: MusicPieces/Create
        public ActionResult Create()
        {
            ViewBag.ComposerID = new SelectList(dal.DbContext.Composers, "ComposerID", "FirstName");
            ViewBag.NamingConventionID = new SelectList(dal.DbContext.OpusNamingConventions, "NamingConventionID", "PieceType");
            return View();
        }

        // POST: MusicPieces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecordNumber,NamingConventionID,ComposerID,Difficulty,Year")] MusicPiece musicPiece)
        {
            if (ModelState.IsValid)
            {
                dal.DbContext.MusicPieces.Add(musicPiece);
                dal.DbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComposerID = new SelectList(dal.DbContext.Composers, "ComposerID", "FirstName", musicPiece.ComposerID);
            ViewBag.NamingConventionID = new SelectList(dal.DbContext.OpusNamingConventions, "NamingConventionID", "PieceType", musicPiece.NamingConventionID);
            return View(musicPiece);
        }

        // GET: MusicPieces/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicPiece musicPiece = dal.DbContext.MusicPieces.Find(id);
            if (musicPiece == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComposerID = new SelectList(dal.DbContext.Composers, "ComposerID", "FirstName", musicPiece.ComposerID);
            ViewBag.NamingConventionID = new SelectList(dal.DbContext.OpusNamingConventions, "NamingConventionID", "PieceType", musicPiece.NamingConventionID);
            return View(musicPiece);
        }

        // POST: MusicPieces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordNumber,NamingConventionID,ComposerID,Difficulty,Year")] MusicPiece musicPiece)
        {
            if (ModelState.IsValid)
            {
                dal.DbContext.Entry(musicPiece).State = EntityState.Modified;
                dal.DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComposerID = new SelectList(dal.DbContext.Composers, "ComposerID", "FirstName", musicPiece.ComposerID);
            ViewBag.NamingConventionID = new SelectList(dal.DbContext.OpusNamingConventions, "NamingConventionID", "PieceType", musicPiece.NamingConventionID);
            return View(musicPiece);
        }

        // GET: MusicPieces/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicPiece musicPiece = dal.DbContext.MusicPieces.Find(id);
            if (musicPiece == null)
            {
                return HttpNotFound();
            }
            return View(musicPiece);
        }

        // POST: MusicPieces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MusicPiece musicPiece = dal.DbContext.MusicPieces.Find(id);
            dal.DbContext.MusicPieces.Remove(musicPiece);
            dal.DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dal.DbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
