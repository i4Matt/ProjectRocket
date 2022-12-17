using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    public float respawnTime;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage()
    {
        currentHealth -= 1;

        if (currentHealth <= 0)
        {
            if (gameObject.name == "Player")
            {
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
