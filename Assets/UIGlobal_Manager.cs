using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayIngredients;

public class UIGlobal_Manager : Singleton<UIGlobal_Manager>
{
    //
    //
    //

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private GameObject monsterUI;
    [SerializeField] private GameObject soldierUI;
    [SerializeField] private Callable screenTransition;


    //
    //
    //
    public void SwitchUIState(UIState newState)
    {
        DeactivateAllUI();
        screenTransition.Execute();
        switch (newState)
        {
            case UIState.Monster:
                monsterUI.SetActive(true);
                break;
            case UIState.Soldier:
                soldierUI.SetActive(true);
                break;
        }
    }
    private void DeactivateAllUI()
    {
        mainMenu.SetActive(false);
        lobbyUI.SetActive(false);
        monsterUI.SetActive(false);
        soldierUI.SetActive(false);
    }
}
public enum UIState
{
    Monster, Soldier 
}
