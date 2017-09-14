using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole
{
    [Flags]
    public enum Color : short
    {
        BLACK = 0x00,
        INTENSITY = 0x88,
        RED = 0x44,
        GREEN = 0x22,
        BLUE = 0x11,
        YELLOW = GREEN | RED,
        MAGENTA = BLUE | RED,
        CYAN = BLUE | GREEN,
        GREY = RED | GREEN | BLUE,

        LIGHT_RED = RED | INTENSITY,
        LIGHT_GREEN = GREEN | INTENSITY,
        LIGHT_BLUE = BLUE | INTENSITY,
        LIGHT_YELLOW = YELLOW | INTENSITY,
        LIGHT_MAGENTA = MAGENTA | INTENSITY,
        LIGHT_CYAN = CYAN | INTENSITY,
        WHITE = GREY | INTENSITY,

        DEFAULT_BACKGROUND = BLACK,
        DEFAULT_FOREGROUND = GREY,
    }
}
