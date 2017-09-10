using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole
{
    public class Trail : Drawable
    {
        private List<PointInTime> points = new List<PointInTime>();

        public double TimeToLive { get; set; }
        public Color Color { get; set; }
        public bool SelfDestruct { get; set; } = false;

        private Point oldPosition;

        public Trail(double time = 5, Color color = Color.LIGHT_CYAN) : base(null)
        {
            TimeToLive = time;
            Color = color;
        }

        protected override void Draw()
        {
            int numPoints = points?.Count ?? 0;
            if (numPoints > 1)
            {
                PointInTime last = points[0];
                Yummy.BackgroundColor = Color;

                for (int i = 1; i < numPoints; i++)
                {
                    PointInTime point = points[i];
                    Yummy.FillLine(last.position.x, last.position.y, point.position.x, point.position.y, ' ');
                    last = point;
                }
            }
            else
            {
                Yummy.BackgroundColor = Color;
                Yummy.FillPoint(ApproxPosition, ' ');
            }
        }

        protected override void Update()
        {
            Point approx = ApproxPosition;

            double now = Time.Seconds;

            // Add new point if moved
            if (approx.x != oldPosition.x || approx.y != oldPosition.y)
                points.Add(new PointInTime { position = approx, timestamp = now });

            // Remove old points
            points.RemoveAll(p => now - p.timestamp > TimeToLive);

            // Kys
            if (SelfDestruct && points.Count == 0)
                Destroy();

            oldPosition = approx;
        }

        private struct PointInTime
        {
            public Point position;
            public double timestamp;
        }
    }
}
