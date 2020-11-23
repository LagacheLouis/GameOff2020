using JetBrains.Annotations;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class ShipPart : MonoBehaviour
{
    public ShipGroup group;
    public List<ShipPart> dockedParts = new List<ShipPart>();
    public bool shipCore;
    public bool undockable;
    [Header("Life")]
    public float maxLife = 100;
    public float life = 100;
    public float minImpulse = 3;
    [Header("Effect")]
    public ParticleSystem damageEffect;
    public AudioEvent damageSound;

    // Start is called before the first frame update
    void Start()
    {
        if (!group) CreateGroup();
    }

    public void CreateGroup()
    {
        GameObject groupObject = new GameObject();
        groupObject.name = "ShipGroup";
        group = groupObject.AddComponent<ShipGroup>() as ShipGroup;
        group.transform.position = transform.position;
        SetGroup(group);
        LinkDockedParts(group);
    }

    public void LinkDockedParts(ShipGroup shipGroup)
    {
        foreach (ShipPart part in dockedParts)
        {
            if (part.group != shipGroup)
            {
                part.SetGroup(shipGroup);
                part.LinkDockedParts(shipGroup);
            }
        }
    }

    public void SetGroup(ShipGroup group)
    {
        this.group = group;
        transform.parent = group.transform;
        if (shipCore)
        {
            group.name = "Ship";
            group.isShip = true;
        }
    }

    public void Dock(ShipPart shipPart)
    {
        if (!dockedParts.Contains(shipPart) && !undockable && !shipPart.undockable)
        {
            dockedParts.Add(shipPart);
            shipPart.Dock(this);
            group.Merge(shipPart.group);
        }
    }

    public void Undock(ShipPart shipPart)
    {
        shipPart.dockedParts.Remove(this);
        dockedParts.Remove(shipPart);
        CreateGroup();
    }

    void Update()
    {
        foreach (ShipPart part in dockedParts)
        {
            Debug.DrawLine(transform.position, Vector3.Lerp (transform.position, part.transform.position,0.5f),Color.red);
        }
    }

    [Button]
    void Explode()
    {
        Broken();
        Destroy(group.gameObject);
        Destroy(gameObject);
    }

    [Button]
    void Broken()
    {
        int parts = dockedParts.Count;
        for (int i = parts - 1; i >= 0; i--)
        {
            dockedParts[i].Undock(this);
        }
        undockable = true;
    }

    public void TakeDamage(float amount)
    {
        life -= amount;
        if (life <= 0)
        {
            Explode();
        }
    }

    public void Collide(Collision collision)
    {
        var rbody = collision.collider.GetComponentInParent<Rigidbody>();
        var shipPart = collision.collider.GetComponentInParent<ShipPart>();
        if (shipPart && !undockable)
        {
            Dock(shipPart);
        }else if (rbody && !collision.collider.CompareTag("Player"))
        {
            float impulse = Vector3.Magnitude(collision.impulse);
            if(impulse > minImpulse)
            {
                Instantiate(damageEffect, collision.contacts[0].point, Quaternion.identity);
                Camera.main.DOShakePosition(.3f,1f);
                TakeDamage(impulse-minImpulse);
            }
        }
    }

}
