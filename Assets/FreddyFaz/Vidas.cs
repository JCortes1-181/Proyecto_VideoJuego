using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorVidas : MonoBehaviour
{
    public static int vidasGlobales = 4; 
    public GameObject[] corazonesUI;   
    public GameObject objetoJumpscare; 
    public AudioSource sonidoGrito;   

    void Start() {
        ActualizarVisualVidas();
    }

    public void ActualizarVisualVidas() {
        // Desactiva los corazones según las vidas restantes
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
        
        // Reiniciamos todo para volver a empezar
        vidasGlobales = 4;
        SceneManager.LoadScene("FreddyFazbear"); 
    }
}