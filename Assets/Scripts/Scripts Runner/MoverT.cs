using UnityEngine;

public class MoverTumba : MonoBehaviour
{
    public float velocidad = 8f;

    void Update() {
        transform.Translate(Vector3.left * velocidad * Time.deltaTime);
        
        // Se borra al pasar al jugador para no acumular basura
        if (transform.position.x < -15f) Destroy(gameObject);
    }
}
