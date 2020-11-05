using UnityEngine;

public class DistanceJoint3D : MonoBehaviour
{

    public Transform connectedRigidbody;
    public float minDistance = 10;
    public float maxDistance = 3;
    public float spring = 0.1f;
    public float damper = 5f;

    protected Rigidbody _rbody;

    void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {

        Vector3 connection = _rbody.position - connectedRigidbody.position;
        float delta = 0;
        if(connection.magnitude < minDistance)
        {
            delta = minDistance - connection.magnitude;
        }
        else if(connection.magnitude > maxDistance)
        {
            delta = maxDistance - connection.magnitude;
        }

        _rbody.position += delta * connection.normalized;
    }
}