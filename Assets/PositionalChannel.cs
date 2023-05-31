using UnityEngine;
using VivoxUnity;

class PositionalChannel : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    private float _nextPosUpdate = 0;
    private LoginCredentials _loginCredentials;
    public bool isActif => LoginCredentials.Instance.channelConnected;
    private void Start()
    {
        _loginCredentials = LoginCredentials.Instance;
    }
    void Update()
    {
        if (isActif && Time.time > _nextPosUpdate)
        {
            _loginCredentials.Update3DPosition(playerPosition, playerPosition);
            _nextPosUpdate += 0.3f; // Only update after 0.3 or more seconds
        }
    }
}
