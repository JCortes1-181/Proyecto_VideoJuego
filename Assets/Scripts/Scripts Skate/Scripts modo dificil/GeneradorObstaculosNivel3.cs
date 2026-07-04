using UnityEngine;

public class GeneradorObstaculosNivel3 : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabObstaculoSuelo; 
    public GameObject prefabObstaculoAlto;  

    [Header("Configuración de Dificultad")]
    public float velocidadObjetos = 8f; 
    public float alturaObstaculoAlto = 2.5f; 
    public float tiempoMinimo = 1.0f; 
    public float tiempoMaximo = 2.0f;

    private float tiempo;
    private float tiempoSiguienteAparicion;
    private int ultimoObstaculo = -1; 

    void Start()
    {
        tiempoSiguienteAparicion = Random.Range(tiempoMinimo, tiempoMaximo);
    }

    void Update()
    {
        tiempo += Time.deltaTime;

        if (tiempo >= tiempoSiguienteAparicion)
        {
            GenerarMixto();
            tiempo = 0;
            tiempoSiguienteAparicion = Random.Range(tiempoMinimo, tiempoMaximo);
        }
    }

    void GenerarMixto()
    {
        int nuevoTipo;

        do
        {
            nuevoTipo = Random.Range(0, 2); 
        } 
        while (nuevoTipo == ultimoObstaculo); 


        GameObject prefabElegido = (nuevoTipo == 0) ? prefabObstaculoSuelo : prefabObstaculoAlto;
        Vector3 posicion = (nuevoTipo == 0) ? transform.position : new Vector3(transform.position.x, transform.position.y + alturaObstaculoAlto, transform.position.z);

        GameObject obstaculo = Instantiate(prefabElegido, posicion, Quaternion.identity);
        
        Rigidbody2D rb = obstaculo.GetComponent<Rigidbody2D>();
        if(rb != null) rb.linearVelocity = Vector2.left * velocidadObjetos;

        Destroy(obstaculo, 6f);
        
        ultimoObstaculo = nuevoTipo;
    }
}
