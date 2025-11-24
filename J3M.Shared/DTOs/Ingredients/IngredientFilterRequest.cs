using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3M.Shared.DTOs.Ingredients
{
    public class IngredientFilterRequest
    {
        public List<string> UserIngredients { get; set; } = null!;
    }
}
