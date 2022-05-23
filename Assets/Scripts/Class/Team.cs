using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Team", menuName = "Team")]
public class Team : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite LogoCircle;
    public Sprite LogoSquare;
    public Color[] Colors = new Color[2];
}
