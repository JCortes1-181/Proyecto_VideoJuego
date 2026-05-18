using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorPausa : MonoBehaviour
{
    public static ControladorPausa Instancia { get; private set; }

    [Header("UI Componentes")]
    [SerializeField] private GameObject panelPausa;

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
        panelPausa.SetActive(false);
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        
        Debug.Log("Tecla ESC presionada. Escena actual: " + SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == "MenuPrincipal") 
        {
            Debug.Log("Pausa cancelada: Estás en el Menú Principal.");
            return;
        }

        if (juegoPausado) 
        {
            Debug.Log("Cambiando a: Reanudar");
            Reanudar();
        }
        else 
        {
            Debug.Log("Cambiando a: Pausar");
            Pausar();
        }
    }
}

    public void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        panelPausa.SetActive(false); 
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