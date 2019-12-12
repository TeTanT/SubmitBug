using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SubmitBug.Models
{
    public class TB_RoleAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500), Display(Name = "功能编码")]
        public string RoleEncodings { get; set; }

        public virtual TB_Role Role { get; set; }
        [Display(Name = "角色ID")]
        public int RId { get; set; }
    }
}