using UnityEngine;

public class Moverse_Mapa2d : MonoBehaviour
{
    public float velocidad = 5f;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      float velocidadX= Input.GetAxis("Horizontal")*Time.deltaTime*velocidad;
      animator.SetFloat("Movimiento", velocidadX*velocidad);
      if (velocidadX < 0)
        {
            transform.localScale= new Vector3(-1,1,1);
        }
        if (velocidadX > 0)
        {
            transform.localScale=new Vector3(1,1,1);
        }
      Vector3 posicion = transform.position;
      transform.position=new Vector3(velocidadX + posicion.x,posicion.y, posicion.z);  
    }
}
