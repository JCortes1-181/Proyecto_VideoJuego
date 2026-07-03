using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorSkate : MonoBehaviour
{
    public float tiempoParaGanar = 10f;
    public GameObject panelVictoria;
    
    void Update()
    {
        tiempoParaGanar -= Time.deltaTime;
        
        if (tiempoParaGanar <= 0)
        {
            StartCoroutine(GanarJuego());
        }
    }

    IEnumerator GanarJuego()
    {
        if (panelVictoria != null) panelVictoria.SetActive(true);
        Time.timeScale = 0f; 
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        
        // --- NUEVO: Regreso Inteligente ---
        string escenaDestino = PlayerPrefs.GetString("EscenaRetorno", "FreddyFazbear");
        SceneManager.LoadScene(escenaDestino);
    }
}