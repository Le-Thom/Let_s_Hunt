using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Game/Equipment")]
public class sc_Equipment : ScriptableObject
{
    public Sprite equipmentSprite;
    public string equipmentName;
    public GameObject equipmentPrefab;
    public int maxStackEquipment;
}
