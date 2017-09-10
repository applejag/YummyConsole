using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YummyConsole
{
    public static class Time
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();
        private static float deltaTime = 0;
        private static long millisecondsPerFrame = 0;
        private static long lastFrameTime = 0;
        private static bool running = false;

        public delegate void FrameEvent();

        /// <summary>
        /// Get or set the target amount of time between each frame.
        /// To get the actual time elapsed, use <see cref="DeltaTime"/>
        /// </summary>
        public static float FramesPerSecond
        {
            get => millisecondsPerFrame * 0.001f;
            set => millisecondsPerFrame = (long) value * 1000;
        }

        /// <summary>
        /// Time elapsed since start of program in seconds.
        /// </summary>
        public static float Seconds => (float)stopwatch.Elapsed.TotalSeconds;

        /// <summary>
        /// Time elapsed since start of program in milliseconds.
        /// </summary>
        public static long Milliseconds => stopwatch.ElapsedMilliseconds;

        /// <summary>
        /// Time elapsed since last frame in seconds.
        /// </summary>
        public static float DeltaTime => deltaTime;

        /// <summary>
        /// Returns true when the frame timer currently running.
        /// <para>See: <see cref="RunFrameTimer"/>, <see cref="StopFrameTimer"/></para>
        /// </summary>
        public static bool IsRunning => running;

        public static void StopFrameTimer()
        {
            running = false;
        }

        public static void RunFrameTimer()
        {
            if (running) throw new InvalidOperationException("Frame timer is already running!");

            running = true;
            stopwatch.Start();

            while (running)
            {
                long now = stopwatch.ElapsedMilliseconds;
                long elapedTime = now - lastFrameTime;
                
                if (elapedTime >= millisecondsPerFrame)
                {
                    deltaTime = elapedTime * 0.001f;

                    Drawable.FrameCallback();

                    lastFrameTime = now;
                }
            }

            stopwatch.Stop();
        }

    }
}