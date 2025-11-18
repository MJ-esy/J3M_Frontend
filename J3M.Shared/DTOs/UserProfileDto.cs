using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3M.Shared.DTOs.Users.ProfilesDtos;

public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;

    [StringLength(100)]
    public string? DisplayName { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;
}

