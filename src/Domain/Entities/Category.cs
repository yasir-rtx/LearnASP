namespace LearnASP.Domain.Entities
{
    public class Category
    {
        public int Id {get; set;}
        public string Name {get; set;} = String.Empty;
        public string Slug {get; set;} = String.Empty;
        public string Description {get; set;} = String.Empty;
        public bool IsActive {get; set;}
        public int CreatedBy {get; set;}
        public int UpdatedBy {get; set;}
        public DateTime? CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }
}