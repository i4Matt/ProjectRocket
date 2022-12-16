using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
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
                //Insert restart here
            }
            if (gameObject.name.Contains("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}
