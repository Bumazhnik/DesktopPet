using DesktopPet.Characters;

namespace DesktopPet;

internal class CharacterConfig
{
    public Dictionary<CharacterState, string> States { get; set; } = new();
}
