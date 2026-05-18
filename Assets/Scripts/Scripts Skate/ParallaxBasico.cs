using UnityEngine;

public class ParallaxSencillo : MonoBehaviour
{
    public Transform camara;
    public float velocidadParallax; // Prueba con 0.1 o 0.2
    private Vector3 ultimaPosCamara;

    void Start() {
        if (camara == null) camara = Camera.main.transform;
        ultimaPosCamara = camara.position;
    }

    void LateUpdate() {
        Vector3 movimientoCamara = camara.position - ultimaPosCamara;
        // Movemos el fondo un porcentaje de lo que se mueve la cámara
        transform.position += new Vector3(movimientoCamara.x * velocidadParallax, 0, 0);
        ultimaPosCamara = camara.position;
    }
}
