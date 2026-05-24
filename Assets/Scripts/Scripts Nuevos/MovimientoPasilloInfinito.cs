using UnityEngine;

public class MovimientoPasilloInfinito : MonoBehaviour
{
    public float velocidad = 5f;
    public float anchoImagen = 24.54f; 

    void Update()
    {
        // Movimiento constante a la izquierda
        transform.Translate(Vector3.left * velocidad * Time.deltaTime);

        // Si la imagen sale del área, se teletransporta al final de la otra
        if (transform.position.x < -anchoImagen)
        {
            transform.position = new Vector3(transform.position.x + (anchoImagen * 2), transform.position.y, transform.position.z);
        }
    }
}