using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;

public class ControladorPopUps : MinijuegoBase
{
    [Header("UI Referencias")]
    public RectTransform areaCanvas;       
    public GameObject prefabPopUp;         
    public GameObject efectoVirusDerrota;  

    [Header("Sistema de Video (Victoria)")]
    public RawImage pantallaVideoUI;       
    public VideoPlayer reproductorVideo;   

    [Header("Variedad de Anuncios")]
    public Sprite[] imagenesAnuncios;

    [Header("Audio (Windows XP Style)")]
    public AudioSource fuenteAudio;        
    public AudioClip sonidoErrorXP;        
    public AudioClip sonidoVictoria;       

    [Header("Música de Fondo")]
    public AudioSource fuenteMusicaFondo; 

    [Header("Configuración del Caos")]
    public float tiempoEntrePopUps = 0.8f; 
    public int limiteAnunciosParaPerder = 5;

    private List<GameObject> listaPopUpsActivos = new List<GameObject>();
    private float temporizadorSpawn = 0f;
    private bool yaPerdio = false;

    protected override void Start()
    {
        tiempoLimite = 10f; 
        base.Start();

        efectoVirusDerrota.SetActive(false);
        
        if (pantallaVideoUI != null) pantallaVideoUI.gameObject.SetActive(false);
        if (reproductorVideo != null) reproductorVideo.Stop();

        if (fuenteAudio == null) fuenteAudio = GetComponent<AudioSource>();

        if (fuenteMusicaFondo != null && fuenteMusicaFondo.clip != null)
        {
            fuenteMusicaFondo.loop = true; 
            fuenteMusicaFondo.Play();
        }

        CrearNuevoPopUp();
    }

    protected override void Update()
    {
        if (juegoTerminado) return;

        cronometro -= Time.deltaTime;
        if (cronometro <= 0)
        {
            TerminarJuego(true); 
            return;
        }

        temporizadorSpawn += Time.deltaTime;
        if (temporizadorSpawn >= tiempoEntrePopUps)
        {
            CrearNuevoPopUp();
            temporizadorSpawn = 0f;
        }

        listaPopUpsActivos.RemoveAll(item => item == null);

        if (listaPopUpsActivos.Count >= limiteAnunciosParaPerder && !yaPerdio)
        {
            yaPerdio = true;
            StartCoroutine(CoSaturacionDerrota());
        }
    }

    private void CrearNuevoPopUp()
    {
        if (juegoTerminado) return;

        GameObject nuevoAnuncio = Instantiate(prefabPopUp, areaCanvas);
        listaPopUpsActivos.Add(nuevoAnuncio);

        if (imagenesAnuncios != null && imagenesAnuncios.Length > 0)
        {
            Image imagenFondo = nuevoAnuncio.GetComponent<Image>();
            if (imagenFondo != null)
            {
                int indiceRandom = Random.Range(0, imagenesAnuncios.Length);
                imagenFondo.sprite = imagenesAnuncios[indiceRandom];
            }
        }

        ReproducirSonidoError();

        float anchoMax = (areaCanvas.rect.width / 2f) - 80f; 
        float altoMax = (areaCanvas.rect.height / 2f) - 50f;

        float randomX = Random.Range(-anchoMax, anchoMax);
        float randomY = Random.Range(-altoMax, altoMax);

        RectTransform rectAnuncio = nuevoAnuncio.GetComponent<RectTransform>();
        rectAnuncio.anchoredPosition = new Vector2(randomX, randomY);

        Button botonCerrar = nuevoAnuncio.GetComponentInChildren<Button>();
        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(() => CerrarAnuncio(nuevoAnuncio));
        }

        // 🔥 EFECTO VISUAL: El pop-up nace desde tamaño 0 y se infla rápidamente
        rectAnuncio.localScale = Vector3.zero;
        StartCoroutine(AnimarEntradaPopUp(rectAnuncio));
    }

    // Corrutina para suavizar la entrada del anuncio
    private IEnumerator AnimarEntradaPopUp(RectTransform rect)
    {
        float tiempo = 0f;
        float duracion = 0.15f; // Qué tan rápido se expande (0.15 segundos)
        
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            if (rect == null) yield break;
            
            float progreso = tiempo / duracion;
            // Va desde 0 hasta 1 en escala
            rect.localScale = Vector3.Lerp(Vector3.zero, new Vector3(1f, 1f, 1f), progreso);
            yield return null;
        }
    }

    public void CerrarAnuncio(GameObject anuncioAEliminar)
    {
        if (juegoTerminado) return;

        listaPopUpsActivos.Remove(anuncioAEliminar);
        Destroy(anuncioAEliminar); 
    }

    private void ReproducirSonidoError()
    {
        if (fuenteAudio != null && sonidoErrorXP != null)
        {
            fuenteAudio.PlayOneShot(sonidoErrorXP);
        }
    }

    private IEnumerator CoSaturacionDerrota()
    {
        juegoTerminado = true;

        if (fuenteMusicaFondo != null) fuenteMusicaFondo.Stop();

        for (int i = 0; i < 20; i++)
        {
            GameObject clonCaos = Instantiate(prefabPopUp, areaCanvas);
            RectTransform rect = clonCaos.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(Random.Range(-250f, 250f), Random.Range(-180f, 180f));
            
            ReproducirSonidoError(); 
            yield return new WaitForSeconds(0.04f); 
        }

        yield return new WaitForSeconds(0.4f);
        TerminarJuego(false);
    }

    public override void TerminarJuego(bool victoria)
    {
        if (victoria)
        {
            Debug.Log("¡Victoria!");
            
            if (fuenteMusicaFondo != null) fuenteMusicaFondo.Stop();
            
            if (fuenteAudio != null && sonidoVictoria != null)
            {
                fuenteAudio.PlayOneShot(sonidoVictoria);
            }

            foreach (GameObject popUp in listaPopUpsActivos)
            {
                if (popUp != null) Destroy(popUp);
            }
            listaPopUpsActivos.Clear();

            if (pantallaVideoUI != null && reproductorVideo != null)
            {
                pantallaVideoUI.gameObject.SetActive(true);
                reproductorVideo.Play();
            }
        }
        else
        {
            if (fuenteMusicaFondo != null) fuenteMusicaFondo.Stop();
            if (efectoVirusDerrota != null) efectoVirusDerrota.SetActive(true); 
        }

        StartCoroutine(EsperarYRegresar(victoria));
    }
}