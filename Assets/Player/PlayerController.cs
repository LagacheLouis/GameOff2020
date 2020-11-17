using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ControllerTarget
{
    public float speed;
    public float drag;
    public SafetyRope rope;
    public float pushForce;



    private Rigidbody _rbody;
    private Vector3 _iMove;
    private Rigidbody _ship;
    private bool _iGrab;
    private bool _iUse;
    private bool _iUseOnce;
    private Rigidbody _grabbedRb;

    protected override void Initialize()
    {
        _controller.On("Grab", (InputAction.CallbackContext ctx) => {  _iGrab = ctx.ReadValue<float>()>0; });
        _controller.On("Use", (InputAction.CallbackContext ctx) => { _iUse = ctx.ReadValue<float>() > 0; _iUseOnce = _iUse; });
        _controller.On("Move", (InputAction.CallbackContext ctx) => { _iMove = ctx.ReadValue<Vector2>(); });
    }

    void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        _rbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        _rbody.useGravity = false;
    }

    void UpdateShip()
    {
        if (!_ship)
        {
            _ship = GameObject.FindGameObjectWithTag("SHIP")?.GetComponentInParent<Rigidbody>();
            rope.connectedBody = _ship;
        }
    }

    void Update()
    {
        Use();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateShip();
        Grab();
        Movement();
    }

    void LateUpdate()
    {
        _iUseOnce = false;
    }

    void Movement()
    {
        if (!_grabbedRb)
        {
            Vector3 velocity = _rbody.velocity - _ship.velocity;
            Vector3 direction = _iMove;
            Vector3 velocityDelta = (direction * speed - velocity) * drag * direction.magnitude;
            _rbody.AddForce(velocityDelta, ForceMode.VelocityChange);
        }
    }

    void Grab()
    {
        if (_iGrab)
        {
            _grabbedRb = FindInteractionTarget<Rigidbody>();
            if (_grabbedRb)
            {
                // _rbody.isKinematic = true; 
                _rbody.velocity = _grabbedRb.velocity;
                Vector3 direction = _iMove;
                //  _grabbedRb.AddForceAtPosition(pushForce * direction, transform.position);
                _grabbedRb.AddForce(pushForce * direction);
              //  _rbody.MovePosition(transform.position + _grabbedRb.velocity);
            }
        }
        else
        {
            _grabbedRb = null;
        }
    }

    void Use()
    {
        var target = FindInteractionTarget<IInteractable>();
        if (target != null)
        {
            if (_iUse)
            {
                target.Use();
            }
            if(_iUseOnce)
            {
                Debug.Log(_iUseOnce);
                target.UseOnce();
            }
        }
    }

    T FindInteractionTarget<T>()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                T target = collider.GetComponentInParent<T>();
                if(target != null) return target;
            }
        }
        return default(T);
    }
}
