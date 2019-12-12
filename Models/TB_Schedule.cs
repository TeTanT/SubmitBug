using SubmitBug.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SubmitBug.Models
{
    public class TB_Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SId { get; set; }

        [Required]
        [CheckDateRangeAttribute]
        [DataType(DataType.Date)]
        [Display(Name = "完成时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FinishDate { get; set; }

        public virtual TB_BugSubmit BugSubmit { get; set; }
        public int BId { get; set; }

        public virtual TB_Follow Follow { get; set; }
        public int FId { get; set; }
    }
}