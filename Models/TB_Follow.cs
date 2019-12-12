using SubmitBug.BLL;
using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SubmitBug.Models
{
    public class TB_Follow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FId { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(100), Display(Name = "跟进计划")]
        public string Schedule { get; set; }

        [CheckDateRangeAttribute]
        [DataType(DataType.Date)]
        [Display(Name = "计划完成时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FollowDate { get; set; }

        public virtual ICollection<TB_Schedule> TB_Schedule { get; set; }

        public virtual TB_LoginOn LoginOn { get; set; }
        [Display(Name = "跟进ID")]
        public int LId { get; set; }
        public virtual TB_BugSubmit BugSubmit { get; set; }
        public int BId { get; set; }

        public virtual TB_Difficultity Difficultity { get; set; }
        public int DId { get; set; }

       

    }
}