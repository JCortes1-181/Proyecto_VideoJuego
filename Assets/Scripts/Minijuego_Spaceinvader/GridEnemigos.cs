using UnityEngine;

public class GridEnemigos : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 2f;
    public float distanciaBajada = 0.5f;
    
    [Header("Límites de la Pantalla")]
    public float limiteIzquierdo = -6f;
    public float limiteDerecho = 6f;
    
    [Header("Límite de Derrota")]
    public float limiteDerrotaY = -3.5f; 

    [Header("Referencia")]
    public ControladorSpace controladorJuego;

    private bool moviendoDerecha = true;

    void Update()
    {
        float velocidadActual = velocidad * DifficultyManager.Instance.CurrentMultiplier;
        
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

        foreach (Transform enemigo in transform)
        {
            if (enemigo.gameObject.activeSelf && enemigo.position.y <= limiteDerrotaY)
            {
                if (controladorJuego != null)
                {
                    controladorJuego.JugadorTocado(); 
                }
                break; 
            }
        }
    }

    void BajarYCambiarDireccion()
    {
        moviendoDerecha = !moviendoDerecha;
        transform.position = new Vector2(transform.position.x, transform.position.y - distanciaBajada);
    }
}