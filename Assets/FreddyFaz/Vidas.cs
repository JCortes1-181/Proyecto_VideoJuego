using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorVidas : MonoBehaviour
{
    public static int vidasGlobales = 4; 
    public GameObject[] corazonesUI;   
    public GameObject objetoJumpscare; 
    public AudioSource sonidoGrito;   

    public void ActualizarVisualVidas() {
        for (int i = 0; i < corazonesUI.Length; i++) {
            if(corazonesUI[i] != null) corazonesUI[i].SetActive(i < vidasGlobales);
        }

        if (vidasGlobales <= 0) {
            StartCoroutine(SecuenciaJumpscare());
        }
    }

    IEnumerator SecuenciaJumpscare() {
        
        if (objetoJumpscare != null) objetoJumpscare.SetActive(true); 

        
        if (sonidoGrito != null) sonidoGrito.gameObject.SetActive(true);

        
        yield return new WaitForEndOfFrame();

        
        if (sonidoGrito != null) sonidoGrito.Play();

        yield return new WaitForSeconds(3f);
        vidasGlobales = 4;
        SceneManager.LoadScene("SampleScene"); 
    }
}