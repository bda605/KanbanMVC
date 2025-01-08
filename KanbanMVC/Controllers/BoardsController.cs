using KanbanMVC.Data;
using KanbanMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanMVC.Controllers
{
    public class BoardsController : Controller
    {
        private readonly KanbanDbContext _context;

        public  BoardsController(KanbanDbContext context)
        {
            _context = context;
            var boards =  _context.Boards.Include(b => b.ItemLists).ThenInclude(l => l.Issues).FirstOrDefault();
            var lists =  _context.ItemList.Include(l => l.Issues).ToList();
            if (boards == null && !lists.Any())
            {
                var board = new Board { Name = "Default Board" };
                _context.Boards.Add(board);
                 _context.SaveChanges();
                _context.ItemList.Add(new ItemList { Name = "待辦清單", BoardId = board.Id });
                _context.ItemList.Add(new ItemList { Name = "進行", BoardId = board.Id });
                _context.ItemList.Add(new ItemList { Name = "完成", BoardId = board.Id });
                _context.SaveChanges();
            }
        }

        public async Task<IActionResult> Index()
        {
            
            var boards = await _context.Boards.Include(b => b.ItemLists).ThenInclude(l => l.Issues).FirstOrDefaultAsync();
            return View(boards);
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Board board)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Boards.Add(board);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(board);
        //}
        //// Edit
        //[HttpGet]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null) return NotFound();
        //    var board = await _context.Boards.FindAsync(id);
        //    if (board == null) return NotFound();
        //    return View(board);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Board board)
        //{
        //    if (id != board.Id) return NotFound();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(board);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BoardExists(board.Id)) return NotFound();
        //            else throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(board);
        //}

        //// Delete
        //[HttpGet]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null) return NotFound();
        //    var board = await _context.Boards.FindAsync(id);
        //    if (board == null) return NotFound();
        //    return View(board);
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board != null)
            {
                _context.Boards.Remove(board);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(int id)
        {
            return _context.Boards.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> Move([FromBody] IssueMoveDto moveDto)
        {
            var task = await _context.Issue.FindAsync(moveDto.TaskId);
            if (task == null) return NotFound();

            task.ListId = moveDto.NewListId;
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddIssue([FromBody] AddIssueViewModel model)
        {
            if (ModelState.IsValid)
            {
                var list = await _context.ItemList.FindAsync(model.ListId);
                var newIssue = new Issue
                {
                    Title = model.Title,
                    Description =model.Description, // 可以選擇是否提供描述
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ListId = model.ListId,
                    ItemLists = list
                };

                _context.Issue.Add(newIssue);
                await _context.SaveChangesAsync();

                return Json(new { success = true, taskId = newIssue.Id, title = newIssue.Title });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteIssue([FromBody] DeleteIssueViewModel model)
        {
            var issue = await _context.Issue.FindAsync(model.TaskId);
            if (issue == null)
            {
                return NotFound();
            }

            _context.Issue.Remove(issue);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public class DeleteIssueViewModel
        {
            public int TaskId { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> EditIssue([FromBody] IssueViewModel model)
        {
            var issue = await _context.Issue.FindAsync(model.TaskId);
            if (issue == null)
            {
                return NotFound();
            }

            issue.Title = model.Title;
            issue.Description = model.Description;
            issue.StartDate = model.StartDate;
            issue.EndDate = model.EndDate;
            await _context.SaveChangesAsync();
            return Ok();
        }

        public class IssueViewModel
        {
            public int TaskId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime StartDate { get; set; } // 開始日期
            public DateTime EndDate { get; set; }   // 結束日期
        }
        public class AddIssueViewModel
        {
            public int ListId { get; set; }
            public string Title { get; set; }

            public string Description { get; set; }
            public DateTime StartDate { get; set; } // 開始日期
            public DateTime EndDate { get; set; }   // 結束日期
        }
        public class IssueMoveDto
        {
            public int TaskId { get; set; }
            public int NewListId { get; set; }
        }
    }
}
