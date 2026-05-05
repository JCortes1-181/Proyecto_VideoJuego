using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorVidas : MonoBehaviour
{
    public static int vidasGlobales = 4; 
    public GameObject[] corazonesUI;   
    public GameObject objetoJumpscare; 
    public AudioSource sonidoGrito;   
    public AudioSource musicaDeFondo;

   void Start() 
    {
        // Esto refresca los corazones apenas entras a la oficina
        ActualizarVisualVidas();
    }

    public void ActualizarVisualVidas() {
        // ... (tu lógica de corazones) ...

        if (vidasGlobales <= 0) {
    // Buscamos al gestor de música y lo apagamos
    if (MusicaControl.instancia != null) {
    MusicaControl.instancia.DetenerMusica();
}
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