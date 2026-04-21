using UnityEngine;

public class EfectoCaricatura : MonoBehaviour
{
    public float intensidad = 0.1f; 
    public float velocidad = 4f;   

    private Vector3 escalaInicial;

    void Start()
    {
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        
        float estiramiento = Mathf.Sin(Time.time * velocidad) * intensidad;

        
        transform.localScale = new Vector3(
            escalaInicial.x - (estiramiento * 0.5f), 
            escalaInicial.y + estiramiento, 
            escalaInicial.z
        );
    }
}