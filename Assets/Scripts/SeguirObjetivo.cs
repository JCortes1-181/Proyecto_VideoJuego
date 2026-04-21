using UnityEngine;

public class SeguirObjetivo : MonoBehaviour
{
    public Transform objetivo; // Aquí arrastraremos al Perro
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Para que flote arriba de su cabeza

    void Update()
    {
        if (objetivo != null)
        {
            // La X copia la POSICIÓN del perro, pero NO su escala (el estiramiento)
            transform.position = objetivo.position + offset;
        }
    }
}