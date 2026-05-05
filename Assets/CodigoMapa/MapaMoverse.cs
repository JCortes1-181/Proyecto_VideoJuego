using UnityEngine;

public class CamaraSigueX : MonoBehaviour
{
    public Transform jugador; // Arrastra a tu personaje aquí en el Inspector
    public float suavizado = 0.125f;
    public Vector2 limitesX = new Vector2(-10f, 10f); // Ajusta según el largo de tu mapa

    void LateUpdate()
    {
        if (jugador != null)
        {
            // Calculamos la nueva posición solo en X
            float nuevaX = Mathf.Lerp(transform.position.x, jugador.position.x, suavizado);
            
            // Opcional: Limitar la cámara para que no se salga de los bordes del dibujo
            nuevaX = Mathf.Clamp(nuevaX, limitesX.x, limitesX.y);

            transform.position = new Vector3(nuevaX, transform.position.y, transform.position.z);
        }
    }
}
