using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform caneraPosition;

    private void Update(){
        transform.position = caneraPosition.position;
    }
}
