using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : Singleton<HealthBarManager>
{
    [SerializeField] private HealthBar healthBar1, healthBar2, healthBar3, healthBar4;

    private void OnEnable()
    {
        List<int> _indexPlayers = new() { 1, 2, 3, 4 };

        // get player owner index and set it to healthbar1
        int _indexOwner = ScS_PlayerData.Instance.monitor.index;
        healthBar1.SetIndexPlayer(ScS_PlayerData.Instance.monitor.index);
        _indexPlayers.Remove(_indexOwner);

        healthBar2.SetIndexPlayer(_indexPlayers[0]);
        _indexPlayers.Remove(_indexPlayers[0]);

        healthBar3.SetIndexPlayer(_indexPlayers[0]);
        _indexPlayers.Remove(_indexPlayers[0]);

        healthBar4.SetIndexPlayer(_indexPlayers[0]);
        _indexPlayers.Remove(_indexPlayers[0]);
    }

    public void ChangeHealthBar(int indexPlayer, int value) 
    {
        HealthBar _healthBar = GetHealthFromIndex(indexPlayer);
        int newHp = Mathf.Clamp(_healthBar.GetHpValue() + value, 0, 10);
        _healthBar.ChangeValueHealthBar(newHp);
    }

    private HealthBar GetHealthFromIndex(int indexPlayer)
    {
        if (healthBar1.GetIndexPlayer() == indexPlayer) return healthBar1;
        else if (healthBar2.GetIndexPlayer() == indexPlayer) return healthBar2;
        else if (healthBar3.GetIndexPlayer() == indexPlayer) return healthBar3;
        else if (healthBar4.GetIndexPlayer() == indexPlayer) return healthBar4;
        else return null;
    }
}
