using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ListEquipment", menuName ="Data/Equipment list")]
public class SC_sc_Object : ScriptableObject
{
    public List<sc_Equipment> objects;
}
