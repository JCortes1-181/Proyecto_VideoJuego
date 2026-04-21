using UnityEngine;

public class AtaqueTenedor : MonoBehaviour
{
    public RectTransform tenedor; // Arrastra el Tenedor aquí
    public RectTransform sombra;  // Arrastra la Sombra aquí
    public float velocidadCaida = 800f;
    
    private Vector2 posicionInicialTenedor;
    private bool atacando = false;

    void OnEnable()
    {
        // Esto se activa cada vez que el minijuego empieza
        posicionInicialTenedor = tenedor.anchoredPosition;
        PrepararAtaque();
    }

    void PrepararAtaque()
    {
        // 1. Movemos la sombra a un lugar aleatorio a la izquierda o derecha
        float randomX = Random.Range(-150f, 150f);
        sombra.anchoredPosition = new Vector2(randomX, sombra.anchoredPosition.y);
        
        // 2. Ponemos el tenedor justo arriba de la sombra (pero fuera de cámara)
        tenedor.anchoredPosition = new Vector2(randomX, posicionInicialTenedor.y);
        
        // 3. Esperamos 1 segundo antes de que caiga (para que el jugador reaccione)
        Invoke("Caer", 1.0f);
    }

    void Caer()
    {
        atacando = true;
    }

    void Update()
    {
        if (atacando)
        {
            // El tenedor baja rápido
            tenedor.anchoredPosition -= new Vector2(0, velocidadCaida * Time.deltaTime);

            // Si el tenedor llega al suelo (donde está la sombra)
            if (tenedor.anchoredPosition.y <= sombra.anchoredPosition.y)
            {
                atacando = false;
                VerificarImpacto();
                
                // Esperamos un poco y repetimos el ataque
                Invoke("PrepararAtaque", 1.5f);
            }
        }
    }

public RectTransform miniPlayer; // Arrastra al MiniPlayer aquí

    // En AtaqueTenedor.cs, cambia la función VerificarImpacto:

// En AtaqueTenedor.cs, cambia la función VerificarImpacto:

void VerificarImpacto()
{
    float distancia = Mathf.Abs(tenedor.anchoredPosition.x - miniPlayer.anchoredPosition.x);

    if (distancia < 40f)
    {
        Debug.Log("¡TE DIO!");
        // Detenemos el ataque para que no siga cayendo
        atacando = false; 
        CancelInvoke(); // Detiene los próximos ataques programados

        // Le avisamos a la lógica central que PERDIMOS esta ronda
        Object.FindFirstObjectByType<LogicaPelea>().FalloEnMinijuego();
    }
    else
    {
        // Si no te dio, el Invoke("PrepararAtaque", 1.5f) que ya tienes seguirá funcionando
    }
}
}