using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayIngredients.Actions;
using GameplayIngredients;

[RequireComponent(typeof(FullScreenFadeAction))]
[RequireComponent(typeof(GameLevelLoadAction))]
public class Scene_Manager : Singleton<Scene_Manager>
{
    //========
    //VARIABLES
    //========

    private FullScreenFadeAction fadeAction;
    private GameLevelLoadAction loadAction;

    //========
    //MONOBEHAVIOUR
    //========
    private void Awake()
    {
        fadeAction = gameObject.GetComponent<FullScreenFadeAction>();
        loadAction = gameObject.GetComponent<GameLevelLoadAction>();

        if(fadeAction.OnComplete.Length <= 0)
        {
            fadeAction.OnComplete = new Callable[] { loadAction };
        }
    }
    //========
    //FONCTION
    //========

    public void ReloadMainScene()
    {
        fadeAction.Execute();
    }
}
