using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    public static Highscore instance;

    public int HIGHSCORE = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }

    }
}
