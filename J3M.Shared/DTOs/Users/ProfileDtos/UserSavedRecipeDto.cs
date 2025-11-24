using J3m_BE.DTOs.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3M.Shared.DTOs.Users.ProfileDtos
{
    public class UserSavedRecipeDto
    {
            public string FullName { get; set; }
            public string Email { get; set; }

            public List<RecipeDetailDto> SavedRecipes { get; set; }
    }
}
