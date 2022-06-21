using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using MVCDemo.Data;
using MVCDemo.Models;

namespace MVCDemo.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MVCDemoContext _context;

        //默认日志记录
        private readonly ILogger _logger;

        public string Message;

        public MoviesController(MVCDemoContext context,ILogger<MoviesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void OnGet()
        {
            Message = $"About page visited at {DateTime.UtcNow.ToLongTimeString()}";
            //写入到日志中

            _logger.LogInformation(Message);


        }



        // GET: Movies
        public async Task<IActionResult> Index(string searchString)
        {
            var movies = from m in _context.Movie
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }
            //get到all
            var list = _context.Movie.ToList();
            return View(await _context.Movie.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
        
                _context.Add(movie);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var p=_context.Movie.FindAsync(id,"123");
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
                
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
                
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mm=_context.Find<Movie>(id);
            var mm2 = _context.Movie.Find(id);

            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            
            await _context.SaveChangesAsync();
          
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            
            return _context.Movie.Any(e => e.Id == id);
        }
        /*
         * {id:int}
         * {active:bool}
         * {dob:datetime}
         * {price:decimal}
         * {id:guid}
         * {ticks:long}
         * {filename:maxlength(8):minlength(4)}
         * {filename:length(5.7)}
         * {age:range(1,4)}{age:min(5)}{age:max(120)}
         * {name:alpha}
         * {ssn:regex(^\\d{{3}}-\\d{{2}}-\\d{{4}}$)}
         * {name:required}强制
         */

        //[Route]("users/{id:int:min(1)}")
        [HttpGet]
        public List<Movie> GetMovieByTitle(string title) {
            var db = _context.Movie.Where(t => t.Title == title)
                .OrderBy(d => d.Id)
                .ToList();
                
            return db;
        }

    }

}
