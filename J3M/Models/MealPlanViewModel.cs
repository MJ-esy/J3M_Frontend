using J3M.Shared.MealPlanModels;

namespace J3M.Models
{
    public class MealPlanViewModel
    {
        public WeeklyMealPlanDto? WeeklyPlan { get; set; }
        public IEnumerable<AllergyOption> Allergies { get; set; } = Enumerable.Empty<AllergyOption>();
        public IEnumerable<DietOption> Diets { get; set; } = Enumerable.Empty<DietOption>();
       
    }

   
}