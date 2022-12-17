using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject death;
    public bool isDead;


    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        death.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == true)
        {
            death.SetActive(true);
            Time.timeScale = 0.001f;
        }
    }


    public void PlayerIsDead()
    {
        isDead = true;
    }
}
