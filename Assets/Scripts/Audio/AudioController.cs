using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource Shield;
    public AudioSource Detonate;
    public AudioSource MenuSelection;
    public AudioSource ControlMenuSelection;

    public void ShieldAudio()
    {
        Shield.Play();
    }

    public void DetonateAudio()
    {
        Detonate.Play();
    }

    public void MenuSeletionAudio()
    {
        MenuSelection.Play();
    }

    public void ControlMenuSelctionAudio()
    {
        ControlMenuSelection.Play();
    }
}
    

    
