using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem exp;

    [Header("Explosion")]
    public bool doesExplode;
    public float explosionRadius;
    public float explosionForce;

    [Header("Settings")]
    
    public bool isHostile;

    
    
    public LayerMask HitLayer;
    public LayerMask BlockExplosionLayer;
    

    private Vector3 startPosition;
    public float shotDistance;

    private Collider[] Hits; 


    // Start is called before the first frame update
    void Start()
    {
        startPosition= transform.position;
    }

    // Update is called once per frame.
    void Update()
    {
        Vector3 currectLocation = transform.position;
        float dif = Mathf.Sqrt(Mathf.Pow(currectLocation.x - startPosition.x, 2) + Mathf.Pow(currectLocation.y - startPosition.y, 2) + Mathf.Pow(currectLocation.z - startPosition.z, 2));
        if (dif >= shotDistance)
        {
            Destroy(gameObject);
        }
    }



    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (exp != null && doesExplode)
        {
            var expChild = Instantiate(exp, pos, rot);
            knockBack();
        }
        Destroy(gameObject);
    }

    void knockBack()
    {
        Collider[] launch = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider c in launch) 
        {
            Rigidbody rig = c.GetComponent<Rigidbody>();
            if (rig != null)
            {
                rig.AddExplosionForce(explosionForce,transform.position, explosionRadius);
            }
        }
    }

}
