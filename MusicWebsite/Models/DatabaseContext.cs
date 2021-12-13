using System;
using System.Collections.Generic;
using System.Data.Entity;
//using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Web;

namespace MusicWebsite.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }
        public DbSet<MusicPiece> MusicPiece { get; set; }
    }
}