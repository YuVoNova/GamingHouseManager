using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Game", menuName = "Game")]
public class Game : ScriptableObject
{
    public int ID;
    public string Name;
    public Image Logo;
    public Color[] Colors = new Color[2];
}
