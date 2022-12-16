using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuSettings : MonoBehaviour
{

    public  AudioMixer audioMixer;
   public void SetVolume (float volume)
   {
    audioMixer.SetFloat("Volume", volume);
    Debug.Log(volume);
   }



}
