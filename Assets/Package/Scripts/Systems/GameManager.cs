using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Audio;

    [SerializeField] private AudioSource Musique;

    void Start()
    {
        Musique.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
