using UnityEngine;

public class GeneradorEnemigosDificil : MonoBehaviour
{
    [Header("Configuración de la Tropa")]
    public GameObject prefabEnemigo;
    public float espacioX = 1.5f;
    public float espacioY = 1.0f;

    [Header("Posicionamiento Aleatorio")]
    public float rangoMinX = -6f;
    public float rangoMaxX = 6f;
    public float alturaInicioY = 3.5f;

    public void GenerarOleada(int filas, int columnas, ControladorSpaceDificil controlador)
    {
        foreach (Transform hijo in transform)
        {
            Destroy(hijo.gameObject);
        }
        float xAleatorio = Random.Range(rangoMinX, rangoMaxX);
        transform.position = new Vector3(xAleatorio, alturaInicioY, 0f);
        GridEnemigosDificil grid = GetComponent<GridEnemigosDificil>();
        if (grid != null) grid.ResetearMovimiento();
        float inicioX = -(columnas / 2f) * espacioX + (espacioX / 2f);
        float inicioY = 0f;
        int cantidadCreada = 0;

        for (int fila = 0; fila < filas; fila++)
        {
            for (int col = 0; col < columnas; col++)
            {
                Vector2 posicionLocal = new Vector2(inicioX + (col * espacioX), inicioY + (fila * espacioY));
                GameObject nuevoEnemigo = Instantiate(prefabEnemigo, transform);
                nuevoEnemigo.transform.localPosition = posicionLocal;
                cantidadCreada++;
            }
        }

        controlador.RegistrarEnemigos(cantidadCreada);
    }
}