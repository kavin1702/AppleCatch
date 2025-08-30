using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 3f;                                              // How fast the object falls


    void Update()
    {
                                                                              // Move the object downward
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

                                                                              // Destroy object if it falls off the screen
        if (transform.position.y < -5)                                        // Make sure this value fits your game view
        {
            Destroy(gameObject);
        }
    }

}
