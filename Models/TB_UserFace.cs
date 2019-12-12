using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SubmitBug.Models
{
    public class TB_UserFace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UId { get; set; }

        [Required]
        [MaxLength(50), Display(Name = "图片ID")]
        public string UserFace { get; set; }

        public virtual TB_LoginOn LoginOn { get; set; }
        [Display(Name = "用户ID")]
        public int LId { get; set; }
    }
}