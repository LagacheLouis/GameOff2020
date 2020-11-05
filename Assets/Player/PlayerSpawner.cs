using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public bool spawnOnStart;
    public bool spawnOnJoin;
    public bool allowDuplicates;
    public ControllerTarget prefab;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnOnStart)
        {
            SpawnAll();
        }
        ControllerManager.Inst.onPlayerJoined += Spawn;
    }

    public void SpawnAll()
    {
        foreach (Controller controller in ControllerManager.Inst.Players)
        {
            Spawn(controller);
        }
    }

    public void Spawn(Controller controller)
    {
        Debug.Log("spawn " + controller);
        ControllerTarget player = Instantiate(prefab, transform.position, Quaternion.identity);
        player.Link(controller);
    }
}
