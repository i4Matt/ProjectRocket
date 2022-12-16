using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{   

    public void  PlayLevel(int val)
    {
        SceneManager.LoadScene(val);
    }
   
    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}

