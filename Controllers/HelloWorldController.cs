using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class HelloWorldController : Controller
    {
        public string Index()
        {
            return "default action";
        }

        public string hello(string name,int age) {
            name = "abc";
            age = 9;
            return HtmlEncoder.Default.Encode($"Hello{name} age {age}");
        }
    }
}
