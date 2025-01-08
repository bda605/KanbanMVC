namespace KanbanMVC.ViewModels
{
    public class AddIssueViewModel
    {

        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime StartDate { get; set; } // 開始日期
        public DateTime EndDate { get; set; }   // 結束日期
        public int ListId { get; set; }
    }
}
