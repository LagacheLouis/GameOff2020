using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public Rigidbody ship;
    public float force;
    public float flickerNoiseRatio;
    public float flickerNoiseScale;

    public ParticleSystem particleSystem;

    private float random;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, 10000);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float noise = Mathf.PerlinNoise(random, Time.time/ flickerNoiseScale);
        if(noise < flickerNoiseRatio)
        {
            ship.AddForceAtPosition(transform.up * force * noise, transform.position);
            particleSystem.enableEmission = true;
        }
        else
        {
            particleSystem.enableEmission = false;
        }

    }
}
