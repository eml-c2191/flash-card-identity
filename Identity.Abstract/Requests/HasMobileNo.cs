using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Abstract.Requests
{
    public record HasMobileNo
    {
        /// <summary>
        /// Mobile no
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone Number Required!")]
        [RegularExpression("^\\+61\\d{9,11}$",
                   ErrorMessage = "Entered phone format is not valid.")]
        [MaxLength(15)]
        [MinLength(10)]
        public string MobileNo { get; set; } = string.Empty;
    }
}
