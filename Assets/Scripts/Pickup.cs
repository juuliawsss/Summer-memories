using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Destroy this firefly after 0.2 seconds when the player touches it
            Destroy(gameObject, 0.2f);
        }
    }
}
