namespace J3M.DTOs
{
    public class UserPageDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        
        public List<RecipeDto> SavedRecipes { get; set; }
    }
}
