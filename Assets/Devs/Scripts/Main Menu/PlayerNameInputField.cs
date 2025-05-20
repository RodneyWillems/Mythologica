using UnityEngine;
using UnityEngine.UIElements;

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;

public class PlayerNameInputField : MonoBehaviour
{
    const string _playerNamePrefKey = "PlayerName";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string defaultName = string.Empty;
        TMP_InputField inputField = this.GetComponent<TMP_InputField>();
        
        inputField.onValueChanged.AddListener(SetPlayerName);
        if (inputField!=null)
        {
            if (PlayerPrefs.HasKey(_playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(_playerNamePrefKey);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName =  defaultName;
    }

    public void SetPlayerName(string value)
    {
        // #Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(_playerNamePrefKey,value);
    }
}
