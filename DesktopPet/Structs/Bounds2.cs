using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPet.Structs
{
    internal struct Bounds2
    {
        public Vector2 position;
        public Size2 size;
        public Bounds2(double x, double y,double width, double height)
        {
            position = new Vector2(x,y);
            size = new Size2(width,height);
        }
        public static bool ClampsToLeft(Bounds2 inner, Bounds2 outer) => inner.position.x < outer.position.x;
        public static bool ClampsToRight(Bounds2 inner, Bounds2 outer) => 
            inner.position.x + inner.size.width > outer.position.x + outer.size.width;
        public static bool ClampsToTop(Bounds2 inner, Bounds2 outer) => inner.position.y < outer.position.y;
        public static bool ClampsToBottom(Bounds2 inner, Bounds2 outer) => 
            inner.position.y + inner.size.height > outer.position.y + outer.size.height;
        public static Bounds2 StickToLeft(Bounds2 inner, Bounds2 outer)
        {
            inner.position.x = outer.position.x;
            return inner;
        }
        public static Bounds2 StickToRight(Bounds2 inner, Bounds2 outer)
        {
            inner.position.x = outer.position.x + outer.size.width - inner.size.width;
            return inner;
        }
        public static Bounds2 StickToTop(Bounds2 inner, Bounds2 outer)
        {
            inner.position.y = outer.position.y;
            return inner;
        }
        public static Bounds2 StickToBottom(Bounds2 inner, Bounds2 outer)
        {
            inner.position.y = outer.position.y + outer.size.height - inner.size.height;
            return inner;
        }
    }
}
