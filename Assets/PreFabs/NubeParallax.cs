using UnityEngine;

public class NubeParallax : MonoBehaviour
{
    public float velocidad; 
    public float puntoReinicioX = -15f; 
    public float puntoAparicionX = 15f; 

    void Update()
    {
        
        transform.Translate(Vector3.left * velocidad * Time.deltaTime);

        
        if (transform.position.x <= puntoReinicioX)
        {
            Vector3 nuevaPos = new Vector3(puntoAparicionX, transform.position.y, transform.position.z);
            transform.position = nuevaPos;
        }
    }
}