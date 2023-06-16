using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "Game/Object")]
public class sc_Object : ScriptableObject
{
    public Sprite objectSprite;
    public Material objectMaterial;
    public string objectName;
}
