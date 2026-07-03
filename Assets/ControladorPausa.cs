using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para el Slider

public class ControladorPausa : MonoBehaviour
{
    public static ControladorPausa Instancia { get; private set; }

    [Header("UI Componentes")]
    [SerializeField] private GameObject panelPausa;
    
    [Header("Ajustes")]
    [SerializeField] private Slider sliderVolumen; // El Slider que pondrás en tu PanelPausa

    private bool juegoPausado = false;

    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (panelPausa != null) panelPausa.SetActive(false);

        // Cargar el volumen guardado (por defecto 100%)
        float volumenGuardado = PlayerPrefs.GetFloat("VolumenJuego", 1f);
        AudioListener.volume = volumenGuardado;

        if (sliderVolumen != null)
        {
            sliderVolumen.value = volumenGuardado;
            // Conectar el slider a la función automáticamente
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Tecla ESC presionada. Escena actual: " + SceneManager.GetActiveScene().name);

            if (SceneManager.GetActiveScene().name == "MenuPrincipal") 
            {
                return; // No pausar en el menú principal
            }

            if (juegoPausado) 
            {
                Reanudar();
            }
            else 
            {
                Pausar();
            }
        }
    }

    public void AlternarPausa()
    {
        if (SceneManager.GetActiveScene().name == "MenuPrincipal") 
        {
            return; 
        }

        if (juegoPausado) 
        {
            Reanudar();
        }
        else 
        {
            Pausar();
        }
    }


    public void Pausar()
    {
        if (panelPausa != null) panelPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void Reanudar()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    // --- FUNCIÓN DEL VOLUMEN ---
    public void CambiarVolumen(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("VolumenJuego", valor);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f; 
        juegoPausado = false;
        if (panelPausa != null) panelPausa.SetActive(false); 
        SceneManager.LoadScene("MenuPrincipal"); 
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego..."); 
        Application.Quit(); 
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}