using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole
{
    public abstract class Moving : Drawable
    {
        public const float GRAVITY = 9.81f;

        public Vector2 Velocity { get; set; }

        protected override void Update()
        {
            LocalPosition += Velocity * Time.DeltaTime;
        }
    }
}
