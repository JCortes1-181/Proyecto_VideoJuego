using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ControladorPou : MinijuegoBase
{
    [Header("UI Textos")]
public TextMeshProUGUI textoPuntaje;
public TextMeshProUGUI textoCronometro;
public EfectoTituloFeria scriptTextoTitulo; 

    [Header("Lista de Pous")]
    public List<ObjetoPou> listaPous;

    [Header("Configuración del Juego")]
    public int puntosParaGanar = 8;
    public float tiempoVisiblePou = 0.6f; 
    public float tiempoEntreSpawns = 0.5f; 

    [Header("Música y Sonidos Globales (Nuevo)")]
    public AudioSource fuenteAudioMusica;
    public AudioSource fuenteAudioEfectos;
    public AudioClip sonidoVictoria;      
    public AudioClip sonidoDerrotaDiabolica; 

    private int puntajeActual = 0;
    private ObjetoPou pouActivoActual;

    protected override void Start()
    {
        base.Start();

        puntajeActual = 0;
        ActualizarInterfaz();

        if (fuenteAudioMusica != null && !fuenteAudioMusica.isPlaying)
        {
            fuenteAudioMusica.loop = true;
            fuenteAudioMusica.Play();
        }

        foreach (ObjetoPou pou in listaPous)
        {
            if (pou != null) pou.gameObject.SetActive(false);
        }

        StartCoroutine(CoCicloJuego());
    }

    private void Update()
    {
        if (juegoTerminado) return;

        tiempoLimite -= Time.deltaTime;

        if (tiempoLimite <= 0f)
        {
            tiempoLimite = 0f;
            TerminarJuego(false); 
        }

        if (textoCronometro != null)
        {
            textoCronometro.text = "Tiempo: " + Mathf.CeilToInt(tiempoLimite).ToString() + "s";
        }
    }

    private IEnumerator CoCicloJuego()
    {
        while (!juegoTerminado)
        {
            yield return new WaitForSeconds(tiempoEntreSpawns);

            if (juegoTerminado) yield break;

            int indiceAlzar = Random.Range(0, listaPous.Count);
            pouActivoActual = listaPous[indiceAlzar];

            if (pouActivoActual != null)
            {
                pouActivoActual.ActivarPou();

                float temporizadorVisible = 0f;
                while (temporizadorVisible < tiempoVisiblePou && pouActivoActual.gameObject.activeSelf && !juegoTerminado)
                {
                    temporizadorVisible += Time.deltaTime;
                    yield return null;
                }

                if (pouActivoActual.gameObject.activeSelf)
                {
                    pouActivoActual.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SumarPunto()
{
    if (juegoTerminado) return;

    puntajeActual++;
    ActualizarInterfaz();

    if (puntajeActual >= puntosParaGanar)
    {
        TerminarJuego(true);
    }
}

    private void ActualizarInterfaz()
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = "Pous: " + puntajeActual + " / " + puntosParaGanar;
        }
    }

    [Header("UI Derrota Especial ( Nuevo)")]
[Tooltip("Arrastra aquí el objeto del Celular con el Pou Diabólico")]
public GameObject panelPouDiabolico; 

public override void TerminarJuego(bool victoria)
{
    if (juegoTerminado) return;
    juegoTerminado = true;

    StopAllCoroutines();

    if (fuenteAudioMusica != null)
    {
        fuenteAudioMusica.Stop();
    }

    if (!victoria)
    {
        foreach (ObjetoPou pou in listaPous)
        {
            if (pou != null) pou.gameObject.SetActive(false);
        }

        if (panelPouDiabolico != null)
        {
            panelPouDiabolico.SetActive(true);
        }

        if (fuenteAudioEfectos != null && sonidoDerrotaDiabolica != null)
        {
            fuenteAudioEfectos.PlayOneShot(sonidoDerrotaDiabolica);
        }

        Debug.Log("¡Perdiste! 'Los voy a matar' activado con jumpscare.");
    }
    else
    {
        foreach (ObjetoPou pou in listaPous)
        {
            if (pou != null && pou.gameObject.activeSelf && pou.GetComponent<Button>().interactable) 
            {
                pou.gameObject.SetActive(false);
            }
        }

        if (scriptTextoTitulo != null)
        {
            scriptTextoTitulo.CambiarTextoVictoria();
        }

        if (fuenteAudioEfectos != null && sonidoVictoria != null)
        {
            fuenteAudioEfectos.PlayOneShot(sonidoVictoria);
        }

        Debug.Log("¡Ganaste! Limpiaste el jardín.");
    }

    StartCoroutine(EsperarYRegresar(victoria));
}
}