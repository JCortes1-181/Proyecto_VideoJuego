using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video; // Requerido para controlar videos reales
using System.Collections;
using System.Collections.Generic;

public class ControladorPopUps : MinijuegoBase
{
    [Header("UI Referencias")]
    public RectTransform areaCanvas;       // Arrastra aquí tu 'Contenedor_Anuncios'
    public GameObject prefabPopUp;         
    public GameObject efectoVirusDerrota;  // El panel que solo tapa la pantalla del PC

    [Header("Sistema de Video (Victoria)")]
    public RawImage pantallaVideoUI;       // El componente RawImage donde se proyecta el video
    public VideoPlayer reproductorVideo;   // El componente VideoPlayer que reproduce el archivo

    [Header("Variedad de Anuncios")]
    public Sprite[] imagenesAnuncios;

    [Header("Audio (Windows XP Style)")]
    public AudioSource fuenteAudio;        
    public AudioClip sonidoErrorXP;        

    [Header("Configuración del Caos")]
    public float tiempoEntrePopUps = 0.8f; 
    public int limiteAnunciosParaPerder = 5;

    private List<GameObject> listaPopUpsActivos = new List<GameObject>();
    private float temporizadorSpawn = 0f;
    private bool yaPerdio = false;

    protected override void Start()
    {
        tiempoLimite = 10f; // 10 segundos resistiendo
        base.Start();

        efectoVirusDerrota.SetActive(false);
        
        // El video empieza oculto/pausado hasta que ganes
        if (pantallaVideoUI != null) pantallaVideoUI.gameObject.SetActive(false);
        if (reproductorVideo != null) reproductorVideo.Stop();

        if (fuenteAudio == null) fuenteAudio = GetComponent<AudioSource>();

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

        // Cambiar la imagen al azar
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

        // Posicionamiento calculado SOLO dentro del Contenedor_Anuncios
        float anchoMax = (areaCanvas.rect.width / 2f) - 80f; 
        float altoMax = (areaCanvas.rect.height / 2f) - 50f;

        float randomX = Random.Range(-anchoMax, anchoMax);
        float randomY = Random.Range(-altoMax, altoMax);

        RectTransform rectAnuncio = nuevoAnuncio.GetComponent<RectTransform>();
        rectAnuncio.anchoredPosition = new Vector2(randomX, randomY);

        // Configurar botón cerrar
        Button botonCerrar = nuevoAnuncio.GetComponentInChildren<Button>();
        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(() => CerrarAnuncio(nuevoAnuncio));
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

    // CORRUTINA: Genera anuncios persas que colapsan la pantalla del PC
    private IEnumerator CoSaturacionDerrota()
    {
        juegoTerminado = true; // Pausamos el flujo normal del minijuego

        for (int i = 0; i < 20; i++)
        {
            GameObject clonCaos = Instantiate(prefabPopUp, areaCanvas);
            RectTransform rect = clonCaos.GetComponent<RectTransform>();
            
            // Spawnea por todo el monitor
            rect.anchoredPosition = new Vector2(Random.Range(-250f, 250f), Random.Range(-180f, 180f));
            
            ReproducirSonidoError(); 
            yield return new WaitForSeconds(0.04f); // Velocidad supersónica de ruidos
        }

        yield return new WaitForSeconds(0.4f);
        TerminarJuego(false);
    }

    public override void TerminarJuego(bool victoria)
    {
        if (victoria)
        {
            Debug.Log("¡Victoria! Limpiando anuncios y reproduciendo serie.");
            
            foreach (GameObject popUp in listaPopUpsActivos)
            {
                if (popUp != null) Destroy(popUp);
            }
            listaPopUpsActivos.Clear();

            // ACTIVAR Y REPRODUCIR EL VIDEO
            if (pantallaVideoUI != null && reproductorVideo != null)
            {
                pantallaVideoUI.gameObject.SetActive(true);
                reproductorVideo.Play();
            }
        }
        else
        {
            Debug.Log("Infección en el PC.");
            if (efectoVirusDerrota != null)
            {
                efectoVirusDerrota.SetActive(true); 
            }
        }

        StartCoroutine(EsperarYRegresar(victoria));
    }
}