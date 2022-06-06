using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Game", menuName = "Game")]
public class Game : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite FullLogo;
    public Sprite Logo;
    public Sprite LogoType;
    public Material GroundMaterial;
    public Material WallMaterial;
}
