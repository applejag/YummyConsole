using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace YummyConsole
{
    public class BeepPlayer
    {

        public const string Tetris = "E5-4,B4-8,C5-8,D5-4,C5-8,B4-8,A4-4,A4-8,C5-8,E5-4,"
                                     + "D5-8,C5-8,B4-4,C5-8,D5-4,E5-4,C5-4,A4-4,A4-8,A4-8,"
                                     + "B4-8,C5-8,D5-4,F5-8,A5-4,G5-8,F5-8,E5-4,C5-8,E5-4,"
                                     + "D5-8,C5-8,B4-4,B4-8,C5-8,D5-4,E5-4,C5-4,A4-4,A4-4,"
                                     + "P-4,E5-2,C5-2,D5-2,B4-2,C5-2,A4-2,GS4-2,B4-4,P-4,"
                                     + "E5-2,C5-2,D5-2,B4-2,C5-4,E5-4,A5-2,GS5-2";

        public enum Octave
        {
            DoublePedal,
            Pedal,
            Deep,
            Low,
            Middle,
            Tenor,
            High,
            DoubleHigh,
        }
        public enum Note
        {
            C, Cs, D, Ds, E, F, Fs, G, Gs, A, As, B, P
        }

        private static readonly int[] Frequency = {
            16,17,18,19,21,22,23,24,26,27,29,31,
            33,35,37,39,41,44,46,49,52,55,58,62,
            65,69,73,78,82,87,92,98,104,110,116,123,
            131,139,147,155,165,175,185,196,208,220,233,245,
            262,277,294,311,330,349,370,392,415,440,466,494,
            523,554,587,622,659,698,740,784,831,880,932,988,
            1046,1109,1175,1244,1328,1397,1480,1568,1661,1760,1865,1975,
            2093,2217,2349,2489,2637,2794,2960,3136,3322,3520,3729,3951
        };

        public static int GetFrequency(Note note, Octave octave)
        {
            return Frequency[12 * (int) octave + (int) note];
        }

        public static async Task PlayTones(Tone[] tones)
        {
            await Task.Run(() => { 
                int length = tones.Length;
                for (int i = 0; i < length; i++)
                {
                    if (tones[i].note == Note.P)
                        Task.Delay(tones[i].milliseconds);
                    else
                        Console.Beep(tones[i].frequency, tones[i].milliseconds);
                }
            });
        }

        public static Tone[] ParseTones(string input)
        {
            string[] strTones = input.Split(new[] {"\n\r", "\r\n", "\n", "\r", ","}, StringSplitOptions.RemoveEmptyEntries);
            int length = strTones.Length;
            Tone[] tones = new Tone[length];
            for (int i = 0; i < length; i++)
            {
                tones[i] = ParseTone(strTones[i]);
            }
            return tones;
        }

        private static Tone ParseTone(string input)
        {
            Match match = Regex.Match(input, @"^((?:[ABCDEFG][s]?)|P)([0-7])?(?:-(1(?:6)?|2|4|8))?$", RegexOptions.IgnoreCase);
            if (!match.Success) throw new FormatException($"Invalid tone format! \"{input}\"");

            return new Tone
            {
                note = (Note) Enum.Parse(typeof(Note), match.Groups[1].Value, true),
                octave = match.Groups[2].Success ? (Octave)int.Parse(match.Groups[2].Value) : Octave.Middle,
                duration = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 1,
            };
        }

        public struct Tone
        {
            public Note note;
            public Octave octave;
            public int duration;
            public int milliseconds => 1000 / duration;
            public int frequency => GetFrequency(note, octave);
        }
    }
}