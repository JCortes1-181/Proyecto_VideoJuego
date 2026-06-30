using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ControladorGaleriaPatos : MinijuegoBase
{
    [Header("UI Componentes")]
    public TextMeshProUGUI textoInstrucciones;
    public TextMeshProUGUI textoContadorMalos; 

    [Header("Configuración de la Partida")]
    public int patosMalosParaGanar = 5;
    [Tooltip("Tiempo mínimo de espera entre patos")]
    public float tiempoEntrePatos = 0.6f; 
    
    [Header("Movimiento Elástico de Feria")]
    public float velocidadAvance = 250f; 
    public float velocidadBalanceo = 5f;
    public float amplitudBalanceo = 30f;
    public float rotacionMaxima = 15f;

    [Tooltip("Separación mínima en píxeles entre un pato y el siguiente para que no se amontonen")]
    public float separacionMinima = 180f;

    [Header("Prefabs de los Patos")]
    public GameObject prefabPatoMalvado;
    public GameObject prefabPatoBueno; 
    public RectTransform contenedorPatos; 

    [Header("Variedad de Visuales")]
    public Sprite[] fotosPatosBuenos; 

    [Header("Sistema de Audio (.mp3)")]
    public AudioSource fuenteEfectos;   
    public AudioSource fuenteMusica;    
    [Space]
    public AudioClip clipMusicaFondo;
    public AudioClip sonidoDisparoGeneral;
    public AudioClip sonidoPatoMalvadoMuere;
    public AudioClip sonidoPatoBuenoMuere; 
    public AudioClip sonidoVictoria;
    public AudioClip sonidoDerrota;

    private int patosMalosEliminados = 0;
    private List<GameObject> patosActivos = new List<GameObject>();
    private Dictionary<GameObject, float> tiemposDeNacimiento = new Dictionary<GameObject, float>();

    private float posicionXUltimoPato = 0f;

    protected override void Start()
    {
        tiempoLimite = 8f;
        base.Start();

        patosMalosEliminados = 0;
        ActualizarInterfaz();

        if (textoInstrucciones != null)
        {
            textoInstrucciones.text = "¡Dispara a los patos MALOS! Evita los buenos.";
            textoInstrucciones.color = Color.white;
        }

        if (fuenteMusica != null && clipMusicaFondo != null)
        {
            fuenteMusica.clip = clipMusicaFondo;
            fuenteMusica.loop = true;
            fuenteMusica.Play();
        }

        StartCoroutine(SpawnPatosLoop());
    }

    private void Update()
    {
        if (juegoTerminado) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (fuenteEfectos != null && sonidoDisparoGeneral != null)
            {
                fuenteEfectos.PlayOneShot(sonidoDisparoGeneral);
            }
        }

        MoverYBalancearPatos();
    }

    private IEnumerator SpawnPatosLoop()
    {
        while (!juegoTerminado)
        {
            yield return new WaitForSeconds(tiempoEntrePatos);

            if (juegoTerminado) break;

            float anchoContenedor = contenedorPatos.rect.width;
            float puntoDeSpawnX = anchoContenedor / 2f + 100f;

            if (patosActivos.Count > 0 && patosActivos[patosActivos.Count - 1] != null)
            {
                RectTransform ultimoRect = patosActivos[patosActivos.Count - 1].GetComponent<RectTransform>();
                if (ultimoRect != null)
                {
                    if (puntoDeSpawnX - ultimoRect.anchoredPosition.x < separacionMinima)
                    {
                        continue; 
                    }
                }
            }

            GameObject prefabElegido = (Random.value > 0.3f) ? prefabPatoMalvado : prefabPatoBueno;

            if (prefabElegido != null && contenedorPatos != null)
            {
                GameObject nuevoPato = Instantiate(prefabElegido, contenedorPatos);
                patosActivos.Add(nuevoPato);
                
                tiemposDeNacimiento.Add(nuevoPato, Time.time);

                if (prefabElegido == prefabPatoBueno && fotosPatosBuenos != null && fotosPatosBuenos.Length > 0)
                {
                    Image componenteImagen = nuevoPato.GetComponent<Image>();
                    if (componenteImagen != null)
                    {
                        int indiceAleatorio = Random.Range(0, fotosPatosBuenos.Length);
                        componenteImagen.sprite = fotosPatosBuenos[indiceAleatorio];
                    }
                }

                RectTransform rectPato = nuevoPato.GetComponent<RectTransform>();
                if (rectPato != null)
                {
                    rectPato.anchoredPosition = new Vector2(puntoDeSpawnX, 0f);
                }

                Button botonPato = nuevoPato.GetComponent<Button>();
                if (botonPato != null)
                {
                    GameObject referenciaPato = nuevoPato;
                    bool esMalo = (prefabElegido == prefabPatoMalvado);
                    botonPato.onClick.AddListener(() => AlDispararPato(referenciaPato, esMalo));
                }
            }
        }
    }

    private void MoverYBalancearPatos()
    {
        float limiteIzquierdo = -(contenedorPatos.rect.width / 2f) - 150f;

        for (int i = patosActivos.Count - 1; i >= 0; i--)
        {
            GameObject pato = patosActivos[i];
            if (pato == null) continue;

            RectTransform rectPato = pato.GetComponent<RectTransform>();
            if (rectPato != null)
            {
                rectPato.anchoredPosition += Vector2.left * velocidadAvance * Time.deltaTime;
                
                float tiempoDeVida = Time.time - tiemposDeNacimiento[pato];
                float ondaSeno = Mathf.Sin(tiempoDeVida * velocidadBalanceo);
                
                Vector2 posActual = rectPato.anchoredPosition;
                rectPato.anchoredPosition = new Vector2(posActual.x, ondaSeno * amplitudBalanceo);

                float anguloZ = ondaSeno * rotacionMaxima;
                rectPato.localRotation = Quaternion.Euler(0f, 0f, anguloZ);

                if (rectPato.anchoredPosition.x < limiteIzquierdo)
                {
                    tiemposDeNacimiento.Remove(pato);
                    patosActivos.RemoveAt(i);
                    Destroy(pato);
                }
            }
        }
    }

    public void AlDispararPato(GameObject patoDisparado, bool esMalo)
    {
        if (juegoTerminado) return;

        if (patosActivos.Contains(patoDisparado))
        {
            if(tiemposDeNacimiento.ContainsKey(patoDisparado)) tiemposDeNacimiento.Remove(patoDisparado);
            patosActivos.Remove(patoDisparado);
        }

        if (esMalo)
        {
            if (fuenteEfectos != null && sonidoPatoMalvadoMuere != null)
            {
                fuenteEfectos.PlayOneShot(sonidoPatoMalvadoMuere);
            }

            Destroy(patoDisparado);
            patosMalosEliminados++;
            ActualizarInterfaz();

            if (patosMalosEliminados >= patosMalosParaGanar)
            {
                TerminarJuego(true);
            }
        }
        else
        {
            if (fuenteEfectos != null && sonidoPatoBuenoMuere != null)
            {
                fuenteEfectos.PlayOneShot(sonidoPatoBuenoMuere);
            }

            Destroy(patoDisparado);
            TerminarJuego(false);
        }
    }

    private void ActualizarInterfaz()
    {
        if (textoContadorMalos != null)
        {
            int restantes = patosMalosParaGanar - patosMalosEliminados;
            textoContadorMalos.text = "Patos Malos Restantes: " + Mathf.Max(0, restantes);
        }
    }

    public override void TerminarJuego(bool victoria)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (fuenteMusica != null) fuenteMusica.Stop();
        LimpiarPatosRestantes();

        if (victoria)
        {
            if (textoInstrucciones != null)
            {
                textoInstrucciones.text = "¡Excelente puntería! Galería despejada.";
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
                textoInstrucciones.text = "¡Oh no! Perdiste la galería.";
                textoInstrucciones.color = Color.red;
            }

            if (fuenteEfectos != null && sonidoDerrota != null)
            {
                fuenteEfectos.PlayOneShot(sonidoDerrota);
            }
        }

        StartCoroutine(EsperarYRegresar(victoria));
    }

    private void LimpiarPatosRestantes()
    {
        foreach (GameObject pato in patosActivos)
        {
            if (pato != null) Destroy(pato);
        }
        patosActivos.Clear();
        tiemposDeNacimiento.Clear();
    }
}