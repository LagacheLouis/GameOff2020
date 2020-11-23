using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGroup : MonoBehaviour
{
    public Rigidbody rbody;
    public bool isShip;

    void Start()
    {
        rbody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        rbody.useGravity = false;
        rbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        rbody.interpolation = RigidbodyInterpolation.Interpolate;
        rbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rbody.drag = 1;
        rbody.angularDrag = 1;
    }

    public void Merge(ShipGroup target)
    {
        if (target.isShip || target == this) return;
        int parts = target.transform.childCount;
        for (int i = parts - 1; i >= 0; i--)
        {
            target.transform.GetChild(i)?.GetComponent<ShipPart>()?.SetGroup(this);
        }
        Destroy(target.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        ShipPart part = collision.contacts[0].thisCollider?.GetComponentInParent<ShipPart>();
        if (part) part.Collide(collision);
    }
}
