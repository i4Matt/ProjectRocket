using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections.Specialized;

public class ProjectileGun : MonoBehaviour
{



    [Header("Bullet")]
    public GameObject bullet;

    [Header("Explosion")]
    public GameObject exp;

    [Header("Force")]
    public float shootForce;
    public float upwardForce;

    [Header("Gun Stats")]
    public float timeBetweenShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;
    public int magazineSize;
    public int bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    // bools
    bool shooting, readyToShoot, reloading;

    [Header("References")]
    public Camera fpsCam;
    public Transform attackPoint;

    [Header("Graphics")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    [Header("KeyBinds")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;

    [Header("Other")]
    public bool allowInvoke = true;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void Awake(){
        // Make Sure Magazine is Full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    private void Update()
    {
        MyInput();

        // Set Ammo Display, if it exists :D
        if (ammunitionDisplay != null){
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
    }

    private void MyInput(){
        // Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(shootKey);
        else Input.GetKeyDown(shootKey);

        // Reloading
        if (Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading) Reload();
        // Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0){
            // Set Bullets Shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot(){
        readyToShoot = false;

        // Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Just a ray throug the middle of you....... IDK His screen cuts off.
        RaycastHit hit;

        // Check if Ray Hits Something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); // Just a Point far away from the player

        // Calculate direction from attackPoint to targetPoint

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        
        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        // Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Instantiate bullet/projectice
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        // Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        
        // Instantiate muzzle Flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        // Invoke ResetShot function (if not already invoked)
        if (allowInvoke){
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        // If more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot(){
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload(){
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished() {
        bulletsLeft = magazineSize;
        reloading = false;
    }


    


    void OnCollisionEnter(Collision co)
    {
        shootForce = 0;
        upwardForce = 0;

        ContactPoint contact = co.contacts[0];
        Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (exp != null)
        {
            var hitVfX = Instantiate (exp, pos, rot);
        }


        Destroy (gameObject);
    }
}
