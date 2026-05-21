using UnityEngine;

public class GeneradorObstaculos : MonoBehaviour
{
    public GameObject prefabPiedra; // Arrastra aquí el prefab de tu roca
    public float tiempoAparicion = 2.5f;
    public float velocidadPiedra = 7f;
    private float tiempo;

    void Update()
    {
        tiempo += Time.deltaTime;
        if (tiempo >= tiempoAparicion)
        {
            CrearPiedra();
            tiempo = 0;
            tiempoAparicion = Random.Range(1.8f, 3.5f); // Variedad en la aparición
        }
    }

   void CrearPiedra()
{
    // Usa la posición real que le pongas al objeto en el editor
    GameObject piedra = Instantiate(prefabPiedra, transform.position, Quaternion.identity);
    
    Rigidbody2D rbPiedra = piedra.GetComponent<Rigidbody2D>();
    if(rbPiedra != null) rbPiedra.linearVelocity = Vector2.left * velocidadPiedra;

    Destroy(piedra, 6f);
}
}
