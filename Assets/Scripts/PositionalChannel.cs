using Unity.Netcode;
using UnityEngine;
using VivoxUnity;

class PositionalChannel : NetworkBehaviour
{
    [SerializeField] private Transform playerPosition;
    private float _nextPosUpdate = 0;
    private LoginCredentials _loginCredentials;
    public bool isActif = false;
    public void _Start()
    {
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
}
