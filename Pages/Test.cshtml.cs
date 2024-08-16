using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Pages
{
    public class TestModel : PageModel
    {
        [BindProperty] public string namee { get; set; }
        [BindProperty] public int age { get; set; }
        public int Count { get; set; } = 1;

        [BindProperty][Required(ErrorMessage = "name not right")] public string name { get; set; }
        [BindProperty][Required] public string first { get; set; }
        [BindProperty][Required] public string last { get; set; }



        public string url { get; set; }
        public string testform {  get; set; } = "defaultvalue";
        public string Message { get; set; }
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string SayGoodbye() => $"Hello, {FullName}! I'm in the model class";
        public void OnGet()
        {
            url = Request.Query["url"].ToString().Replace("-", " ");
            FirstName = "Donald";
            LastName = "Trump";
            Message = "GET";
        }
        public void OnPost()
        {
            Message = "POST";
            if (!ModelState.IsValid) {
            }
            //return NotFound();
        }
        public void OnPostTest()
        {
            testform = "Test";
        }
        public void OnPostTest2()
        {
            testform = "Test 2";
        }
        public void OnPostTest3()
        {
            testform = "Test 3";
        }
        //[AllowAnonymous]
        //[IgnoreAntiforgeryToken]
        //[DisableCors]
        //[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostDataTest()
        {
            Console.Write("-------- XSADHSAD --------");
            return new JsonResult(new { a = "ass", b = "age" });
        }
        public IActionResult OnGetDataa()
        {
            return new JsonResult(new { messageX = "aaaaaa" });
        }
    }
}

