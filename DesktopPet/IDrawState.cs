using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopPet.Characters;

namespace DesktopPet;
internal interface IDrawState
{
    void DrawState(CharacterState state);
}

