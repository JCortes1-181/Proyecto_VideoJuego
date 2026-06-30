using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControladorSpeedPesca : MinijuegoBase
{
    [Header("UI Elementos (RectTransform)")]
    public RectTransform contenedorBarra; 
    public RectTransform barraJugador;     
    public RectTransform marcadorObjetivo; 

    [Header("UI Barra de Miedo/Alerta (Slider)")]
    public Slider barraMiedoUI;

    [Header("Configuración Físicas de Silencio")]
    public float gravedadImpulso = 800f;    
    public float fuerzaSilencio = 900f;     
    public float velocidadMarcadorGrito = 0.7f; 
    
    [Header("Sistema de Alerta (Tolerancia)")]
    [Tooltip("Tiempo en segundos para que la criatura se despierte por completo")]
    public float tiempoParaPerder = 1.5f; 
    private float miedoActual = 0f; 

    [Header("Visuales de IShowSpeed")]
    public RectTransform rectSpeed;
    public Image imagenSpeed;      
    public Sprite fotoSpeedNormal;       
    public Sprite fotoSpeedHablando;     
    public Sprite fotoSpeedAsustado; 
    public Sprite fotoSpeedTranquilo;    

    [Header("Visuales del Monstruo (Hank)")]
    public Image imagenMonstruo;
    public Sprite monstruoDurmiendo;     
    public Sprite monstruoAlertado;      
    public Sprite monstruoDespierto;     
    public GameObject objetoJumpscarePanel;

    [Header("UI Textos Colectados")]
    public EfectoTituloFeria scriptTextoTitulo; 

    [Header("Sistema de Audio (.mp3)")]
    public AudioSource fuenteAudioEfectos;
    public AudioSource fuenteAudioMusica;    
    public AudioClip clipMusicaFondo;        
    public AudioClip sonidoGritoSpeed;       
    public AudioClip sonidoMonstruoAlerta;   
    public AudioClip sonidoVictoria;         
    public AudioClip sonidoJumpscareDerrota; 

    private float posicionJugadorY = 0f;
    private float velocidadJugador = 0f;
    private float limiteInferior;
    private float limiteSuperior;
    
    private Vector2 posicionInicialSpeed; 
    private bool estabaFallando = false;
    private bool cinematicaIniciada = false;

    protected override void Start()
    {
        tiempoLimite = 10f;
        base.Start(); 
        
        miedoActual = 0f;
        cinematicaIniciada = false;

        if (barraMiedoUI != null)
        {
            barraMiedoUI.maxValue = tiempoParaPerder;
            barraMiedoUI.value = 0f;
        }

        if (imagenSpeed != null && fotoSpeedNormal != null) imagenSpeed.sprite = fotoSpeedNormal;
        if (imagenMonstruo != null && monstruoDurmiendo != null) imagenMonstruo.sprite = monstruoDurmiendo;

        float altoContenedor = contenedorBarra.rect.height;
        float medioAltoContenedor = altoContenedor / 2f;
        
        limiteInferior = -medioAltoContenedor + (barraJugador.rect.height / 2f);
        limiteSuperior = medioAltoContenedor - (barraJugador.rect.height / 2f);

        posicionJugadorY = limiteInferior;
        barraJugador.anchoredPosition = new Vector2(barraJugador.anchoredPosition.x, posicionJugadorY);

        if (rectSpeed != null)
        {
            posicionInicialSpeed = rectSpeed.anchoredPosition;
        }

 
        if (fuenteAudioMusica != null && clipMusicaFondo != null)
        {
            fuenteAudioMusica.clip = clipMusicaFondo;
            fuenteAudioMusica.loop = true;
            fuenteAudioMusica.Play();
        }
    }

  protected override void Update()
{

    if (juegoTerminado || cinematicaIniciada) return;

    cronometro -= Time.deltaTime;
        
        if (cronometro <= 0)
        {
            TerminarJuego(true);
            return;
        }

        float altoContenedor = contenedorBarra.rect.height;
        float medioAltoContenedor = altoContenedor / 2f;
        float rangoMarcadorY = medioAltoContenedor - (marcadorObjetivo.rect.height / 2f);
        
        float sinMovimiento = Mathf.Sin(Time.time * velocidadMarcadorGrito); 
        float yMarcador = sinMovimiento * rangoMarcadorY;
        marcadorObjetivo.anchoredPosition = new Vector2(marcadorObjetivo.anchoredPosition.x, yMarcador);

        if (Input.GetMouseButton(0)) 
        {
            velocidadJugador += fuerzaSilencio * Time.deltaTime;
        }
        else
        {
            velocidadJugador -= gravedadImpulso * Time.deltaTime;
        }

        velocidadJugador = Mathf.Clamp(velocidadJugador, -600f, 600f);
        posicionJugadorY += velocidadJugador * Time.deltaTime;

        if (posicionJugadorY < limiteInferior) { posicionJugadorY = limiteInferior; velocidadJugador = 0; }
        else if (posicionJugadorY > limiteSuperior) { posicionJugadorY = limiteSuperior; velocidadJugador = 0; }

        barraJugador.anchoredPosition = new Vector2(barraJugador.anchoredPosition.x, posicionJugadorY);


       if (EstaSuperpuesto(barraJugador, marcadorObjetivo))
        {
            miedoActual = Mathf.MoveTowards(miedoActual, 0f, Time.deltaTime * 0.8f);
            
            if (estabaFallando)
            {
                if (imagenSpeed != null && fotoSpeedNormal != null) imagenSpeed.sprite = fotoSpeedNormal;
                
                if (fuenteAudioEfectos != null) 
                    fuenteAudioEfectos.Stop();

                estabaFallando = false;
            }
        }
        else
        {
            miedoActual += Time.deltaTime;

            if (miedoActual >= tiempoParaPerder)
            {
                miedoActual = tiempoParaPerder; 
                StartCoroutine(CinematicaDerrotaSecuencial());
                return;
            }

            if (!estabaFallando && !cinematicaIniciada && !juegoTerminado)
            {
                if (imagenSpeed != null && fotoSpeedHablando != null) imagenSpeed.sprite = fotoSpeedHablando;
                
                if (fuenteAudioEfectos != null && sonidoGritoSpeed != null)
                {
                    fuenteAudioEfectos.clip = sonidoGritoSpeed;
                    fuenteAudioEfectos.Play();
                }
                estabaFallando = true;
            }
        

            if (miedoActual >= tiempoParaPerder)
            {
                StartCoroutine(CinematicaDerrotaSecuencial());
                return;
            }
        }

        if (barraMiedoUI != null) barraMiedoUI.value = miedoActual;

        float porcentajeMiedo = miedoActual / tiempoParaPerder;
        if (porcentajeMiedo > 0.4f && porcentajeMiedo < 1f)
        {
            if (imagenMonstruo != null && monstruoAlertado != null && imagenMonstruo.sprite != monstruoAlertado)
            {
                imagenMonstruo.sprite = monstruoAlertado;
                
                if (fuenteAudioEfectos != null && sonidoMonstruoAlerta != null && !fuenteAudioEfectos.isPlaying)
                    fuenteAudioEfectos.PlayOneShot(sonidoMonstruoAlerta);
            }
        }
        else if (porcentajeMiedo <= 0.4f)
        {
            if (imagenMonstruo != null && monstruoDurmiendo != null) imagenMonstruo.sprite = monstruoDurmiendo;
        }

        float temblorBase = 8f; 
        float temblorMaximo = 32f;
        float factorTemblor = temblorBase + (porcentajeMiedo * temblorMaximo); 

        if (rectSpeed != null)
        {
            rectSpeed.anchoredPosition = posicionInicialSpeed + new Vector2(
                Random.Range(-factorTemblor, factorTemblor),
                Random.Range(-factorTemblor, factorTemblor)
            );
        }
    }

    private bool EstaSuperpuesto(RectTransform r1, RectTransform r2)
    {
        Vector3[] esquinasR1 = new Vector3[4]; r1.GetWorldCorners(esquinasR1);
        Vector3[] esquinasR2 = new Vector3[4]; r2.GetWorldCorners(esquinasR2);
        return (esquinasR1[2].y >= esquinasR2[0].y && esquinasR1[0].y <= esquinasR2[2].y);
    }

    private IEnumerator CinematicaDerrotaSecuencial()
{
        cinematicaIniciada = true;
        juegoTerminado = true;

        if (fuenteAudioMusica != null) fuenteAudioMusica.Stop();
        if (fuenteAudioEfectos != null) fuenteAudioEfectos.Stop();

        if (imagenSpeed != null && fotoSpeedHablando != null) 
            imagenSpeed.sprite = fotoSpeedHablando;
            

        yield return new WaitForSeconds(1.2f); 

        if (imagenMonstruo != null && monstruoDespierto != null) 
            imagenMonstruo.sprite = monstruoDespierto;
            
        if (fuenteAudioEfectos != null && sonidoMonstruoAlerta != null) 
            fuenteAudioEfectos.PlayOneShot(sonidoMonstruoAlerta);

        yield return new WaitForSeconds(1.0f); 

        if (imagenSpeed != null && fotoSpeedAsustado != null) 
            imagenSpeed.sprite = fotoSpeedAsustado;

        if (scriptTextoTitulo != null)
        {
            scriptTextoTitulo.CambiarTextoDerrota();
        }

        if (objetoJumpscarePanel != null) 
            objetoJumpscarePanel.SetActive(true);

        if (fuenteAudioEfectos != null && sonidoJumpscareDerrota != null) 
            fuenteAudioEfectos.PlayOneShot(sonidoJumpscareDerrota);

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(EsperarYRegresar(false));
    }
    public override void TerminarJuego(bool victoria)
    {
        if (victoria)
        {
            if (juegoTerminado) return;
            juegoTerminado = true;

            if (fuenteAudioMusica != null) fuenteAudioMusica.Stop();
            if (fuenteAudioEfectos != null) fuenteAudioEfectos.Stop();
            
            if (imagenSpeed != null && fotoSpeedTranquilo != null) imagenSpeed.sprite = fotoSpeedTranquilo;
            if (imagenMonstruo != null && monstruoDurmiendo != null) imagenMonstruo.sprite = monstruoDurmiendo;
            if (fuenteAudioEfectos != null && sonidoVictoria != null) fuenteAudioEfectos.PlayOneShot(sonidoVictoria);
            
            StartCoroutine(EsperarYRegresar(true));
        }
        else
        {
            if (!cinematicaIniciada) StartCoroutine(CinematicaDerrotaSecuencial());
        }
    }
}