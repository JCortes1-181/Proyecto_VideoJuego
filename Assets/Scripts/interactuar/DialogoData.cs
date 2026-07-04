using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NuevaConversacion", menuName = "Dialogos/Conversacion")]
public class DialogoData : ScriptableObject 
{
    public List<Frase> frases; 

    [Header("Configuración de Salida")]
    public bool cambiarEscenaAlFinal; 
    public string nombreEscenaDestino;
}

[System.Serializable]
public struct Frase 
{
    public string nombre; 
    [TextArea(3, 10)] public string texto; 
    public Sprite retrato; 
}
