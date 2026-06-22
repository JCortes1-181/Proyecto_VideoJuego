using UnityEngine;

public class MovimientoPopUp : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 posicionInicial;
    
    // Variables para hacer el movimiento único por cada anuncio
    private float velocidadX;
    private float velocidadY;
    private float amplitudX;
    private float amplitudY;
    private float desfaseTiempo;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        // Guardamos la posición exacta donde nació dentro del contenedor
        posicionInicial = rectTransform.anchoredPosition;

        // Aleatoriedad para que no todos se muevan en perfecta sincronía
        velocidadX = Random.Range(1.2f, 2.5f);
        velocidadY = Random.Range(1.2f, 2.5f);
        
        // Qué tantos píxeles se va a desplazar hacia los lados (puedes ajustar estos números)
        amplitudX = Random.Range(15f, 30f); 
        amplitudY = Random.Range(15f, 30f); 
        
        desfaseTiempo = Random.Range(0f, 5f);
    }

    void Update()
    {
        // Usamos funciones matemáticas (Seno y Coseno) para crear un vaivén circular/suave
        float desplazamientoX = Mathf.Sin(Time.time * velocidadX + desfaseTiempo) * amplitudX;
        float desplazamientoY = Mathf.Cos(Time.time * velocidadY + desfaseTiempo) * amplitudY;

        rectTransform.anchoredPosition = posicionInicial + new Vector2(desplazamientoX, desplazamientoY);
    }
}