using UnityEngine;

public class MovimientoMiniJuego : MonoBehaviour
{
    public float velocidad = 300f;
    public float limiteX = 180f; 

    void Update()
    {
        
        float mover = Input.GetAxis("Horizontal");
        
        
        Vector2 nuevaPos = transform.localPosition;
        nuevaPos.x += mover * velocidad * Time.deltaTime;

        
        nuevaPos.x = Mathf.Clamp(nuevaPos.x, -limiteX, limiteX);

        transform.localPosition = nuevaPos;
    }
}