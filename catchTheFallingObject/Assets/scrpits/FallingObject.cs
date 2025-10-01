using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 3f;                                            


    void Update()
    {
                                                                              
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

                                                                              
        if (transform.position.y < -5)                             
        {
            Destroy(gameObject);
        }
    }

}
