using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    // Add Gun Script Here
    [Header("References")]
    public ProjectileGun gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    public Canvas crossHair;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    [Header("HotKeys")]
    public KeyCode pickUpKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;

    public GameObject bullet1;
    public GameObject bullet2;

    // Start is called before the first frame update
    private void Start()
    {
        //Setup
        if (!equipped){
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
            slotFull = false;
            crossHair.enabled = false;
        }
        if (equipped){
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
            crossHair.enabled = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if player is in range and "PickUp Key" is Pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(pickUpKey) && !slotFull) {PickUp();}

        // Drop if equipped and "Drop Key" is pressed
        if (equipped && Input.GetKeyDown(dropKey)) {Drop();}  
    }

    private void PickUp(){
        equipped = true;
        slotFull = true;

        // Make Weapon a Child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        // Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        // Enable Gun Script
        gunScript.enabled = true;

        //Enable Hud
        crossHair.enabled = true;
    }

    private void Drop(){
        equipped = false;
        slotFull = false;

        // Set Parent to null
        transform.SetParent(null);

        // Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        // Gun carrie momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        // Add Forces to Gun
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        // Disable Gun Script
        gunScript.enabled = false;

        // Disable Hud
        crossHair.enabled = false;
    }
}
