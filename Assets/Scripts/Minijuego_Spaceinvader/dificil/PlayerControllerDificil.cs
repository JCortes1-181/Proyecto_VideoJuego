using UnityEngine;

public class PlayerControllerDificil : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [Header("Disparo")]
    public GameObject balaPrefab; 
    public Transform puntoDisparo; 
    
    [Header("Conexiones")]
    public ControladorSpaceDificil controladorJuego;

    private Rigidbody2D rb2d;
    private Vector2 movement;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement.x = Input.GetAxis("Horizontal"); 
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Disparar();
        }
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + movement * speed * Time.fixedDeltaTime);
    }

    void Disparar()
    {
        Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
    }
//h
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigos"))
        {
            if(controladorJuego != null)
            {
                controladorJuego.JugadorTocado();
            }
            gameObject.SetActive(false);
        }
    }
}