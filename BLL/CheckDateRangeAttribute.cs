using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SubmitBug.BLL
{
    public class CheckDateRangeAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dt = (DateTime)value;
                if (dt >= DateTime.Today)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(ErrorMessage ?? "您输入的日期必须大于等于今天的日期！");
            }
            return new ValidationResult(ErrorMessage ?? "您没有输入日期！");
        }
    }
}