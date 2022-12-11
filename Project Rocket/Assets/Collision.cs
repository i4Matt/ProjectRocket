using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{


    [Header("Explosion")]
    public ParticleSystem exp;

    public ContactPoint[] contacts;
    // Start is called before the first frame update
    void Start()
    {
        exp = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        Instantiate(exp, pos, rot);
        Destroy(gameObject);
    }


}
