using UnityEngine;

public class SeguirObjetivo : MonoBehaviour
{
    public Transform objetivo; 
    public Vector3 offset = new Vector3(0, 1.5f, 0); 

    void Update()
    {
        if (objetivo != null)
        {
            
            transform.position = objetivo.position + offset;
        }
    }
}