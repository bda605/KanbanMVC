using System.Collections.Generic;

namespace KanbanMVC.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ListId { get; set; }
        public ItemList ItemLists { get; set; }
    }
}
