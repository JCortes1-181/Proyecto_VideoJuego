using UnityEngine;

public class ConfiguracionVideoGlobal : MonoBehaviour
{
    public static ConfiguracionVideoGlobal Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
    }
}