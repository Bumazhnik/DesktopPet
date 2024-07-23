using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPet
{
    internal class Character : ICharacter
    {
        public CharacterState State => CharacterState.Idle;

        public Vector2Int Position { get; set; }

        public Size2Int Size { get; set; }

        public void Update(GameFrame frame)
        {
            if (dragging)
            {
                Vector2Int newPos;
                newPos.x = offsetX + frame.cursor.x;
                newPos.y = offsetY + frame.cursor.y;
                Position = newPos;
            }
        }
        int offsetX;
        int offsetY;
        bool dragging;

        public Character(Size2Int size)
        {
            Size = size;
        }

        public void OnMouseDown(Vector2Int cursor)
        {
            dragging = true;
            offsetX = Position.x - cursor.x;
            offsetY = Position.y - cursor.y;
        }
        public void OnMouseUp()
        {
            dragging = false;
        }
    }
}
