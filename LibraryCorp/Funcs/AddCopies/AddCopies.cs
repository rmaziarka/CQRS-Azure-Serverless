using System;
using System.Collections.Generic;
using LibraryCorp.Models;

namespace LibraryCorp.Funcs.AddCopies
{
    public class AddCopies
    {

        public Guid LibraryId { get; set; }

        public Guid BrandId { get; set; }
        
        public List<string> CopyNumbers { get; set; }
    }
}
