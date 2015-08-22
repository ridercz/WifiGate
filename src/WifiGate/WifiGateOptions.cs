using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiGate {
    public class WifiGateOptions {

        public string ForceHostName { get; set; }

        public string OutputFileName { get; set; }

        public int MaximumPasswordLength { get; set; }

        public List<ProviderInfo> IdentityProviders { get; set; }
        
        public class ProviderInfo {

            public string Id { get; set; }

            public string Title { get; set; }

        }

    }
}
