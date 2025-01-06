namespace KanbanMVC.Models
{
    public class ItemList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    }
}
