using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using DG.Tweening;

public class Revive : InteractableObject
{
    
    [SerializeField] private GameObject onCanPickUp;
    [SerializeField] private Tps_PlayerController playerController;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private float timerRevive = 1f;
    [SerializeField] private Slider slider;
    private Coroutine coroutine;
    private bool revive = false;
    void OnEnable()
    {
        isInteractable = false;
        sphereCollider.enabled = false;
        onCanPickUp.SetActive(false);
        slider.gameObject.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)] 
    public void IsActiveServerRpc(bool value)
    {
        if (value)
            IsActiveClientRpc();
        else
            IsInactiveClientRpc();
    }

    [ClientRpc]
    public void IsActiveClientRpc()
    {
        Debug.LogError("no");
        UI_Message_Manager.Instance.ShowMessage(Color.red, "Player has died");
        isInteractable = true;
        sphereCollider.enabled = true;
        //playerController.GetComponentInParent<Tps_PlayerController>();
    }

    [ClientRpc]
    public void IsInactiveClientRpc()
    {
        isInteractable = false;
        sphereCollider.enabled = false;

        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);

    }

    public override async void Interact()
    {
        UI_Message_Manager.Instance.ShowMessage(Color.red, "Starting to Revive");

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(CheckRevive());

        slider.gameObject.SetActive(true);
        //_ReviveServerRpc();

       slider.maxValue = timerRevive;
       await DOVirtual.Float(slider.value, timerRevive, timerRevive, v => slider.value = v).AsyncWaitForCompletion();

       _ReviveServerRpc();

        slider.value = 0;
        slider.gameObject.SetActive(false);

        revive = false;
    }

    private IEnumerator CheckRevive()
    {
        slider.gameObject.SetActive(true);
        _GetReviveClientRpc();
        while (playerController.isInteracting)
        {
            slider.value += 1 / timerRevive * Time.deltaTime;
            yield return Time.deltaTime;

            if (slider.value >= slider.maxValue)
            {
                _ReviveServerRpc();
                revive = true;
                break;
            }
        }
        slider.value = 0;
        slider.gameObject.SetActive(false);
        Tps_PlayerController.Instance.ChangeStateToIdle();

        if (!revive) _StopGetReviveClientRpc();
        revive = false;
    }

    private void OnDisable()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    [ServerRpc(RequireOwnership = false)]
    public void _ReviveServerRpc()
    {
        UI_Message_Manager.Instance.ShowMessage(Color.red, "Success Player is Being Revied");
        _InteractClientRpc();
    }

    [ClientRpc]
    public void _InteractClientRpc()
    {
        if (playerController == null) 
        {
            Debug.LogError("Yes bro");
            return; 
        }


        Tps_PlayerController.Instance.hunterAnimationController.PlayAudioReviveClientRpc(transform.position);
        playerController.Revive();
        UI_Message_Manager.Instance.ShowMessage(Color.red, "You are Revived");
        if (IsOwner)
        {
            UI_Message_Manager.Instance.ShowMessage(Color.red, "You are Revived  Z2");
            Debug.LogError("No bro");
            Tps_PlayerController tps_PlayerController = FindAnyObjectByType<Tps_PlayerController>();
            tps_PlayerController.Revive();
        }
    }
    [ClientRpc]
    public void _GetReviveClientRpc()
    {
        if (playerController == null) return;

        //playerController.ReviveAnim();
    }
    [ClientRpc]
    public void _StopGetReviveClientRpc()
    {
        if (playerController == null) return;

        Tps_PlayerController.Instance.ChangeStateToIdle();
        //playerController.StopReviveAnim();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isInteractable) return;

        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (!_hunterCollider.IsOwner) return;

            Tps_PlayerController.Instance.interactableObjects.Add(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (!_hunterCollider.IsOwner) return;

            if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
                Tps_PlayerController.Instance.interactableObjects.Remove(this);

        }
    }
}
