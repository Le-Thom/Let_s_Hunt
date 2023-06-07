using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using VivoxUnity;

class PositionalChannel : NetworkBehaviour
{
    [SerializeField] private Transform playerPosition;
    private float _nextPosUpdate = 0;
    private LoginCredentials _loginCredentials;
    public bool isActif = false;
    public bool toggleMute = false;
    public bool forcedMute = false;
    private Controls _inputs;
    [SerializeField] private LoginCredentials vivoxManager;
    [SerializeField] private FMODUnity.EventReference audioToggleMute;
    [SerializeField] private Toggle vocToggle;
    public void _Start()
    {
        _inputs = new();
        _inputs.Enable();
        _loginCredentials = LoginCredentials.Instance;
        isActif = true;
    }

    void Update()
    {
        if (IsOwner && isActif && Time.time > _nextPosUpdate)
        {
            _loginCredentials.Update3DPosition(playerPosition, playerPosition);
            _nextPosUpdate += 0.3f; // Only update after 0.3 or more seconds
        }
    }

    public void ChangeMute()
    {
        if (forcedMute) return;

        toggleMute = !toggleMute;
        vocToggle.isOn = !toggleMute;
        vivoxManager.client.AudioInputDevices.Muted = toggleMute;
    }

    public void ForceMute()
    {
        forcedMute = true;
        vocToggle.isOn = false;
        vivoxManager.client.AudioInputDevices.Muted = false;
    }
    public void UnforceMute()
    {
        forcedMute = false;
        vocToggle.isOn = true;
        vivoxManager.client.AudioInputDevices.Muted = toggleMute;
    }
}
