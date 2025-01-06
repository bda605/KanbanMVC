using KanbanMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KanbanMVC.Data
{
    public class KanbanDbContext: DbContext
    {
        public KanbanDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Board> Boards { get; set; }
        public DbSet<ItemList> ItemList { get; set; }
        public DbSet<Issue> Issue { get; set; }
    }
}
