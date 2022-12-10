using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    // Add Gun Script Here
    [Header("References")]
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    [Header("HotKeys")]
    public static KeyCode pickUpKey = KeyCode.E;
    public static KeyCode dropKey = KeyCode.Q;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Check if player is in range and "PickUp Key" is Pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(pickUpKey) && !slotFull) {PickUp();}

        //Drop if equipped and "Drop Key" is pressed
        if (equipped && Input.GetKeyDown(dropKey)) {Drop();}
    }

    private void PickUp(){
        equipped = true;
        slotFull = true;

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable Gun Script
    }

    private void Drop(){
        equipped = false;
        slotFull = false;

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Disable Gun Script
    }
}
