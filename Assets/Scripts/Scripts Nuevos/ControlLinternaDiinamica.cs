using UnityEngine;
using UnityEngine.Rendering.Universal; // Necesario para la luz 2D

public class ControlLinternaDinamica : MonoBehaviour
{
    public Light2D luzLinterna;
    public float suavidad = 3f;

    // Radios según tus 4 vidas: [0 vidas, 1, 2, 3, 4 vidas]
    public float[] nivelesDeRadio = { 0f, 3f, 6f, 9f, 12f };

    void Update()
    {
        // Accedemos a tu variable estática de vidas
        int vidasActuales = Mathf.Clamp(ControladorVidas.vidasGlobales, 0, nivelesDeRadio.Length - 1);
        float radioObjetivo = nivelesDeRadio[vidasActuales];

        // Cambiamos el radio de la luz suavemente
        luzLinterna.pointLightOuterRadius = Mathf.Lerp(luzLinterna.pointLightOuterRadius, radioObjetivo, Time.deltaTime * suavidad);
    }
}
