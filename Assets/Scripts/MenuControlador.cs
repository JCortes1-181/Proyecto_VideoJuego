using UnityEngine;
using UnityEngine.Audio; // Fundamental para el volumen
using UnityEngine.SceneManagement; // Para cargar el juego

public class MenuControlador : MonoBehaviour
{
    [Header("Paneles de Navegación")]
    public GameObject menuPrincipal;
    public GameObject panelOpciones;

    [Header("Configuraciones Técnicas")]
    public AudioMixer masterMixer;

    // --- NAVEGACIÓN ---

    public void IniciarJuego()
    {
        // Cambia "NombreDeTuEscenaJuego" por el nombre real de tu nivel principal
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

    // --- CONFIGURACIONES ---

    public void CambiarVolumen(float valor)
    {
        // MasterVol debe ser el nombre del parámetro expuesto en tu Audio Mixer
        masterMixer.SetFloat("MasterVol", Mathf.Log10(valor) * 20);
    }

    public void CambiarPantalla(bool completa)
    {
        Screen.fullScreen = completa;
    }
}