using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuOpaTrading.Models
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public long? GameID { get; set; }
    }
}
