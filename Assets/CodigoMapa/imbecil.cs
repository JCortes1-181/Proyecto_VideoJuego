using UnityEngine;

public class EfectoGelatina : MonoBehaviour
{
    [Header("Ajustes de Gelatina")]
    public float velocidad = 2f;    // Qué tan rápido rebota
    public float intensidad = 0.1f; // Qué tanto se estira (0.1 es un 10%)
    
    private Vector3 escalaOriginal;

    void Start()
    {
        // Guardamos la escala que le pusiste en el Inspector (0.4877, 0.4291 aprox)
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        // Usamos Sinusoidal para el efecto de rebote
        // El eje Y se estira mientras el eje X se encoge (efecto squash)
        float variacion = Mathf.Sin(Time.time * velocidad) * intensidad;

        transform.localScale = new Vector3(
            escalaOriginal.x - (variacion * 0.5f), // Se encoge un poco a los lados
            escalaOriginal.y + variacion,          // Se estira hacia arriba
            escalaOriginal.z
        );
    }
}
