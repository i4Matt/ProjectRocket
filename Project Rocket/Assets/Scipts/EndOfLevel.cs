using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{

    public bool enemiesLeft;
    GameObject[] objects;

    public GameObject victory;
    // Start is called before the first frame update
    void Start()
    {
        enemiesLeft = true;
        victory.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Scan();

        if (!enemiesLeft)
        {
            Time.timeScale = 0.5f;
            victory.SetActive(true);
            Invoke("ToMenu", 2);
        }
    }

    void Scan()
    {
        int enemyCount = 0;
        objects = FindObjectsOfType<GameObject>();
        foreach (GameObject check in objects)
        {
            if(check.name.Contains("Enemy"))
            {
                enemyCount += 1;
            }
        }

        if (enemyCount == 0)
        {
            enemiesLeft = false;
        }

    }

    void ToMenu()
    {
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
