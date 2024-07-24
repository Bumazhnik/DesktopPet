using DesktopPet.Structs;

namespace DesktopPet.Characters;
internal interface ICharacter
{
    void Update(GameFrame frame);
    CharacterState State { get; }
    Bounds2 CharacterBounds { get; }

    void OnMouseDown(Vector2 cursor);
    void OnMouseUp();
    void MakeHappy();
}

