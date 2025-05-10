using UnityEngine;
using UnityEngine.InputSystem;

public class OrpheusEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MinigameManager.Instance.OrpheusWin(other.GetComponent<PlayerInput>());
            GetComponent<Collider>().enabled = false;
        }
    }
}
