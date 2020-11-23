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
    [Range(0, 1)]
    public float pivotForceRatio;
    public Color[] playerColors;


    private Rigidbody _rbody;
    private Vector3 _iMove;
    private Rigidbody _ship;
    private bool _iGrab;
    private bool _iUse;
    private bool _iUseOnce;
    private Rigidbody _grabbedRb;
    private Color _playerColor;
    private int _playerIndex;
    private SpriteRenderer _characterRenderer;

    protected override void Initialize()
    {
        _controller.On("Grab", (InputAction.CallbackContext ctx) => {  _iGrab = ctx.ReadValue<float>()>0; });
        _controller.On("Use", (InputAction.CallbackContext ctx) => { _iUse = ctx.ReadValue<float>() > 0; _iUseOnce = _iUse; });
        _controller.On("Move", (InputAction.CallbackContext ctx) => { _iMove = ctx.ReadValue<Vector2>(); });
        UpdateHelmetColor();
    }

    void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        _rbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        _rbody.useGravity = false;
    }

    void UpdateHelmetColor()
    {
        _playerIndex = _controller.GetId();
        //   _playerIndex = UnityEngine.Random.Range(0, 3); // different players can have the same color..
        //_playerColor = new Color(playerColors[_playerIndex].r, playerColors[_playerIndex].g, playerColors[_playerIndex].b, 1f);
        _playerColor = playerColors[_playerIndex];
        _characterRenderer = transform.Find("player_idle_color").gameObject.GetComponent<SpriteRenderer>();
        _characterRenderer.color = _playerColor;
        GameObject helmet = transform.Find("Helmet Quad").gameObject;
        MeshRenderer helmetRenderer = helmet.GetComponent<MeshRenderer>(); // not sure to get the right MeshRenderer..
        helmetRenderer.sortingOrder = 3;
        // helmetRenderer.material.SetColor("PlayerColor", _playerColor);
        helmetRenderer.material.SetColor("PlayerColor", _playerColor);
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
                Vector3 direction = _iMove;
                _grabbedRb.AddForceAtPosition(direction * pushForce * pivotForceRatio, transform.position);
                _grabbedRb.AddForce(direction * pushForce * (1f-pivotForceRatio));
                _rbody.velocity = _grabbedRb.GetPointVelocity(transform.position);
                _rbody.angularVelocity = _grabbedRb.angularVelocity;
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
