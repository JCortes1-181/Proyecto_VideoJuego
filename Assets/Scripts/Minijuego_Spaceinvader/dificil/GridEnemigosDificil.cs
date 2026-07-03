using UnityEngine;

public class GridEnemigosDificil : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 2f;
    public float distanciaBajada = 0.5f;
    
    [Header("Límites de la Pantalla")]
    public float limiteIzquierdo = -6f;
    public float limiteDerecho = 6f;

    [Header("Límite de Derrota")]
    [Tooltip("Altura a la que los enemigos tocan el suelo y pierdes")]
    public float limiteSuelo = -4f;
    public ControladorSpaceDificil controlador;

    private bool moviendoDerecha = true;

    public void ResetearMovimiento()
    {
        moviendoDerecha = true;
    }

    void Update()
    {
        float velocidadActual = velocidad;
        if (DifficultyManager.Instance != null) {
            velocidadActual = velocidad * DifficultyManager.Instance.CurrentMultiplier;
        }
        
        if (moviendoDerecha)
        {
            transform.Translate(Vector2.right * velocidadActual * Time.deltaTime);
            if (transform.position.x >= limiteDerecho) BajarYCambiarDireccion();
        }
        else
        {
            transform.Translate(Vector2.left * velocidadActual * Time.deltaTime);
            if (transform.position.x <= limiteIzquierdo) BajarYCambiarDireccion();
        }

        // Comprobar si llegaron al suelo para perder
        if (transform.position.y <= limiteSuelo)
        {
            if (controlador != null) controlador.EnemigosLlegaronAbajo();
        }
    }

    void BajarYCambiarDireccion()
    {
        moviendoDerecha = !moviendoDerecha;
        transform.position = new Vector2(transform.position.x, transform.position.y - distanciaBajada);
    }
}