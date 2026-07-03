using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorVidas : MonoBehaviour
{
    public static int vidasGlobales = 4; 
    public GameObject[] corazonesUI;   
    
    [Header("Jumpscare (Opcional)")]
    public GameObject objetoJumpscare; // Déjalo vacío si no quieres jumpscare
    public AudioSource sonidoGrito;   

    [Header("UI de Menú")]
    public GameObject panelGameOver; 

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
            StartCoroutine(SecuenciaDerrota());
        }
    }

    IEnumerator SecuenciaDerrota() {
        // --- CAMBIO: Solo hace el Jumpscare si le asignaste un monstruo ---
        if (objetoJumpscare != null) {
            objetoJumpscare.SetActive(true); 
            if (sonidoGrito != null) {
                sonidoGrito.gameObject.SetActive(true);
                sonidoGrito.Play();
            }
            yield return new WaitForSeconds(3f);
            objetoJumpscare.SetActive(false);
        }
        
        if (panelGameOver != null) panelGameOver.SetActive(true);
        
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f; 
    }
}