﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public enum GameState
{
    MAIN_MENU, GAMEPLAY, PAUSE
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject fadeGameObject;
    public GameState currentState;
    private int idCurrentScene;
    private FadeController _FadeController;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        //garante que não tenha erro caso teste o game começando em uma cena diferente da intro
        if(fadeGameObject != null)
        {
            //garante que o fade esteja ativado no começo do game
            if(fadeGameObject.activeSelf == false) {fadeGameObject.SetActive(true); }
            _FadeController = FindObjectOfType(typeof(FadeController)) as FadeController;
        }
        else
        {
            Debug.LogWarning("Voce está sem o fade, funções de passar entre cenas nao irão funcionar");
        }
    }

    private void Update() {

        //==== DEBUG ====
        if(Input.GetKeyDown(KeyCode.N))
        {   
            NextScene();
        }
    }

    #region SCENEMANAGEMENT
    public void NextScene()
    {
       _FadeController.NextScene();
    }

    public void LoadSaveScene()
    {
        _FadeController.LoadSaveScene();
    }

    #endregion

    public void ChangeGameState(GameState newState)
    {
        currentState = newState;
    }   
}
