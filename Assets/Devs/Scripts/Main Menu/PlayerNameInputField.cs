using UnityEngine;
using UnityEngine.UIElements;

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;

public class PlayerNameInputField : MonoBehaviour
{
    const string _playerNamePrefKey = "PlayerName";

    private TMP_InputField _inputField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string defaultName = string.Empty;
        _inputField = this.GetComponent<TMP_InputField>();
        
        _inputField.onValueChanged.AddListener(SetPlayerName);
        if (_inputField!=null)
        {
            if (PlayerPrefs.HasKey(_playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(_playerNamePrefKey);
                _inputField.text = defaultName;
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
        if (value.Length > 12)
        {
            _inputField.text =_inputField.text.Remove(12, 1);
            Debug.LogError("Player Name is too long, must be less than 12 characters");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(_playerNamePrefKey,value);
    }
}
