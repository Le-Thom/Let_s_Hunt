using GameplayIngredients.Events;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [SerializeField] private int _nbInInventaire;
    public int nbInInventaire
    {
        get { return _nbInInventaire; }
        set
        {
            _nbInInventaire = value;
            textNb.text = value.ToString();

            if (value == 0 ) EquipmentNbIs0();
        }
    }

    [SerializeField] private sc_Equipment _equipment;
    private sc_Equipment equipment
    {
        get { return _equipment; }
        set
        {
            _equipment = value;

            if (value == null) return;

            imageEquipment.sprite = value.objectSprite;
            textEquipment.text = value.objectName;
        }
    }

    [SerializeField] private Tps_PlayerController playerController;
    [SerializeField] private Image imageEquipment;
    [SerializeField] private TMP_Text textEquipment;
    [SerializeField] private TMP_Text textNb;

    private bool onSelected;

    [SerializeField] private Sprite nullEquipment;

    [SerializeField] private Equipment otherEquipment;

    public void SetOnSelected()
    {
        if (equipment == null) return; 
        onSelected = !onSelected;
        if (otherEquipment.onSelected) otherEquipment.onSelected = false;
    }
    public bool GetOnSelected() { return onSelected; }

    /// <summary>
    /// If there is nothing in equipment.
    /// </summary>
    /// <param name="_equipment"></param>
    /// <param name="nb"></param>
    /// <returns></returns>
    public int AddEquipment(sc_Equipment _equipment, int nb)
    {
        equipment = _equipment;
        nbInInventaire = nb;
        nbInInventaire = Mathf.Clamp(nbInInventaire, 1, equipment.maxStackEquipment);

        return nb - nbInInventaire;
    }

    /// <summary>
    /// If equipment is already the same.
    /// </summary>
    /// <param name="nb"></param>
    /// <returns></returns>
    public int AddEquipment(int nb)
    {
        nbInInventaire += nb;
        int _nb = nbInInventaire;
        nbInInventaire = Mathf.Clamp(nbInInventaire, 1, equipment.maxStackEquipment);

        return _nb - nbInInventaire;
    }

    public sc_Equipment GetEquipment()
    {
        return equipment ?? null;
    }

    /// <summary>
    /// If nb of equipment is 0.
    /// </summary>
    public void EquipmentNbIs0()
    {
        onSelected = false;
        equipment = null;
        imageEquipment.sprite = nullEquipment;
        textEquipment.text = "";
        textNb.text = "";
    }

    private void Start()
    {
        EquipmentNbIs0();
    }

    private void Update()
    {
        if (onSelected && transform.localScale != Vector3.one * 1.2f)
            transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one * 1.2f, 1);
        else if (!onSelected && transform.localScale != Vector3.one)
            transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one, 1);
    }

    public void Drop(Tps_PlayerController player)
    {
        if (onSelected)
        {
            GameObject _drop = Instantiate(Resources.Load<GameObject>("DropObject"), player.transform.position, Quaternion.Euler(0,0,0));
            _drop.GetComponentInChildren<ObjectDrop>().SetUpObj(equipment, player.GetComponentInChildren<HunterHitCollider>());

            float _directionX = Mathf.Clamp(player.directionLook.x, -150, 150);
            float _directionY = Mathf.Clamp(player.directionLook.y, -150, 150);

            _drop.GetComponent<Rigidbody>().velocity += new Vector3(-_directionX, 0, -_directionY) * Time.deltaTime + Vector3.up * 4;


            ItemUsed();
        }
    }

    private void ItemUsed() => nbInInventaire = nbInInventaire - 1;

    public void UseItem(Tps_PlayerController player)
    {
        SC_UseItem _script = player.gameObject.AddComponent(System.Type.GetType(equipment.script_equipment.name)) as SC_UseItem;

        Debug.Log(_script.GetType());
        if (_script.GetType() == typeof(SC_UI_MedKit) && Tps_PlayerController.Instance.GetCurrentState() == AkarisuMD.Player.StateId.HEALING) 
        {
            Debug.Log("Bloque");
            return;
        }


        SC_sc_Object _sc_sc_equipment = Resources.Load<SC_sc_Object>("Equipment/");
        int equipmentIndex = 0;
        for (int i = 0; i < _sc_sc_equipment.objects.Count; i++)
        {
            if (equipment == _sc_sc_equipment.objects[i])
            {
                equipmentIndex = i;
                break;
            }
        }

        _script.UseItem(player.transform.position, equipmentIndex, player.directionLook);
        ItemUsed();
    }
}
