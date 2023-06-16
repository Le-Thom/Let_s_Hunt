using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using VivoxUnity;

public class DevicesSelector : MonoBehaviour
{
    [SerializeField] private VivoxUnity.IAudioDevices currentDevice;
    [SerializeField] private VivoxUnity.IReadOnlyDictionary<string, VivoxUnity.IAudioDevice> allDevices;
    [SerializeField] private TMP_Dropdown dropdown;

    private IEnumerator OnEnable()
    {
        yield return new WaitForSeconds(0.01f);
        allDevices = LoginCredentials.Instance.GetAllDevices();
        currentDevice = LoginCredentials.Instance.GetCurrentDevices();
        foreach (var device in allDevices)
        {
            Debug.Log($"{device}, {device.Name}");
        }

        SetDropDown();
        dropdown.onValueChanged.AddListener(ChangeDevice);
    }

    private void SetDropDown()
    {
        dropdown.ClearOptions();

        List<string> _devices = new List<string>();
        int _currentDeviceId = 0;
        int i = 0;
        foreach (var device in allDevices)
        {
            string _nameDevice = device.Name;

            if (_nameDevice != "No Device")
            {

                _devices.Add(_nameDevice);

                if (device.Name == currentDevice.ActiveDevice.Name)
                { _currentDeviceId = i; }

                i++;
            }
        }

        dropdown.AddOptions(_devices);
        dropdown.value = _currentDeviceId;
        dropdown.RefreshShownValue();
    }

    public void ChangeDevice(int deviceIndex)
    {
        VivoxUnity.IAudioDevice _newDevice;
        int i = 0;
        foreach (var device in allDevices)
        {
            if (i == deviceIndex)
            {
                Debug.Log($"{device.Name}");
                _newDevice = device;
                LoginCredentials.Instance.ChangeInputDevice(_newDevice);
                break;
            }

            i++;
        }
    }
}

