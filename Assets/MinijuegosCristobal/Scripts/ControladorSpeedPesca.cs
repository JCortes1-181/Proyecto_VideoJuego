using UnityEngine;
using UnityEngine.UI;

public class ControladorSpeedPesca : MinijuegoBase
{
    [Header("UI Elementos (RectTransform)")]
    public RectTransform contenedorBarra; 
    public RectTransform barraJugador;     
    public RectTransform marcadorObjetivo; 

    [Header("UI Barra de Miedo (Nuevo)")]
    [Tooltip("Arrastra aquí un Slider de Unity que represente el miedo de Speed")]
    public Slider barraMiedoUI;

    [Header("Configuración Físicas de Pesca")]
    public float gravedad = 750f;
    public float fuerzaFlasheoClic = 550f;
    public float velocidadMarcador = 0.7f; 
    
    [Header("Sistema de Miedo (Tolerancia)")]
    [Tooltip("Cuánto tiempo en segundos le toma a Speed asustarse al 100% y perder")]
    public float tiempoParaPerder = 1.5f; 
    private float miedoActual = 0f; // Empieza en 0 (sin miedo)

    [Header("Visuales de IShowSpeed")]
    public RectTransform imagenSpeed;      
    public Image componenteImagenSpeed;    
    public Sprite fotoSpeedBocaAbierta;    
    public GameObject objetoJumpscarePanel;

    private float posicionJugadorY = 0f;
    private float velocidadJugador = 0f;
    
    private float limiteInferior;
    private float limiteSuperior;

    protected override void Start()
    {
        // Configuramos el minijuego a 10 segundos
        tiempoLimite = 10f;
        base.Start(); 
        
        // Inicializamos el miedo en cero
        miedoActual = 0f;
        if (barraMiedoUI != null)
        {
            barraMiedoUI.maxValue = tiempoParaPerder;
            barraMiedoUI.value = 0f;
        }

        // Calculamos los límites reales basados en la altura del contenedor gris
        float altoContenedor = contenedorBarra.rect.height;
        float medioAltoContenedor = altoContenedor / 2f;
        
        limiteInferior = -medioAltoContenedor + (barraJugador.rect.height / 2f);
        limiteSuperior = medioAltoContenedor - (barraJugador.rect.height / 2f);

        // Iniciamos posiciones en el centro/piso de forma segura
        posicionJugadorY = limiteInferior;
        barraJugador.anchoredPosition = new Vector2(barraJugador.anchoredPosition.x, posicionJugadorY);
        marcadorObjetivo.anchoredPosition = new Vector2(marcadorObjetivo.anchoredPosition.x, limiteInferior);
    }

    protected override void Update()
    {
        // Controlamos el tiempo de forma manual para evitar la derrota automática de la base
        if (juegoTerminado) return;

        cronometro -= Time.deltaTime;
        
        // CONDICIÓN DE VICTORIA: Si sobreviviste los 10 segundos sin llenar la barra de miedo
        if (cronometro <= 0)
        {
            TerminarJuego(true); // ¡Ganaste!
            return;
        }

        // 1. MOVIMIENTO DEL MARCADOR ROJO
        float altoContenedor = contenedorBarra.rect.height;
        float medioAltoContenedor = altoContenedor / 2f;
        float rangoMarcadorY = medioAltoContenedor - (marcadorObjetivo.rect.height / 2f);
        
        float sinMovimiento = Mathf.Sin(Time.time * velocidadMarcador); 
        float yMarcador = sinMovimiento * rangoMarcadorY;
        marcadorObjetivo.anchoredPosition = new Vector2(marcadorObjetivo.anchoredPosition.x, yMarcador);

        // 2. MOVIMIENTO DE LA BARRA VERDE (Clic del mouse)
        if (Input.GetMouseButtonDown(0)) 
        {
            velocidadJugador = fuerzaFlasheoClic;
        }

        velocidadJugador -= gravedad * Time.deltaTime;
        posicionJugadorY += velocidadJugador * Time.deltaTime;

        if (posicionJugadorY < limiteInferior)
        {
            posicionJugadorY = limiteInferior;
            velocidadJugador = 0; 
        }
        else if (posicionJugadorY > limiteSuperior)
        {
            posicionJugadorY = limiteSuperior;
            velocidadJugador = 0; 
        }

        barraJugador.anchoredPosition = new Vector2(barraJugador.anchoredPosition.x, posicionJugadorY);

        // 3. SISTEMA DE MIEDO (¿Estás protegiendo el marcador rojo?)
        if (EstaSuperpuesto(barraJugador, marcadorObjetivo))
        {
            // Si estás encima, el miedo se reduce poco a poco
            miedoActual = Mathf.MoveTowards(miedoActual, 0f, Time.deltaTime * 0.8f);
        }
        else
        {
            // Si te sales de la zona, el miedo empieza a subir
            miedoActual += Time.deltaTime;

            // CONDICIÓN DE DERROTA: El miedo llegó al límite
            if (miedoActual >= tiempoParaPerder)
            {
                TerminarJuego(false); // Jumpscare
                return;
            }
        }

        // Actualizamos la barra visual en la interfaz
        if (barraMiedoUI != null)
        {
            barraMiedoUI.value = miedoActual;
        }

        // 4. TEMBLOR DINÁMICO EN BASE AL MIEDO ACTUAL
        float porcentajeMiedo = miedoActual / tiempoParaPerder;
        float factorTemblor = 3f + (porcentajeMiedo * 20f); 
        imagenSpeed.anchoredPosition = new Vector2(
            Random.Range(-factorTemblor, factorTemblor),
            Random.Range(-factorTemblor, factorTemblor)
        );
    }

    private bool EstaSuperpuesto(RectTransform r1, RectTransform r2)
    {
        Vector3[] esquinasR1 = new Vector3[4];
        r1.GetWorldCorners(esquinasR1);
        float r1MínimoY = esquinasR1[0].y;
        float r1MáximoY = esquinasR1[2].y;

        Vector3[] esquinasR2 = new Vector3[4];
        r2.GetWorldCorners(esquinasR2);
        float r2MínimoY = esquinasR2[0].y;
        float r2MáximoY = esquinasR2[2].y;

        return (r1MáximoY >= r2MínimoY && r1MínimoY <= r2MáximoY);
    }

    public override void TerminarJuego(bool victoria)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (victoria)
        {
            Debug.Log("¡VICTORIA! Speed aguantó los 10 segundos y salió vivo.");
            // Aquí puedes activar un texto en pantalla que diga "¡LOGRADO!" o un emoticón gracioso
        }
        else
        {
            Debug.Log("¡DERROTA! El miedo llegó al 100%. JUMPSCARE.");
            if (componenteImagenSpeed != null && fotoSpeedBocaAbierta != null)
            {
                componenteImagenSpeed.sprite = fotoSpeedBocaAbierta;
            }
            if (objetoJumpscarePanel != null)
            {
                objetoJumpscarePanel.SetActive(true);
            }
        }

        StartCoroutine(EsperarYRegresar(victoria));
    }
}