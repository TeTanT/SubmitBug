using SubmitBug.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SubmitBug.Models
{
    public class TB_Role
    {
        public TB_Role()
        {
            this.RoleAction = new HashSet<TB_RoleAction>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RId { get; set; }
        [Required]
        [MaxLength(20), Display(Name = "角色编码")]
        public string RoleEncoding { get; set; }
        [Required]
        [MaxLength(20), Display(Name = "角色名称")]
        public string RoleName { get; set; }
        [Required]
        [MaxLength(20), Display(Name = "角色描述")]
        public string RoleDesc { get; set; }
        [CheckDateRangeAttribute]
        [Display(Name = "提交时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}",ApplyFormatInEditMode =true)]
        public DateTime AddRoleDate { get; set; }

        public virtual ICollection<TB_RoleAction> RoleAction { get; set; }

        //public virtual ICollection<TB_Users> Users { get; set; }
    }
}