using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AirdropManager : NetworkBehaviour
{
    [SerializeField] private List<SpawnAirdrop> airdropList = new();
    [SerializeField] private int airdropCount = 2;
    [SerializeField] private NetworkList<int> airdropIndex = new NetworkList<int>();

    public void CallAirdrop(int index)
    {
        List<SpawnAirdrop> _list = new();

        int y = airdropCount;
        for (int i = 0; i < y; i++)
        {
            int rng = Random.Range(0, airdropList.Count);

            if (!_list.Contains(airdropList[rng]))
            {
                _list.Add(airdropList[rng]);
                airdropIndex.Add(rng);
            }
            else 
            {
                y++;

                if (y >= 100) break;
            }
        }

        CallAirdropServerRpc(index);
    }
    
    [ServerRpc(RequireOwnership = true)]
    public void CallAirdropServerRpc(int index)
    {
        for (int i = 0;i < airdropIndex.Count;i++)
        {
            airdropList[airdropIndex[i]].Spawn_AirdropServerRpc(index);
        }
        airdropIndex.Clear();
        airdropIndex = new();
    }
}
