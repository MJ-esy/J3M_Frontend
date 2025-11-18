using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3M.Shared.DTOs
{
    public class UpdateProfileDto
    {
        [Required, MaxLength(100)]
        public string? UserName { get; set; } = string.Empty;


        [MaxLength(100)]
        public string? DisplayName { get; set; }

        [EmailAddress]
        [Required, MaxLength(256)]
        public string? Email { get; set; } = string.Empty;
    }
}
