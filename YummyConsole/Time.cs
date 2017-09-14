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
        private static readonly Stopwatch _stopwatch = new Stopwatch();
        private static float _deltaTime = 0;
        private static long _millisecondsPerFrame = 0;
        private static long _lastFrameTime = 0;
        private static bool _running = false;

        /// <summary>
        /// Get or set the target amount of time between each frame.
        /// To get the actual time elapsed, use <see cref="DeltaTime"/>
        /// </summary>
        public static float FramesPerSecond
        {
            get => _millisecondsPerFrame * 0.001f;
            set => _millisecondsPerFrame = (long) value * 1000;
        }

        /// <summary>
        /// Time elapsed since start of program in seconds.
        /// </summary>
        public static float Seconds => (float)_stopwatch.Elapsed.TotalSeconds;

        /// <summary>
        /// Time elapsed since start of program in milliseconds.
        /// </summary>
        public static long Milliseconds => _stopwatch.ElapsedMilliseconds;

        /// <summary>
        /// Time elapsed since last frame in seconds.
        /// </summary>
        public static float DeltaTime => _deltaTime;

        /// <summary>
        /// Returns true when the frame timer currently running.
        /// <para>See: <see cref="RunFrameTimer"/>, <see cref="StopFrameTimer"/></para>
        /// </summary>
        public static bool IsRunning => _running;

        public static void StopFrameTimer()
        {
            _running = false;
        }

        public static async Task RunFrameTimer()
        {
	        await Task.Run(() => {

				// Initialize
				_running = true;
				_stopwatch.Restart();
		        _lastFrameTime = _stopwatch.ElapsedMilliseconds;
				Yummy.ValidateFileHandler();

				while (_running)
				{
					long now = _stopwatch.ElapsedMilliseconds;
					long elapedTime = now - _lastFrameTime;
                
					if (elapedTime >= _millisecondsPerFrame)
					{
						_deltaTime = elapedTime * 0.001f;

						Drawable.FrameCallback();

						_lastFrameTime = now;
					}
				}

				_stopwatch.Stop();
	        });
		}

	    public static void RunSingleFrame()
	    {
		    Yummy.ValidateFileHandler();

			if (!_stopwatch.IsRunning) _stopwatch.Restart();

		    long now = _stopwatch.ElapsedMilliseconds;
		    long elapedTime = now - _lastFrameTime;
		    _deltaTime = elapedTime * 0.001f;

		    Drawable.FrameCallback();

			_lastFrameTime = now;
	    }

	}
}