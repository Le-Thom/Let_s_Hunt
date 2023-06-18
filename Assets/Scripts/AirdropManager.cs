using GameplayIngredients.Rigs;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AirdropManager : MonoBehaviour
{
    [SerializeField] private List<SpawnAirdrop> airdropList = new();
    [SerializeField] private int airdropCount = 2;
    [SerializeField] private List<int> airdropIndex = new();

    public void CallAirdrop(int index)
    {
        List<SpawnAirdrop> _list = new();
        int y = airdropCount;
        airdropIndex = new();
        for (int i = 0; i < y; i++)
        {
            int rng = Random.Range(0, airdropList.Count);

            if (!_list.Contains(airdropList[rng]))
            {
                _list.Add(airdropList[rng]);
                airdropIndex.Add(i);
            }
            else
            {
                y++;

                if (y >= 100) break;
            }
        }

        for (int i = 0; i < airdropIndex.Count; i++)
        {
            airdropList[airdropIndex[i]].Spawn_AirdropServerRpc(index);
        }
    }
}
