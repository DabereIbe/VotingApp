using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class Aspirants
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [DisplayName("Position")]
        public int PositionId { get; set; }

        public int Votes { get; set; }

        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
    }
}
