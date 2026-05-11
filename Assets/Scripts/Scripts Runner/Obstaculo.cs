using UnityEngine;

public class Obstaculos : MonoBehaviour
{
    public GameObject tumbaPrefab;
    public float tiempoGeneracion = 1.5f; 

    void Start() {
        InvokeRepeating("Generar", 1f, tiempoGeneracion);
    }

    void Generar() {
        
        Instantiate(tumbaPrefab, new Vector3(12f, -3.5f, 0f), Quaternion.identity);
    }
}
