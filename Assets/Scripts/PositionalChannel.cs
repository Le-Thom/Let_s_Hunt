using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using VivoxUnity;

public class PositionalChannel : MonoBehaviour
{
    private float _nextPosUpdate = 0;
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
        isActif = true;
        _inputs.VOIP.ToggleMute.started += ctx => ChangeMute();
    }

    void Update()
    {
        if (isActif && Time.time > _nextPosUpdate)
        {
            vivoxManager.Update3DPosition(transform, transform);
            _nextPosUpdate += 0.3f; // Only update after 0.3 or more seconds
        }
    }

    public void ChangeMute()
    {
        if (forcedMute) return;

        FMODUnity.RuntimeManager.PlayOneShot(audioToggleMute);

        toggleMute = !toggleMute;
        vocToggle.isOn = !toggleMute;
        vivoxManager.client.AudioInputDevices.Muted = toggleMute;
    }

    public void ForceMute()
    {
        forcedMute = true;
        vocToggle.isOn = false;
        vivoxManager.client.AudioInputDevices.Muted = true;
    }
    public void UnforceMute()
    {
        forcedMute = false;
        vocToggle.isOn = true;
        vivoxManager.client.AudioInputDevices.Muted = toggleMute;
    }
}
