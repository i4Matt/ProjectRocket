using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnemyCounter : MonoBehaviour
{
    GameObject[] enemies;
    public int enemiesLeft;
    public bool isVisible;
    string sceneName;
    Scene currentScene;
    public GUIStyle style;

    private void Start()
    {
        isVisible = false;
    }


    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName == "Main Menu")
        {
            isVisible = false;
        }
        else
        {
            isVisible = true;
        }

        if (isVisible)
        {
            CountEnemies();
        }

    }

    void CountEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesLeft = enemies.Length;
    }


    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 600, 100), "Enemies Remaining: " + enemiesLeft, style);
    }


}
