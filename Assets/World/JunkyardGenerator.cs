using Shaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkyardGenerator : MonoBehaviour
{
    public List<GameObject> junks = new List<GameObject>();
    public ShipPart shipCore;
    public float spawnDistance;
    public float directionAngleVariation;
    public float spawnSpeed;
    public float junkLifeTime;

    private float _spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shipCore && shipCore.group && shipCore.group.rbody)
        {
            Vector3 direction = shipCore.group.rbody.velocity.normalized;
            _spawnTimer += shipCore.group.rbody.velocity.magnitude * Time.deltaTime;
            if (_spawnTimer > spawnSpeed)
            {
                _spawnTimer -= spawnSpeed;
                float randomAngle = Random.Range(-directionAngleVariation * 0.5f, directionAngleVariation * 0.5f);
                direction = Quaternion.Euler(0, 0, randomAngle) * direction;
                Vector3 position = shipCore.transform.position + direction * spawnDistance;
                GameObject junk = Instantiate(junks.RandomItem(), position, Quaternion.identity);
                junk.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
                junk.transform.localScale = Vector3.one + Vector3.one * Random.Range(0, 10);
            }
        }
    }
}
