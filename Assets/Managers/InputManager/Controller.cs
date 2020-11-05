using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private PlayerInput _playerInput;
    public Action onLeft = () => { };


    public  void Initialize()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        Debug.Log("awake");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Destroy(gameObject);
        }
    }

    public void On(string actionName, Action<InputAction.CallbackContext> callback)
    {
        foreach (InputAction action in _playerInput.actions)
        {
            if(action.name == actionName)
            {
                action.performed += callback;
            }
        }
    }

    public void Remove(string actionName, Action<InputAction.CallbackContext> callback)
    {
        foreach (InputAction action in _playerInput.actions)
        {
            if (action.name == actionName)
            {
                action.performed -= callback;
            }
        }
    }
}
