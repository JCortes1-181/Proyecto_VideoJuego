using UnityEngine;

public class NubeParallax : MonoBehaviour
{
    public float velocidad; // Velocidad lenta para efecto de fondo
    public float puntoReinicioX = -15f; // Donde desaparece
    public float puntoAparicionX = 15f; // Donde vuelve a salir

    void Update()
    {
        // Movimiento constante a la izquierda
        transform.Translate(Vector3.left * velocidad * Time.deltaTime);

        // Si la nube sale de la pantalla, la teletransportamos al inicio
        if (transform.position.x <= puntoReinicioX)
        {
            Vector3 nuevaPos = new Vector3(puntoAparicionX, transform.position.y, transform.position.z);
            transform.position = nuevaPos;
        }
    }
}