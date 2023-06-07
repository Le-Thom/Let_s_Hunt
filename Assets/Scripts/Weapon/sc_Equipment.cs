using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Game/Equipment")]
public class sc_Equipment : sc_Object
{
    public equipmentType type;

    public int maxStackEquipment;
    public GameObject prefab_Object;

    public Object script_equipment;
}
