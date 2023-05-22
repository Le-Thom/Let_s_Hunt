using GameplayIngredients.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [SerializeField] private int _nbInInventaire;
    private int nbInInventaire
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
            if (value == null) return;

            _equipment = value;
            imageEquipment.sprite = value.equipmentSprite;
            textEquipment.text = value.equipmentName;
        }
    }

    [SerializeField] private Tps_PlayerController playerController;
    [SerializeField] private Image imageEquipment;
    [SerializeField] private TMP_Text textEquipment;
    [SerializeField] private TMP_Text textNb;

    public bool onSelected;

    [SerializeField] private Sprite nullEquipment;

    [SerializeField] private Equipment otherEquipment;

    public void SetOnSelected()
    {
        if (equipment == null) return; 
        onSelected = !onSelected;
        if (otherEquipment.onSelected) otherEquipment.onSelected = false;
    }

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

        return nbInInventaire - nb;
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

    public void Drop()
    {
        if (onSelected)
        {
            // drop 1 obj on the ground.
            ItemUsed();
        }
    }

    private void _Drop() { }
    private void ItemUsed() => nbInInventaire--;
}
