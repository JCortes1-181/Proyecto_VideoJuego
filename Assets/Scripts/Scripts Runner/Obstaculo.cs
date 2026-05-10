using UnityEngine;

public class Obstaculos : MonoBehaviour
{
    public GameObject tumbaPrefab;
    public float tiempoGeneracion = 1.5f; // Ajusta qué tan rápido salen

    void Start() {
        InvokeRepeating("Generar", 1f, tiempoGeneracion);
    }

    void Generar() {
        // Aparece fuera de cámara a la derecha (X=12)
        Instantiate(tumbaPrefab, new Vector3(12f, -3.5f, 0f), Quaternion.identity);
    }
}
