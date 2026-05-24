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
        
        if (moviendoDerecha)
        {
            
            transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            
            
            if (transform.position.x >= limiteDerecho)
            {
                BajarYCambiarDireccion();
            }
        }
        else
        {
            
            transform.Translate(Vector2.left * velocidad * Time.deltaTime);
            
            
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