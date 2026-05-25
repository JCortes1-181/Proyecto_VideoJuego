using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NuevaConversacion", menuName = "Dialogos/Conversacion")]
public class DialogoData : ScriptableObject 
{
    public List<Frase> frases; // Lista de frases que componen la charla

    [Header("Configuración de Salida")]
    public bool cambiarEscenaAlFinal; 
    public string nombreEscenaDestino;
}

[System.Serializable]
public struct Frase 
{
    public string nombre; // Quién habla
    [TextArea(3, 10)] public string texto; // Qué dice
    public Sprite retrato; // <-- NUEVO: Aquí arrastrarás la cara/expresión del personaje
}
