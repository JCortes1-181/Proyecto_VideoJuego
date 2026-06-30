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
    [Tooltip("Arrastra aquí todos los botones de los objetos que estarán escondidos en la escena")]
    public List<Button> objetosEscondidos;

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
        if (objetosEscondidos == null || objetosEscondidos.Count == 0)
        {
            Debug.LogError("¡No has puesto objetos escondidos en la lista del Inspector!");
            return;
        }

        int indiceAleatorio = Random.Range(0, objetosEscondidos.Count);
        objetoObjetivoActual = objetosEscondidos[indiceAleatorio];

        Image imagenDelObjeto = objetoObjetivoActual.GetComponent<Image>();
        if (imagenDelObjeto != null && imagenObjetivoVisual != null)
        {
            imagenObjetivoVisual.sprite = imagenDelObjeto.sprite;
            imagenObjetivoVisual.preserveAspect = true; 
        }

        if (textoInstrucciones != null)
        {
            textoInstrucciones.text = "Encuentra esto:";
            textoInstrucciones.color = Color.white;
        }

        foreach (Button botonObjeto in objetosEscondidos)
        {
            botonObjeto.onClick.RemoveAllListeners();

            Button botonActual = botonObjeto;

            botonObjeto.onClick.AddListener(() => OnObjetoClickeado(botonActual));
        }
    }

    private void MoverLinternaConElMouse()
    {
        if (linternaEfecto == null || canvasPrincipal == null) return;

        Vector2 posicionLocalMouse;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasPrincipal.transform as RectTransform,
            Input.mousePosition,
            canvasPrincipal.worldCamera,
            out posicionLocalMouse
        );

        linternaEfecto.anchoredPosition = posicionLocalMouse;
    }

    public void OnObjetoClickeado(Button botonPresionado)
    {
        if (juegoTerminado) return;

        if (botonPresionado == objetoObjetivoActual)
        {
            if (fuenteEfectos != null && sonidoClickCorrecto != null)
            {
                fuenteEfectos.PlayOneShot(sonidoClickCorrecto);
            }
            
            TerminarJuego(true);
        }
        else
        {
            if (fuenteEfectos != null && sonidoClickIncorrecto != null)
            {
                fuenteEfectos.PlayOneShot(sonidoClickIncorrecto);
            }
        }
    }

    public override void TerminarJuego(bool victoria)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (fuenteMusica != null) fuenteMusica.Stop();
        
        Cursor.visible = true; 

        if (victoria)
        {
            if (textoInstrucciones != null)
            {
                textoInstrucciones.text = "¡Lo encontraste!";
                textoInstrucciones.color = Color.green;
            }

            if (fuenteEfectos != null && sonidoVictoria != null)
            {
                fuenteEfectos.PlayOneShot(sonidoVictoria);
            }
        }
        else
        {
            if (textoInstrucciones != null)
            {
                textoInstrucciones.text = "Se agotó el tiempo...";
                textoInstrucciones.color = Color.red;
            }

            if (fuenteEfectos != null && sonidoDerrota != null)
            {
                fuenteEfectos.PlayOneShot(sonidoDerrota);
            }
        }

        StartCoroutine(EsperarYRegresar(victoria));
    }
}