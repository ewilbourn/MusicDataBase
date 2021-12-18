using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicWebsite.ViewModels;

namespace MusicWebsite.classes
{
    /// <summary>
    /// This class helps to pass filters for filtering the datatable
    /// </summary>
    public class MusicTableParameters : DataTableViewModel.DataTablesParameters
    {
        public string FullName { get; set; }

        public int Difficulty { get; set; }

        public string KeySignature { get; set; }

        public int ComposerID { get; set; }

        public int Year { get; set; }

        public string Instrument { get; set; }
    }
}