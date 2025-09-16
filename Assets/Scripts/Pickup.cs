using UnityEngine;

public class Pickup : MonoBehaviour
{
    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Try to raycast from mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj.CompareTag("Pickup"))
                {
                    Debug.Log("E key pressed, destroying pickup under mouse.");
                    Destroy(hitObj);
                    return;
                }
            }

            // If no pickup under mouse, destroy nearest
            GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
            GameObject nearest = null;
            float minDist = Mathf.Infinity;

            foreach (GameObject pickup in pickups)
            {
                if (pickup == this.gameObject) continue; // Skip self
                float dist = Vector3.Distance(transform.position, pickup.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = pickup;
                }
            }

            if (nearest != null)
            {
                Debug.Log("E key pressed, destroying nearest pickup.");
                Destroy(nearest);
            }
            else
            {
                Debug.Log("E key pressed, but no other pickups found.");
            }
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
