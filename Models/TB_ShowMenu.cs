using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SubmitBug.Models
{
    public class TB_ShowMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MId { get; set; }

        [Required]
        [MaxLength(20), Display(Name = "菜单名称")]
        public string MenuName { get; set; }
        public int AId { get; set; }
        public virtual TB_UserAccess UserAccess { get; set; }
        
    }
}