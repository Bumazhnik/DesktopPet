namespace DesktopPet;
internal interface ICharacter
{
    void Update(GameFrame frame);
    CharacterState State { get; }
    Vector2Int Position { get; }
    Size2Int Size { get; }

    void OnMouseDown(Vector2Int cursor);
    void OnMouseUp();
}

