using UnityEngine;

public class MovimientoSuelo : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 8f; // Velocidad a la que avanza el mapa
    public float limiteIzquierdo = -20f; // Punto donde se borra el suelo

    void Update()
    {
        // Mueve el suelo hacia la izquierda de la pantalla
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);

        // Si el suelo sale de la vista por la izquierda, se elimina
        if (transform.position.x < limiteIzquierdo)
        {
            Destroy(gameObject);
        }
    }
}