using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3M.Shared.DTOs.Ingredients
{
    public class IngredientRequest
    {
        public string Ingredient { get; set; }
        public List<string> CurrentIngredients { get; set; }
    }
}
