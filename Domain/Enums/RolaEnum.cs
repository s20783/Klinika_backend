using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    [Flags]
    public enum RolaEnum
    {
        Admin = 1,
        Klient = 2,
        Weterynarz = 3
    }
}