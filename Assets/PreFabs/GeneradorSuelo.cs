using UnityEngine;

public class GeneradorSuelos : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject sueloPrefab;
    public GameObject muroPrefab;
    
    [Header("Ajustes")]
    public float tiempoEntreSuelos = 2.2f;
    public float offsetMuroIzquierda = 0.5f; // Empuja el muro al borde
    private float cronometro = 0f;

    void Start()
    {
        // Genera el primer suelo nada más empezar
        GenerarBloque(false); 
    }

    void Update()
    {
        cronometro += Time.deltaTime;
        if (cronometro >= tiempoEntreSuelos)
        {
            GenerarBloque(true);
            cronometro = 0f;
        }
    }

    void GenerarBloque(bool conMuro)
    {
        if (sueloPrefab == null) return;

        // 1. Crear el suelo
        GameObject nuevoSuelo = Instantiate(sueloPrefab, transform.position, Quaternion.identity);
        
        // 2. Si toca generar muro, calcular su posición en la esquina izquierda
        if (conMuro && muroPrefab != null && Random.Range(0, 100) < 45) 
        {
            // Calculamos la mitad del ancho del suelo basado en su Scale
            float mitadSuelo = nuevoSuelo.transform.localScale.x / 2f;
            
            Vector3 posMuro = transform.position;
            posMuro.x -= (mitadSuelo + offsetMuroIzquierda); // Lo mueve al borde izquierdo
            posMuro.y += 1.2f; // Lo sube para que sea un obstáculo alto

            Instantiate(muroPrefab, posMuro, Quaternion.identity);
        }
    }
}