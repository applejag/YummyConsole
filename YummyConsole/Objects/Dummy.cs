using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole
{
    public class Dummy : Drawable
    {
        public Action OnUpdate;
        public Action OnDraw;

        protected override void Update()
        {
            OnUpdate?.Invoke();
        }

        protected override void Draw()
        {
            OnDraw?.Invoke();
        }
    }
}
