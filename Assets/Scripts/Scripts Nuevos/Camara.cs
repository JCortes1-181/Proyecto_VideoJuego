using UnityEngine;

public class Camara : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform objetivo;
    public float velocidadcamara = 0.025f;
    public Vector3 desplazamientoC;

    private void LateUpdate()
    {
      Vector3 posicionDeseada= objetivo.position + desplazamientoC;
      Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadcamara);
      transform.position= posicionSuavizada;  
    }

}
