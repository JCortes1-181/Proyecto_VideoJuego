using UnityEngine;

public class AnimacionFlotante : MonoBehaviour
{
    public float amplitud = 0.2f; 
    public float velocidad = 3f;  

    private Vector3 posicionInicial;

    void Start()
    {
        
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        
        float nuevoY = Mathf.Sin(Time.time * velocidad) * amplitud;

        
        transform.localPosition = posicionInicial + new Vector3(0, nuevoY, 0);
    }
}