using UnityEngine;
using UnityEngine.Audio; 
using UnityEngine.SceneManagement; 
public class MenuControlador : MonoBehaviour
{
    [Header("Paneles de Navegación")]
    public GameObject menuPrincipal;
    public GameObject panelOpciones;

    [Header("Configuraciones Técnicas")]
    public AudioMixer masterMixer;

  

    public void IniciarJuego()
    {

        SceneManager.LoadScene("SampleScene"); 
    }

    public void AbrirOpciones()
    {
        menuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
        menuPrincipal.SetActive(true);
    }

  

    public void CambiarVolumen(float valor)
    {
        masterMixer.SetFloat("MasterVol", Mathf.Log10(valor) * 20);
    }

    public void CambiarPantalla(bool completa)
    {
        Screen.fullScreen = completa;
    }
}