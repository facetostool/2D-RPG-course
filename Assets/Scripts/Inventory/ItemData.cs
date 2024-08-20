using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Material,
    Equipment,
}

[CreateAssetMenu(fileName = "New item data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public string guid;
    
    [Range(0, 100)] public int dropChance = 100;
    
    void OnValidate()
    {
        #if UNITY_EDITOR
            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            this.guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);
        #endif
    }
}
