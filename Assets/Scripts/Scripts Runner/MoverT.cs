using UnityEngine;

public class MoverTumba : MonoBehaviour
{
    public float velocidad = 8f;

    void Update() {
        transform.Translate(Vector3.left * velocidad * Time.deltaTime);
        
        
        if (transform.position.x < -15f) Destroy(gameObject);
    }
}
