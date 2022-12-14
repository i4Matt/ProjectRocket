using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [Header("Explosion")]
    public ParticleSystem exp;
    public float radius, expForce;
    public int MaxHits = 25;
    public int MaxDamage = 50;
    public int MinDamage = 10;
    public LayerMask HitLayer;
    public LayerMask BlockExplosionLayer;

    private Collider[] Hits; 


    // Start is called before the first frame update
    void Start()
    {
        Hits = new Collider[MaxHits];
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

        if (exp != null)
        {
            var expChild = Instantiate(exp, pos, rot);
            knockBack();
        }
        Destroy(gameObject);
    }

    void knockBack()
    {
        Collider[] launch = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider c in launch) 
        {
            Rigidbody rig = c.GetComponent<Rigidbody>();
            if (rig != null)
            {
                rig.AddExplosionForce(expForce,transform.position, radius);
            }
        }
    }

}
