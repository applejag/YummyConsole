using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole
{
    public static class Input
    {
        internal static List<ConsoleKey> pressedKeys;
        private static string _inputString = string.Empty;
        private static bool _anyKeyDown;
        
        /// <summary>
        /// Returns true the frame a key is pressed.
        /// </summary>
        public static bool AnyKeyDown => _anyKeyDown;
        /// <summary>
        /// Returns the keyboard input entered this frame.
        /// </summary>
        public static string InputString => _inputString;

        internal static void AnalyzeInput()
        {
            _inputString = string.Empty;
            pressedKeys = new List<ConsoleKey>();

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                _inputString += info.KeyChar;
                pressedKeys.Add(info.Key);
                _anyKeyDown = true;
            }
        }

        public static bool GetKeyDown(ConsoleKey key)
        {
            return pressedKeys?.Contains(key) ?? false;
        }

    }
}
