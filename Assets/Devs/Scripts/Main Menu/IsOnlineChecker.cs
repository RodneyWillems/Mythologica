using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class IsOnlineChecker : MonoBehaviour
{
    private Toggle _isOnline;
    void Start()
    {
        _isOnline = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOnline != null)
        {
            _isOnline.isOn = PhotonNetwork.IsConnected;
        }

        if (gameObject.name == "Connect Button")
        {
            if (PhotonNetwork.IsConnected)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
