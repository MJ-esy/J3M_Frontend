namespace J3M.Models
{
    public class DietOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; internal set; }
    }
}
