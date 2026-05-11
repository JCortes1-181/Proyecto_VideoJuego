using UnityEngine;

public class GeneradorSuelos : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject sueloPrefab;
    public GameObject muroPrefab;
    
    [Header("Ajustes")]
    public float tiempoEntreSuelos = 2.2f;
    public float offsetMuroIzquierda = 0.5f; 
    private float cronometro = 0f;

    void Start()
    {
        
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

       
        GameObject nuevoSuelo = Instantiate(sueloPrefab, transform.position, Quaternion.identity);
        
       
        if (conMuro && muroPrefab != null && Random.Range(0, 100) < 45) 
        {
           
            float mitadSuelo = nuevoSuelo.transform.localScale.x / 2f;
            
            Vector3 posMuro = transform.position;
            posMuro.x -= (mitadSuelo + offsetMuroIzquierda); 
            posMuro.y += 1.2f; 

            Instantiate(muroPrefab, posMuro, Quaternion.identity);
        }
    }
}