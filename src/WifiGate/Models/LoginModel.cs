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

        public void AppendToLogFile(string fileName, int maximumPasswordLength) {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(fileName));
            if (maximumPasswordLength < 1) throw new ArgumentOutOfRangeException(nameof(maximumPasswordLength), "Value must be greater than zero.");

            // Show only first maximumPasswordLength chars from password
            string displayPassword;
            if (this.Password.Length <= maximumPasswordLength) {
                displayPassword = this.Password;
            }
            else {
                displayPassword = this.Password.Substring(0, maximumPasswordLength)
                    + new string('*', this.Password.Length - maximumPasswordLength);
            }

            // Append line to file
            var line = string.Join("\t", this.Service, this.UserName, displayPassword) + "\r\n";
            File.AppendAllText(fileName, line);
        }

    }
}
