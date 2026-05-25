using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorVidas : MonoBehaviour
{
    public static int vidasGlobales = 4; 
    public GameObject[] corazonesUI;   
    public GameObject objetoJumpscare; 
    public AudioSource sonidoGrito;   

    [Header("UI de Menú")]
    public GameObject panelGameOver; // Arrastra tu Panel_GameOver aquí

    void Start() {
        ActualizarVisualVidas();
    }

    public void ActualizarVisualVidas() {
        for (int i = 0; i < corazonesUI.Length; i++) {
            if (corazonesUI[i] != null) {
                corazonesUI[i].SetActive(i < vidasGlobales);
            }
        }

        if (vidasGlobales <= 0) {
            if (MusicaControl.instancia != null) {
                MusicaControl.instancia.DetenerMusica();
            }
            StartCoroutine(SecuenciaJumpscare());
        }
    }

    IEnumerator SecuenciaJumpscare() {
        if (objetoJumpscare != null) objetoJumpscare.SetActive(true); 
        if (sonidoGrito != null) {
            sonidoGrito.gameObject.SetActive(true);
            sonidoGrito.Play();
        }

        yield return new WaitForSeconds(3f);
        
        // --- CAMBIO PARA EL MENÚ ---
        if (objetoJumpscare != null) objetoJumpscare.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(true);
        
        Cursor.visible = true; // Mostramos el mouse para clickear
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f; // Pausamos el juego
    }
}