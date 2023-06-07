using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using FMODUnity;
using DG.Tweening;

public class UI_Message_Manager : Singleton<UI_Message_Manager>
{
    [SerializeField] private GameObject messageParent;
    [SerializeField] private TextMeshProUGUI messagePrefab;
    public async void ShowMessage(Color color, string text, string fmodEventName = null)
    {
        GameObject newMessage = Instantiate(messagePrefab.gameObject, messageParent.transform);
        TextMeshProUGUI messageTMPro = newMessage.GetComponent<TextMeshProUGUI>();

        messageTMPro.color = color;
        messageTMPro.text = text;

        await messageTMPro.DOColor(new Color( messageTMPro.color.r, messageTMPro.color.g, messageTMPro.color.b, 0), 5).AsyncWaitForCompletion();
        Destroy(newMessage);
    }
}
