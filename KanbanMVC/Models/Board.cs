using System.Collections.Generic;

namespace KanbanMVC.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ItemList> ItemLists { get; set; } = new List<ItemList>();
    }
}
