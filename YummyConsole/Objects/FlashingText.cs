using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole
{
    public class FlashingText : Text
    {
        public Color[] foregrounds;
        public Color[] backgrounds;

        /// <summary>
        /// Time before a color change in seconds.
        /// </summary>
        public float interval = 1;

        protected override void Update()
        {
            int numForegrounds = foregrounds?.Length ?? 0;
            if (numForegrounds > 0)
            {
                int fgIndex = (int)(Time.Seconds / interval) % numForegrounds;
                foregroundColor = foregrounds[fgIndex];
            }

            int numbackgrounds = backgrounds?.Length ?? 0;
            if (numbackgrounds > 0)
            {
                int bgIndex = (int)(Time.Seconds / interval) % numbackgrounds;
                backgroundColor = backgrounds[bgIndex];
            }
        }

        //public override void Draw()
        //{
        //    base.Draw();
        //    Yummy.CursorX = 30;
        //    Yummy.CursorY = 20;
        //    Yummy.Write(Time.Seconds.ToString());
        //}
    }
}
