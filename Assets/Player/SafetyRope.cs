using Boo.Lang;
using System;
using UnityEngine;


public class SafetyRope : MonoBehaviour
{

    public Rigidbody connectedBody;
    public float minDistance;
    public float maxDistance;
    public float spring;

    public LineRenderer line;

    protected Rigidbody _rbody;

    void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        line.useWorldSpace = true;
    }

    void Start()
    {

    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, connectedBody.transform.position);
        List<Vector3> points = new List<Vector3>();
        float nbPoints = 30f;
        line.positionCount = (int)nbPoints + 1;
        points.Add(transform.position);
        for (int i=1; i<nbPoints; i++)
        {
            Vector3 point = Vector3.Lerp(transform.position, connectedBody.transform.position, i/ nbPoints);
            Vector3 lpoint = connectedBody.transform.InverseTransformPoint(point) / 2f + new Vector3(Time.time / 5f, Time.time / 5f + 100, 0);
            point += new Vector3(Mathf.PerlinNoise(lpoint.x, lpoint.y) - 0.5f, Mathf.PerlinNoise(lpoint.x + 100, lpoint.y + 100) - 0.5f) * (1 - distance/maxDistance) * 8f;
            points.Add(point);
        }
        points.Add(connectedBody.transform.position);
        line.SetPositions(points.ToArray());
    }

    void FixedUpdate()
    {
        if (connectedBody == null) return;
        var connection = _rbody.position - connectedBody.position;
        float delta = 0;
        if(connection.magnitude < minDistance)
        {
            delta = minDistance - connection.magnitude;
        }
        else if (connection.magnitude > maxDistance)
        {
            delta = maxDistance - connection.magnitude;
        }
        Debug.Log(_rbody.velocity + " " + connectedBody.velocity);
        _rbody.position += delta * connection.normalized;
        _rbody.velocity += delta * connection.normalized * spring;

    }
}