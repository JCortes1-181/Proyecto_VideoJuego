using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ControladorBuscarObjetos : MinijuegoBase
{
    [Header("UI Componentes")]
    public TextMeshProUGUI textoInstrucciones; 
    public Image imagenObjetivoVisual;         
    public RectTransform linternaEfecto;       
    public Canvas canvasPrincipal;             

    [Header("Configuración del Juego")]
    public float tiempoParaBuscar = 12f;       

    [Header("Objetos Ocultos en la Habitación")]
    public List<Button> objetosEscondidos;

    [Header("Límites de Aparición Aleatoria")]
    [Tooltip("Margen en píxeles para que los objetos no aparezcan pegados a las esquinas del Canvas")]
    public float margenBorde = 100f;

    [Header("Sistema de Audio (.mp3)")]
    public AudioSource fuenteEfectos;
    public AudioSource fuenteMusica;
    [Space]
    public AudioClip clipMusicaAmbiente;
    public AudioClip sonidoClickCorrecto;
    public AudioClip sonidoClickIncorrecto;
    public AudioClip sonidoVictoria;
    public AudioClip sonidoDerrota;

    private Button objetoObjetivoActual; 

    protected override void Start()
    {
        tiempoLimite = tiempoParaBuscar;
        base.Start();

        if (fuenteMusica != null && clipMusicaAmbiente != null)
        {
            fuenteMusica.clip = clipMusicaAmbiente;
            fuenteMusica.loop = true;
            fuenteMusica.Play();
        }

        ConfigurarRonda();
    }

    private void Update()
    {
        if (juegoTerminado) return;
        MoverLinternaConElMouse();
    }

    private void ConfigurarRonda()
{
    if (canvasPrincipal == null)
    {
        Debug.LogError("¡Falta asignar el Canvas Principal en el GameManager_Buscar!");
        return;
    }
    if (objetosEscondidos == null || objetosEscondidos.Count == 0)
    {
        Debug.LogError("¡La lista de Objetos Escondidos está vacía!");
        return;
    }

    if (linternaEfecto != null)
    {
        linternaEfecto.gameObject.SetActive(true);
    }

    RectTransform rectCanvas = canvasPrincipal.GetComponent<RectTransform>();
    float anchoMedio = rectCanvas.rect.width / 2f;
    float altoMedio = rectCanvas.rect.height / 2f;
    float anchoPanelDerecho = 450f; 
    foreach (Button botonObjeto in objetosEscondidos)
    {
        if (botonObjeto == null) continue;

        RectTransform rectBoton = botonObjeto.GetComponent<RectTransform>();
        if (rectBoton != null)
        {
            float randomX = Random.Range(-anchoMedio + margenBorde, anchoMedio - anchoPanelDerecho);
            float randomY = Random.Range(-altoMedio + margenBorde, altoMedio - margenBorde);
            rectBoton.anchoredPosition = new Vector2(randomX, randomY);

            ObjetoMovil movimiento = botonObjeto.GetComponent<ObjetoMovil>();
            if (movimiento == null)
            {
                movimiento = botonObjeto.gameObject.AddComponent<ObjetoMovil>();
            }
            movimiento.enabled = true;
            movimiento.Inicializar(rectCanvas, anchoPanelDerecho);
        }
    }

    int indiceAleatorio = Random.Range(0, objetosEscondidos.Count);
    objetoObjetivoActual = objetosEscondidos[indiceAleatorio];

    if (objetoObjetivoActual != null)
    {
        Image imagenDelObjeto = objetoObjetivoActual.GetComponent<Image>();
        if (imagenDelObjeto != null && imagenObjetivoVisual != null)
        {
            imagenObjetivoVisual.sprite = imagenDelObjeto.sprite;
            imagenObjetivoVisual.preserveAspect = true;
            imagenObjetivoVisual.color = Color.white; 
        }
    }

    if (textoInstrucciones != null)
    {
        textoInstrucciones.text = "Encuentra esto:";
        textoInstrucciones.color = Color.white;
    }

    foreach (Button botonObjeto in objetosEscondidos)
    {
        if (botonObjeto == null) continue;
        botonObjeto.onClick.RemoveAllListeners();
        Button botonActual = botonObjeto;
        botonObjeto.onClick.AddListener(() => OnObjetoClickeado(botonActual));
    }
}
    private void MoverLinternaConElMouse()
    {
        if (linternaEfecto == null || canvasPrincipal == null) return;

        Vector2 posicionPantallaMouse = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasPrincipal.transform as RectTransform,
            posicionPantallaMouse,
            canvasPrincipal.worldCamera,
            out Vector2 posicionConvertidaCanvas
        );

        linternaEfecto.anchoredPosition = posicionConvertidaCanvas;
    }

    public void OnObjetoClickeado(Button botonPresionado)
    {
        if (juegoTerminado) return;

        if (botonPresionado == objetoObjetivoActual)
        {
            if (fuenteEfectos != null && sonidoClickCorrecto != null) fuenteEfectos.PlayOneShot(sonidoClickCorrecto);
            TerminarJuego(true);
        }
        else
        {
            if (fuenteEfectos != null && sonidoClickIncorrecto != null) fuenteEfectos.PlayOneShot(sonidoClickIncorrecto);
        }
    }

    public override void TerminarJuego(bool victoria)
    {
    if (juegoTerminado) return;
    juegoTerminado = true;

    if (linternaEfecto != null)
    {
        linternaEfecto.gameObject.SetActive(false); 
    }

    foreach (Button botonObjeto in objetosEscondidos)
    {
        if (botonObjeto == null) continue;
        ObjetoMovil movimiento = botonObjeto.GetComponent<ObjetoMovil>();
        if (movimiento != null) movimiento.enabled = false;
    }

    if (fuenteMusica != null) fuenteMusica.Stop();

    if (victoria)
    {
        if (textoInstrucciones != null)
        {
            textoInstrucciones.text = "¡Lo encontraste!";
            textoInstrucciones.color = Color.green;
        }
        if (fuenteEfectos != null && sonidoVictoria != null) fuenteEfectos.PlayOneShot(sonidoVictoria);
    }
    else
    {
        if (textoInstrucciones != null)
        {
            textoInstrucciones.text = "Se ocultó en la oscuridad...";
            textoInstrucciones.color = Color.red;
        }
        if (fuenteEfectos != null && sonidoDerrota != null) fuenteEfectos.PlayOneShot(sonidoDerrota);
    }

    StartCoroutine(EsperarYRegresar(victoria));
}
}