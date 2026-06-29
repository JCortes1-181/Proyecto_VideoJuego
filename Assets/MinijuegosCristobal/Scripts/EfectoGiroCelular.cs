using UnityEngine;

public class EfectoGiroCelular : MonoBehaviour
{
    private RectTransform rectTransform;
    
    [Header("Configuración del Balanceo")]
    public float velocidadGiro = 4f;   // Qué tan rápido va y viene
    public float anguloMaximo = 12f;   // Qué tanto se inclina a los lados (en grados)
    
    [Header("Configuración del Flote")]
    public float velocidadFlote = 2.5f;  // Qué tan rápido sube y baja
    public float amplitudFlote = 15f;   // Cuántos píxeles se desplaza arriba/abajo

    private Vector2 posicionInicial;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        posicionInicial = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // 1. Efecto de rotación de izquierda a derecha
        float rotacionZ = Mathf.Sin(Time.time * velocidadGiro) * anguloMaximo;
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotacionZ);

        // 2. Efecto de flote sutil para que se mueva en el escritorio
        float desplazamientoY = Mathf.Sin(Time.time * velocidadFlote) * amplitudFlote;
        rectTransform.anchoredPosition = posicionInicial + new Vector2(0f, desplazamientoY);
    }
}