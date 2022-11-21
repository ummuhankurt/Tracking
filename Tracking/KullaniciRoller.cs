using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracking
{
    public class KullaniciRoller
    {
        public int KullaniciId { get; set; }
        public int RolId { get; set; }
        public Kullanici kullanici { get; set; }
        public Rol Rol { get; set; }
    }
}
