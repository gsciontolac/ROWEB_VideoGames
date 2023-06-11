using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcVideoGames.Data;
using MvcVideoGames.Models;

namespace MvcVideoGames.Controllers
{
    public class VideoGamesController : Controller
    {
        private readonly MvcVideoGamesContext _context;

        public VideoGamesController(MvcVideoGamesContext context)
        {
            _context = context;
        }

        // GET: VideoGames
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string filterType, string searchString, int? pageNumber, int itemsPerPage)
        {
            ViewData["TitleSortParam"] = string.IsNullOrEmpty(sortOrder) ? "Title_desc" : "";
            ViewData["ReleaseDateSortParam"] = sortOrder == "ReleaseDate" ? "ReleaseDate_desc" : "ReleaseDate";
            ViewData["GenreSortParam"] = sortOrder == "Genre" ? "Genre_desc" : "Genre";
            ViewData["PriceSortParam"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["CurrentType"] = string.IsNullOrEmpty(filterType) ? null : filterType;
            ViewData["PageNumber"] = pageNumber;
            ViewData["ItemsPerPage"] = itemsPerPage;

            if (itemsPerPage == 0)
                itemsPerPage = 3;

            var videoGames = from v in _context.VideoGames
                           select v;
            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            bool desc = false;

            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "Title";

            if(sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                desc = true;
            }

            //verificam daca pretul e nr
            decimal temp;
            if(filterType == "Price" && !decimal.TryParse(searchString, out temp))
            {
                ViewData["InputError"] = "Price is not a number!";
                if (desc)
                {
                    return View(await PaginatedList<VideoGames>.CreateAsyncSortDesc(videoGames, pageNumber ?? 1, itemsPerPage, sortOrder));
                }
                else
                {
                    return View(await PaginatedList<VideoGames>.CreateAsyncSorted(videoGames, pageNumber ?? 1, itemsPerPage, sortOrder));
                }
            }

            if(!string.IsNullOrEmpty(searchString))
            {
                videoGames = videoGames.Where(v => EF.Property<object>(v, filterType) == searchString);
            }

            /*switch (sortOrder)
            {
                case "title_desc":
                    videoGames = videoGames.OrderByDescending(v => v.Title);
                    break;
                case "Genre":
                    videoGames = videoGames.OrderBy(v => v.Genre);
                    break;
                case "genre_desc":
                    videoGames = videoGames.OrderByDescending(v => v.Genre);
                    break;
                case "ReleaseDate":
                    videoGames = videoGames.OrderBy(v => v.ReleaseDate);
                    break;
                case "releaseDate_desc":
                    videoGames = videoGames.OrderByDescending(v => v.ReleaseDate);
                    break;
                case "Price":
                    videoGames = videoGames.OrderBy(v => v.Price);
                    break;
                case "price_desc":
                    videoGames = videoGames.OrderByDescending(v => v.Price);
                    break;


                default:
                    videoGames = videoGames.OrderBy(v => v.Title);
                    break;
            }*/

        
            
            if(desc)
            {
                return View(await PaginatedList<VideoGames>.CreateAsyncSortDesc(videoGames, pageNumber ?? 1, itemsPerPage, sortOrder));
            }
            else
            {
                return View(await PaginatedList<VideoGames>.CreateAsyncSorted(videoGames, pageNumber ?? 1, itemsPerPage, sortOrder));
            }

        }

        // GET: VideoGames/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewData["VideoGamesId"] = new SelectList(id.ToString().AsEnumerable());
            if (id == null || _context.VideoGames == null)
            {
                return NotFound();
            }

            var videoGame = await _context.VideoGames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoGame == null)
            {
                return NotFound();
            }

            var reviews = _context.Review.Where(r => r.VideoGamesId == id);
            var model = new VideoGameReviews() { VideoGame=videoGame, Reviews=reviews };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("Id, UserName, ReviewVideoGame, VideoGamesId")] Review review)
        {
            review.CreatedDate = DateTime.Now;
            _context.Add(review);
                await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = review.VideoGamesId});
        }

        // GET: VideoGames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VideoGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] VideoGames videoGames)
        {
            if (ModelState.IsValid)
            {
                _context.Add(videoGames);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(videoGames);
        }

        // GET: VideoGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VideoGames == null)
            {
                return NotFound();
            }

            var videoGames = await _context.VideoGames.FindAsync(id);
            if (videoGames == null)
            {
                return NotFound();
            }
            return View(videoGames);
        }

        // POST: VideoGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] VideoGames videoGames)
        {
            if (id != videoGames.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoGames);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoGamesExists(videoGames.Id))
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
            return View(videoGames);
        }

        // GET: VideoGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VideoGames == null)
            {
                return NotFound();
            }

            var videoGames = await _context.VideoGames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoGames == null)
            {
                return NotFound();
            }

            return View(videoGames);
        }

        // POST: VideoGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VideoGames == null)
            {
                return Problem("Entity set 'MvcVideoGamesContext.VideoGames'  is null.");
            }
            var videoGames = await _context.VideoGames.FindAsync(id);
            if (videoGames != null)
            {
                _context.VideoGames.Remove(videoGames);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoGamesExists(int id)
        {
          return (_context.VideoGames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
