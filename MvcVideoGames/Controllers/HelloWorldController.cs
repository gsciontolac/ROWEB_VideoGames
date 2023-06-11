using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcVideoGames.Controllers
{
    public class HelloWorldController : Controller
    {
        // 
        // GET: /HelloWorld/

        public IActionResult Index()
        {
            return View();//"This is my default action...";
        }

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }

        // 
        // GET: /HelloWorld/Welcome/ 
        /*
        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

        public string Welcome(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }*/

        public string Welcome1(string name, int ID = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
        }
    }
}
