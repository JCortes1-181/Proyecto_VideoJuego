using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Configuración de la Tropa")]
    public GameObject prefabEnemigo; // Aquí pondremos el molde de tu nave
    public int filas = 4;
    public int columnas = 8;
    
    [Header("Espaciado")]
    public float espacioX = 1.5f;
    public float espacioY = 1.0f;

    void Start()
    {
        GenerarEnemigos();
    }

    void GenerarEnemigos()
    {
        
        float inicioX = -(columnas / 2f) * espacioX + (espacioX / 2f);
        float inicioY = 1.5f; 

        for (int fila = 0; fila < filas; fila++)
        {
            for (int col = 0; col < columnas; col++)
            {
                
                Vector2 posicion = new Vector2(inicioX + (col * espacioX), inicioY + (fila * espacioY));
                
                
                GameObject nuevoEnemigo = Instantiate(prefabEnemigo, posicion, Quaternion.identity);
                
                
                nuevoEnemigo.transform.SetParent(this.transform);
            }
        }
    }
}