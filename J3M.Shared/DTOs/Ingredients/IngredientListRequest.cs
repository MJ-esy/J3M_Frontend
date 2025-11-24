using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3M.Shared.DTOs.Ingredients
{

    public class IngredientListRequest
    {
        public string Ingredient { get; set; } = string.Empty;
        public List<string> CurrentIngredients { get; set; } = new();
    }
}
