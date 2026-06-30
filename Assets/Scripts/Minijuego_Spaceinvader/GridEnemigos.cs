using UnityEngine;

public class GridEnemigos : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 2f;
    public float distanciaBajada = 0.5f;
    
    [Header("Límites de la Pantalla")]
    public float limiteIzquierdo = -6f;
    public float limiteDerecho = 6f;

    private bool moviendoDerecha = true;

    void Update()
    {
        // === NUEVA LÍNEA AGREGADA ===
        // Multiplicamos la velocidad base por el modificador de dificultad actual
        float velocidadActual = velocidad * DifficultyManager.Instance.CurrentMultiplier;
        
        if (moviendoDerecha)
        {
            // === CAMBIO: Ahora usamos 'velocidadActual' en vez de 'velocidad' ===
            transform.Translate(Vector2.right * velocidadActual * Time.deltaTime);
            
            if (transform.position.x >= limiteDerecho)
            {
                BajarYCambiarDireccion();
            }
        }
        else
        {
            // === CAMBIO: Aquí también usamos 'velocidadActual' ===
            transform.Translate(Vector2.left * velocidadActual * Time.deltaTime);
            
            if (transform.position.x <= limiteIzquierdo)
            {
                BajarYCambiarDireccion();
            }
        }
    }

    void BajarYCambiarDireccion()
    {
        moviendoDerecha = !moviendoDerecha;
        transform.position = new Vector2(transform.position.x, transform.position.y - distanciaBajada);
    }
}