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
        // 1. Limpiar enemigos anteriores si quedó alguno
        foreach (Transform hijo in transform)
        {
            Destroy(hijo.gameObject);
        }

        // 2. Colocar el padre (el grid) en una posición X aleatoria arriba
        float xAleatorio = Random.Range(rangoMinX, rangoMaxX);
        transform.position = new Vector3(xAleatorio, alturaInicioY, 0f);

        // 3. Reiniciar la dirección para que siempre empiece a moverse bien
        GridEnemigosDificil grid = GetComponent<GridEnemigosDificil>();
        if (grid != null) grid.ResetearMovimiento();

        // 4. Instanciar los enemigos en sus posiciones locales
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

        // 5. Avisarle al controlador cuántos enemigos hay que matar
        controlador.RegistrarEnemigos(cantidadCreada);
    }
}