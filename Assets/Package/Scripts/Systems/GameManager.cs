using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Audio;

    [Header("Volume")]
    [Range(0f, 1f)] public float MusicVolume = 1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
