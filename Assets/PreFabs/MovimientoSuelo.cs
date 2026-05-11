using UnityEngine;

public class MovimientoSuelo : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 8f; 
    public float limiteIzquierdo = -20f; 

    void Update()
    {
        
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);

        
        if (transform.position.x < limiteIzquierdo)
        {
            Destroy(gameObject);
        }
    }
}