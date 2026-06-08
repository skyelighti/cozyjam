using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using System;

public enum ActionMap { Player, UI, Dialouge}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance  { get; private set; }
    public InputSystem_Actions playerInputActions;
    public event EventHandler OnActionMapChanged;
    //set player velocity to 0 when action map is changed

    //Singleton pattern
    private void Awake() {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        playerInputActions = new InputSystem_Actions(); 
        playerInputActions.Enable();  
    }

    public void SwapMap(ActionMap map)
    {
        foreach (InputActionMap m in playerInputActions.asset.actionMaps)
        {
            m.Disable();
        }
        playerInputActions.asset.FindActionMap(map.ToString()).Enable();
        OnActionMapChanged?.Invoke(this, EventArgs.Empty);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
    }
}
