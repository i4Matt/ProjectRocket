using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    public float respawnTime;
    public GameObject heart1;
    public GameObject heart2;
    public AudioSource getHit;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        if(gameObject.name == "Player")
        {
        heart1.SetActive(true);
        heart2.SetActive(true);
        }
    }


    public void TakeDamage()
    {
        currentHealth -= 1;
        if(gameObject.name == "Player")
        {
        heart2.SetActive(false);
        getHit.Play();
        }
        if (currentHealth <= 0)
        {
            if (gameObject.name == "Player")
            {
                heart1.SetActive(false);
                gameObject.GetComponent<PlayerDeath>().PlayerIsDead();
                Invoke("ResetLevel", respawnTime / 1000f);
            }
            if (gameObject.name.Contains("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }


    public void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if(gameObject.name.Contains("Player"))
        {
            if(collision.gameObject.name.Contains("Lava"))
            {
                gameObject.GetComponent<PlayerDeath>().PlayerIsDead();
                Invoke("ResetLevel", respawnTime / 1000f);
            }
        }
    }


    public void ResetLevel()
    {
        Scene reset = SceneManager.GetActiveScene();
        SceneManager.LoadScene(reset.name);
    }
}
