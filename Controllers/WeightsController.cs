using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DT102G_WeightApp.Data;
using DT102G_WeightApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ExcelDataReader;
using Microsoft.AspNet.Identity;

namespace DT102G_WeightApp.Controllers
{
    public class WeightsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WeightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Weights 
        // Get entries from database corresponding to logged in user id
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Get user custom values
            ViewBag.UserInfo = _context.Users.Where(x => x.Id == User.Identity.GetUserId()).SingleOrDefault();
            
            // Get all entries from Weights and Comments tables
            var results = await (from w in _context.Weights
                join c in _context.Comments on w.Id equals c.WeightModelId into table_join
                from c in table_join.DefaultIfEmpty()
                orderby w.Date
                where w.UserName == User.Identity.GetUserId()
                select new WeightViewModel()
                {
                    WeightId = w.Id,
                    Date = w.Date,
                    Weight = w.Weight,
                    Comment = c.Comment
                }).ToListAsync();

            // Send result to view
            return View(results);
        }
   
        // GET: Weights/Details/5
        // Get info on specific entry
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get info from Weights and Comments tables
            var results = await (from w in _context.Weights
                join c in _context.Comments on w.Id equals c.WeightModelId into table_join
                from c in table_join.DefaultIfEmpty()
                select new WeightViewModel()
                {
                    WeightId = w.Id,
                    Date = w.Date,
                    Weight = w.Weight,
                    Comment = c.Comment
                }).FirstOrDefaultAsync(m => m.WeightId == id);

            if (results == null)
            {
                return NotFound();
            }

            return View(results);
        }

        // GET: Weights/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Weights/Create
        // Get input from user and save to database using viewmodel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Date,Weight,Comment")] WeightViewModel weightViewModel)
        {
            if (ModelState.IsValid)
            {
                // First save weight to table
                WeightModel weight = new WeightModel()
                {
                    UserName = User.Identity.GetUserId(),
                    Date = weightViewModel.Date,
                    Weight = weightViewModel.Weight
                };
                _context.Add(weight);
                await _context.SaveChangesAsync();

                // Check if comment is added and save to database
                if(weightViewModel.Comment != null)
                {
                    int lastWeightId = _context.Weights.Max(item => item.Id);
                    CommentModel comment = new CommentModel()
                    {
                        WeightModelId = lastWeightId,
                        Comment = weightViewModel.Comment
                    };
                    _context.Add(comment);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(weightViewModel);
        }

        // GET: Weights/Edit/5
        // Get specific info from database and offer edit option
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var results = await (from w in _context.Weights
                join c in _context.Comments on w.Id equals c.WeightModelId into table_join
                from c in table_join.DefaultIfEmpty()
                select new WeightViewModel()
                {
                    WeightId = w.Id,
                    Date = w.Date,
                    Weight = w.Weight,
                    Comment = c.Comment
                }).FirstOrDefaultAsync(m => m.WeightId == id);

            if (results == null)
            {
                return NotFound();
            }
            return View(results);
        }

        // POST: Weights/Edit/5
        // If user saves edited info update database
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("WeightId,Date,Weight,Comment")] WeightViewModel weightViewModel)
        {
            if (id != weightViewModel.WeightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // First update Weights tables
                    var weightModel = await _context.Weights.FirstOrDefaultAsync(m => m.Id == id);
                    weightModel.Date = weightViewModel.Date;
                    weightModel.Weight = weightViewModel.Weight;
                    _context.Update(weightModel);

                    var commentModel = await _context.Comments.FirstOrDefaultAsync(m => m.WeightModelId == id);

                    // If prior comment exist, update else create new
                    if(commentModel != null)
                    {
                        commentModel.Comment = weightViewModel.Comment;
                        _context.Update(commentModel);
                    }
                    else
                    {
                        CommentModel comment = new CommentModel()
                        {
                            WeightModelId = id,
                            Comment = weightViewModel.Comment
                        };
                        _context.Add(comment);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeightModelExists(weightViewModel.WeightId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(weightViewModel);
        }

        // GET: Weights/Delete/5
        // Display info thats about to be deleted
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var results = await (from w in _context.Weights
                join c in _context.Comments on w.Id equals c.WeightModelId into table_join
                from c in table_join.DefaultIfEmpty()
                select new WeightViewModel()
                {
                    WeightId = w.Id,
                    Date = w.Date,
                    Weight = w.Weight,
                    Comment = c.Comment
                }).FirstOrDefaultAsync(m => m.WeightId == id);
            if (results == null)
            {
                return NotFound();
            }

            return View(results);
        }

        // POST: Weights/Delete/5
        // Delete chosen info. Since Comments table contain foreign key corresponding entires will be deleted
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weightModel = await _context.Weights.FindAsync(id);
            _context.Weights.Remove(weightModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeightModelExists(int id)
        {
            return _context.Weights.Any(e => e.Id == id);
        }

        // GET: Display import from Excel options
        [HttpGet]
        [Authorize]
        public IActionResult Excel()
        {
            return View();
        }

        // POST: Import from Excel
        [HttpPost]
        [Authorize]
        public IActionResult Excel(IFormFile file, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            // Save file and read values
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using(FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using(var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        // Get values from excel start from a1 cell
                        string stringDate = reader.GetValue(0).ToString();
                        DateTime date = DateTime.Parse(stringDate);

                        string stringWeight = reader.GetValue(1).ToString();
                        Decimal weight = Decimal.Parse(stringWeight);

                        // First save to Weights table
                        WeightModel weightModel = new WeightModel()
                        {
                            UserName = User.Identity.GetUserId(),
                            Date = date,
                            Weight = weight
                        };
                        _context.Add(weightModel);
                        _context.SaveChanges();

                        // If comment exist save to Comments table
                        if (reader.GetValue(2) != null)
                        {
                            int lastWeightId = _context.Weights.Max(item => item.Id);
                            CommentModel commentModel = new CommentModel()
                            {
                                WeightModelId = lastWeightId,
                                Comment = reader.GetValue(2).ToString()
                            };
                            _context.Add(commentModel);
                            _context.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
