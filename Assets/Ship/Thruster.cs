using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour, IInteractable
{
    public bool powered;
    public ShipPart shipPart;
    public float force;
    [Range(0,1)]
    public float pivotForceRatio;

    public ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (powered)
        {
            shipPart.group.rbody.AddForce(transform.up * force * (1 - pivotForceRatio));
            shipPart.group.rbody.AddForceAtPosition(transform.up * force * pivotForceRatio, transform.position);
        }

        particleSystem.enableEmission = powered;

    }

    public void Use()
    {
      
    }

    public void UseOnce()
    {
        powered = !powered;
    }
}
