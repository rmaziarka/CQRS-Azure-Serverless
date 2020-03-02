using System;
using LibraryCorp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCorp.Funcs.ReserveBook
{
    public class ReserveBook
    {
        public Guid ReaderId { get; set; }

        public Guid BrandId { get; set; }

        public Guid LibraryId { get; set; }
    }
}
