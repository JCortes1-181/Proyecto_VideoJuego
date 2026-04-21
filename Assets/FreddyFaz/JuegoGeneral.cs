using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class JuegoGeneral : MonoBehaviour
{
    [Header("Escenas")]
    public GameObject Oficina_Normal;      
    public GameObject MiniJuego;          
    public GameObject Estatica_Efecto;
    public GameObject ContenedorCorazones; 
    [Header("UI")]
    public TextMeshProUGUI textoTiempo;   
    public GameObject Perdiste_Gif, Ganaste_Gif;

    [Header("Minijuego")]
    public List<GameObject> TodosLosPanes; 
    public Movimiento_Mario ScriptTeto;   

    [Header("Ajustes")]
    public float tiempoRonda = 10f;       
    
    private float timerActual;
    private bool juegoActivo = false;

    void Start() {
        timerActual = tiempoRonda;
        Oficina_Normal.SetActive(true);
        MiniJuego.SetActive(false);
        if(ContenedorCorazones) ContenedorCorazones.SetActive(true);
        
        Invoke("IniciarCiclo", 3f);
    }

    void IniciarCiclo() { 
        if (ControladorVidas.vidasGlobales > 0) {
            StartCoroutine(TransicionEntrada()); 
        }
    }

    IEnumerator TransicionEntrada() {
        foreach (GameObject p in TodosLosPanes) if (p != null) p.SetActive(true);
        if(ScriptTeto != null) ScriptTeto.ResetearPosicion();
        
        timerActual = tiempoRonda;

        if(Estatica_Efecto) Estatica_Efecto.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        if(Estatica_Efecto) Estatica_Efecto.SetActive(false);

       
        Oficina_Normal.SetActive(false);
        if(ContenedorCorazones) ContenedorCorazones.SetActive(false);
        
        MiniJuego.SetActive(true);
        juegoActivo = true;
    }

    void Update() {
        if (!juegoActivo) return;

        timerActual -= Time.deltaTime;
        if (textoTiempo) {
            int seg = Mathf.CeilToInt(timerActual);
            textoTiempo.text = "" + (seg < 0 ? 0 : seg).ToString();
        }

        int recogidos = 0;
        foreach (GameObject p in TodosLosPanes) if (p != null && !p.activeSelf) recogidos++;

        if (recogidos >= 5) FinalizarRonda(true);
        else if (timerActual <= 0) FinalizarRonda(false);
    }

    void FinalizarRonda(bool ganado) {
        juegoActivo = false;
        StartCoroutine(SecuenciaResultado(ganado));
    }

    IEnumerator SecuenciaResultado(bool ganado) {
        MiniJuego.SetActive(false);

        if (ganado) {
            Ganaste_Gif.SetActive(true);
        } else {
            Perdiste_Gif.SetActive(true);
            ControladorVidas.vidasGlobales--; 
        }

        yield return new WaitForSeconds(2.5f);

        if (ganado) Ganaste_Gif.SetActive(false);
        else Perdiste_Gif.SetActive(false);

        
        Oficina_Normal.SetActive(true);
        if(ContenedorCorazones) ContenedorCorazones.SetActive(true);

        ControladorVidas gestor = FindObjectOfType<ControladorVidas>();
        if (gestor != null) {
            gestor.ActualizarVisualVidas();
        }
        
        
        if (ControladorVidas.vidasGlobales > 0) {
            Invoke("IniciarCiclo", 4f); 
        }
    }
}