using UnityEngine;

public class Pickup : MonoBehaviour
{
    private bool playerNearby = false;

    void Update()
    {
        // Check if player is nearby and E is pressed
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
