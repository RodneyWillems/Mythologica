using System;
using Photon.Pun;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of MultiplayerManager exists
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadSceneOnline(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
