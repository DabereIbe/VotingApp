using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models.ViewModels
{
    public class VotesViewModel
    {
        public IEnumerable<Aspirants> Aspirants { get; set; }

        public Position Position { get; set; }

        public int? TotalItems { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public int TotalPages { get; set; }
    }
}
