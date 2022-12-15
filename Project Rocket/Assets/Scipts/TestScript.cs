using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public PlayerCam plCam;
    public PlayerMovement plMove;

    static bool inMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private void LockPlayer(){
        if (Input.GetKeyDown(KeyCode.Escape) && !inMenu){
            plCam.enabled = false;
            plMove.enabled = false;

            inMenu = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && inMenu){
            plCam.enabled = true;
            plMove.enabled = true;

            inMenu = false;
        }
    }
}
