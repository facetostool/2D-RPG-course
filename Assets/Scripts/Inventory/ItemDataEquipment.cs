using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Amulet,
    Armor,
    Weapon,
    Flask
}

[CreateAssetMenu(fileName = "New item data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;
}
