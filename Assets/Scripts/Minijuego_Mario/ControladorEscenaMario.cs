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

        timerActual -= Time.deltaTime;
        if (textoTiempo) textoTiempo.text = Mathf.CeilToInt(timerActual).ToString();

        int recogidos = 0;
        foreach (GameObject p in todosLosPanes) {
            if (p != null && !p.activeSelf) recogidos++;
        }

        if (recogidos >= panesNecesarios) {
            Finalizar(true);
        } else if (timerActual <= 0) {
            Finalizar(false);
        }
    }

    public void Finalizar(bool ganado) {
        if (juegoTerminado) return; 
        
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
        
        string escenaDestino = PlayerPrefs.GetString("EscenaRetorno", "FreddyFazbear");
        SceneManager.LoadScene(escenaDestino);
    }
}