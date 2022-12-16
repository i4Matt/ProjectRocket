using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionsMenu;
    
    public void LevelReturn ()
    {
        //Debug.Log("QUIT")
        SceneManager.LoadScene(0);
    }


    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
    {
        if(GameIsPaused)
        {
           Resume(); 
        } else 
        {
            Pause();
        }
    } 
    }
    
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        OptionsMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
