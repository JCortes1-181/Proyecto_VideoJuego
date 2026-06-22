using UnityEngine;

public class EfectoGiroCelular : MonoBehaviour
{
    private RectTransform rectTransform;
    
    [Header("Configuración del Balanceo")]
    public float velocidadGiro = 3f;   // Qué tan rápido va y viene
    public float anguloMaximo = 15f;   // Qué tanto se inclina a los lados (en grados)
    
    [Header("Configuración del Flote (Opcional)")]
    public float velocidadFlote = 2f;  // Qué tan rápido sube y baja
    public float AmplitudFlote = 20f;  // Cuántos píxeles se desplaza arriba/abajo

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

        // 2. Efecto de flote sutil para que no sea estático en el escritorio
        float desplazamientoY = Mathf.Sin(Time.time * velocidadFlote) * AmplitudFlote;
        rectTransform.anchoredPosition = posicionInicial + new Vector2(0f, desplazamientoY);
    }
}