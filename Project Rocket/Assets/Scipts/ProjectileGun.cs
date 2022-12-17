using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections.Specialized;
using System.Diagnostics;

public class ProjectileGun : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bullet;

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
    public GameObject Bullet1;
    public GameObject Bullet2;

    [Header("Graphics")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    [Header("KeyBinds")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;

    [Header("Sound Effects")]
    public AudioSource shootingSFX;
    public AudioSource reloadingSFX;

    [Header("Other")]
    public bool allowInvoke = true;

    void Awake(){
        // Make Sure Magazine is Full
        bulletsLeft = magazineSize;
        readyToShoot = true;
        UIReload();

    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        // Set Ammo Display, if it exists :D
        if (ammunitionDisplay != null){
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }

    }

    void MyInput(){
        // Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(shootKey);
        else shooting = Input.GetKeyDown(shootKey);

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

    void Shoot(){
        readyToShoot = false;
        shootingSFX.Play();

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
        UIDrop();

        // Invoke ResetShot function (if not already invoked)
        if (allowInvoke){
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        // If more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    void ResetShot(){
        readyToShoot = true;
        allowInvoke = true;
    }

    void Reload(){
        reloading = true;
        reloadingSFX.Play();
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished() {
        bulletsLeft = magazineSize;
        reloading = false;
        UIReload();
    }

    void UIReload(){
        if (magazineSize == 1){
            Bullet1.SetActive(true);
            Bullet2.SetActive(false);
        }else{
            Bullet1.SetActive(true);
            Bullet2.SetActive(true);
        }
    }

    void UIDrop(){
        if (bulletsLeft == 1){
            Bullet2.SetActive(false);
        }else if (bulletsLeft == 0) {
            Bullet1.SetActive(false);
        }
    }
}
