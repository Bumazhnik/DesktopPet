using DesktopPet.Characters;

namespace DesktopPet;

internal class CharacterConfig
{
    public Dictionary<CharacterState, string> States { get; set; } = new();
    public List<string> FavoriteWords { get; set; } = new();
    public bool BackSpaceDeletes { get; set; }
    public int GroundHeight { get; set; } = 40;
    public void SimplifyWords()
    {
        for (var i = 0; i < FavoriteWords.Count; i++)
        {
            string word = FavoriteWords[i].ToLower().FromRussian().ToKeys();
            FavoriteWords[i] = word;
        }
    }
}
