using UnityEngine;

public class Firefly : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the firefly forward at a constant speed
        float speed = 2.0f;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
