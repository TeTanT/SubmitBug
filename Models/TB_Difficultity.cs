using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SubmitBug.Models
{
    public class TB_Difficultity
    {
        public TB_Difficultity()
        {
            this.Follow = new HashSet<TB_Follow>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DId { get; set; }
        [Required]
        [MaxLength(20), Display(Name = "难易度")]
        public string DLevel { get; set; }

        public virtual ICollection<TB_Follow> Follow { get; set; }
    }
}