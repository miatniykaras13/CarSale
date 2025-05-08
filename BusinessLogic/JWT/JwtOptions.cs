using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.JWT
{
    public class JwtOptions
    {
        public int Expires { get; set; }

        public string SecretKey { get; set; } = String.Empty;
    }
}
