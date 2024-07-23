using DesktopPet.Structs;

namespace DesktopPet.Characters;
internal interface ICharacter
{
    void Update(GameFrame frame);
    CharacterState State { get; }
    Vector2 Position { get; }
    Size2 Size { get; }

    void OnMouseDown(Vector2 cursor);
    void OnMouseUp();
}

