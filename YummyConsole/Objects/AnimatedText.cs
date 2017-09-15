using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyConsole.Helpers;

namespace YummyConsole
{
    public class AnimatedText : Text
    {
        public string[] frames;

        /// <summary>
        /// Time before a frame change in seconds. Use -1 to disable automatic animation.
        /// </summary>
        public float interval = 1;
        
        private int _frameIndex = -1;
        public int FrameIndex
        {
            get => _frameIndex;
            set => text = frames[_frameIndex = value];
        }

        public AnimatedText(Drawable parent = null) : base(parent)
        { }

        public AnimatedText(string pathOfAnimation, Drawable parent = null) : base(parent)
        {
            frames = LoadFromFile(pathOfAnimation);
            FrameIndex = 0;
        }

        public void StepFrame()
        {
            FrameIndex = MathHelper.Wrap(FrameIndex + 1, frames.Length);
        }

        protected override void Update()
        {
            if (interval > 0)
            {
                int numFrames = frames?.Length ?? 0;
                if (numFrames > 0)
                {
                    FrameIndex = (int)(Time.Seconds / interval) % numFrames;
                }
            }

            base.Update();
        }

        public static string[] LoadFromFile(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    int[] size = Array.ConvertAll(reader.ReadLine().Split(','), x => {
                        if (int.TryParse(x.Trim(), out int num)) return num;
                        else throw new FormatException($"Unable to parse size int from string \"{x}\"!");
                    });
                    if (size.Length != 2) throw new FormatException($"Wrong number of size values! Expected 2, got {size.Length}.");

                    int width = size[0];
                    int height = size[1];

                    string buffer = reader.ReadToEnd();
	                string[] rows = buffer.FixNewLines().Split('\n');

                    string[] frames = new string[rows.Length / height];
                    for (int i = 0; i < rows.Length; i++)
                    {
                        int frame = i / height;
                        frames[frame] = (frames[frame] ?? "") + rows[i].PadRight(width).Substring(0, width) + '\n';
                    }

                    return frames;
                }
            }
            catch (Exception e)
            {
                throw new FileLoadException("Unable to load animation!", path, e);
            }
        }
    }
}
