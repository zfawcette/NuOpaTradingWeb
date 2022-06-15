using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuOpaTrading.Models
{
    public class DetailsVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public List<string> ImageURLS { get; set; }
        public string TrailerURL { get; set; }
        public List<string> Genres { get; set; }
        public string ReleaseDate { get; set; }
        public List<string> PlayModes { get; set; }

    }
}
