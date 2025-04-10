using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Base Data")]
    public string ItemID;
    public string ItemName;
    public Sprite ItemIcon;
    public string ItemDescription;
}
