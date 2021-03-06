﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour {

    private static MusicController instance;

    public static MusicController Instance { get { return instance; } }

    public AudioClip menuMusic;
    public AudioClip inGameMusic;

    private int oldSceneIndex = 0;

    void Start()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex > 0 && oldSceneIndex == 0)
        {
            GetComponent<AudioSource>().clip = inGameMusic;
            oldSceneIndex = scene.buildIndex;
            GetComponent<AudioSource>().Play();
        }
        else if (scene.buildIndex == 0 && oldSceneIndex > 0)
        {
            GetComponent<AudioSource>().clip = menuMusic;
            oldSceneIndex = scene.buildIndex;
            GetComponent<AudioSource>().Play();
        }
    }
}
