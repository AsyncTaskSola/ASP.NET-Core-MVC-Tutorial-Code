using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Heavy.Web.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("[\\w!#$%&'*+/=?^_`{|}~-]+(?:\\.[\\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\\w](?:[\\w-]*[\\w])?\\.)+[\\w](?:[\\w-]*[\\w])?")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [MaxLength(18)]
        public string IdCardNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public List<string> Claims  { get; set; }
    }
}
