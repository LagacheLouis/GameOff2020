using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ControllerTarget
{
    public float force;

    private Rigidbody _rbody;
    private Vector2 _iMove;

    protected override void Initialize()
    {
        _controller.On("Use", (InputAction.CallbackContext ctx) => { Debug.Log("Use " + ctx.ReadValue<float>()); });
        _controller.On("Move", (InputAction.CallbackContext ctx) => { _iMove = ctx.ReadValue<Vector2>(); });
    }

    void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        _rbody.constraints = RigidbodyConstraints.FreezePositionZ;
        _rbody.useGravity = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rbody.AddForce(_iMove * force);
    }

 

}
