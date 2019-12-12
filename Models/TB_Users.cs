using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SubmitBug.Models
{
    public class TB_Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UId { get; set; }
        [Required]
        [MaxLength(20), Display(Name = "工号")]
        public string LoginNo { get; set; }
        [Required]
        [MaxLength(50), Display(Name = "姓名")]
        public string LoginName { get; set; }
        [Required]
        [MaxLength(50), Display(Name = "密码"), DataType(DataType.Password)]
        public string LoginPwd { get; set; }
        [Required]
        [MaxLength(50), Display(Name = "联系电话")]
        [RegularExpression(@"^[0-9-]*$", ErrorMessage = "电话号码输入不符合规则!")]
        public string LPhone { get; set; }
        [Required,]
        [MaxLength(20), Display(Name = "电脑编号")]
        public string ComputerNo { get; set; }

        //public virtual TB_Role Role { get; set; }
        //[Display(Name = "角色ID")]
        //public int RId { get; set; }

    }
}