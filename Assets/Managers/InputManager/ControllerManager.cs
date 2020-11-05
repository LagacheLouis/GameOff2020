using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shaker;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(PlayerInputManager))]
public class ControllerManager : Singleton<ControllerManager>
{
    private PlayerInputManager _playerInputManager;
    private List<Controller> _players = new List<Controller>();

    public Controller[] Players { get { return _players.ToArray(); }}

    public Action<Controller> onPlayerJoined = (Controller controller) => { Debug.Log("Player joined"); };

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        _playerInputManager.onPlayerJoined += PlayerJoined;
    }

    private void PlayerJoined(PlayerInput input)
    {
        input.transform.SetParent(transform, true);
        Controller controller = input.GetComponent<Controller>();
        controller.Initialize();
        _players.Add(controller);
        onPlayerJoined(controller);
    }

    public void KickPlayer(Controller player)
    {
        _players.Remove(player);
        Destroy(player.gameObject);
    }
}
