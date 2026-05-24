using UnityEngine;

public class EnemigoInvader : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    
    [Header("Tiempos Aleatorios")]
    public float tiempoMin = 2f; 
    public float tiempoMax = 6f; 

    private float tiempoSiguienteDisparo;

    void Start()
    {
        ProgramarSiguienteDisparo();
    }

    void Update()
    {
        
        if (Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();
            ProgramarSiguienteDisparo();
        }
    }

    void Disparar()
    {
        Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
    }

    void ProgramarSiguienteDisparo()
    {
        
        tiempoSiguienteDisparo = Time.time + Random.Range(tiempoMin, tiempoMax);
    }
}