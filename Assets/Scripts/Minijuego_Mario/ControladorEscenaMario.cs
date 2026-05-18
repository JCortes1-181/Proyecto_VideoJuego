using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ControladorEscenaMario : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoRonda = 10f;
    public int panesNecesarios = 5;
    public List<GameObject> todosLosPanes; 

    [Header("UI")]
    public TextMeshProUGUI textoTiempo;
    public GameObject gifGanaste;
    public GameObject gifPerdiste;

    private float timerActual;
    private bool juegoTerminado = false;

    void Start() {
        timerActual = tiempoRonda;
    }

    void Update() {
        if (juegoTerminado) return;

        // El tiempo corre
        timerActual -= Time.deltaTime;
        if (textoTiempo) textoTiempo.text = Mathf.CeilToInt(timerActual).ToString();

        // Contamos panes desactivados
        int recogidos = 0;
        foreach (GameObject p in todosLosPanes) {
            if (p != null && !p.activeSelf) recogidos++;
        }

        // ¿Ganó o perdió?
        if (recogidos >= panesNecesarios) {
            Finalizar(true);
        } else if (timerActual <= 0) {
            Finalizar(false);
        }
    }

    void Finalizar(bool ganado) {
        juegoTerminado = true;
        StartCoroutine(VolverAOficina(ganado));
    }

    IEnumerator VolverAOficina(bool ganado) {
        if (ganado) {
            if (gifGanaste) gifGanaste.SetActive(true);
        } else {
            if (gifPerdiste) gifPerdiste.SetActive(true);
            
           
            ControladorVidas.vidasGlobales--; 
        }

        yield return new WaitForSeconds(2.5f);
        
       
        SceneManager.LoadScene("FreddyFazbear"); 
    }
}