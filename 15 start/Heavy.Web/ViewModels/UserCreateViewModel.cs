﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Heavy.Web.ViewModels
{
    public class UserCreateViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName{ get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("[\\w!#$%&'*+/=?^_`{|}~-]+(?:\\.[\\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\\w](?:[\\w-]*[\\w])?\\.)+[\\w](?:[\\w-]*[\\w])?")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Required]
        [Display(Name = "身份证号")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "{0}的长度是{1}")]
        public string IdCardNo { get; set; }

        [Required]
        [Display(Name = "出生日期")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
