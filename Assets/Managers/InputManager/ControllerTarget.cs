using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerTarget : MonoBehaviour
{
    protected Controller _controller;

    public void Link(Controller controller)
    {
        _controller = controller;
        Initialize();
    }

    protected abstract void Initialize();
}
