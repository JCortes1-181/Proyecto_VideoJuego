using UnityEngine;

public static class GestorGuardado
{

    public static int slotActual = 1;

  
    public static void GuardarPartida()
    {

        PlayerPrefs.SetInt("Vidas_Slot_" + slotActual, ControladorVidas.vidasGlobales);
        

        PlayerPrefs.SetInt("Progreso_Slot_" + slotActual, JuegoGeneral.minijuegosCompletados);
        
      
        PlayerPrefs.Save();
        Debug.Log("¡Juego guardado con éxito en el Slot " + slotActual + "!");
    }

    
    public static void CargarPartida()
    {
       
        ControladorVidas.vidasGlobales = PlayerPrefs.GetInt("Vidas_Slot_" + slotActual, 4);
        JuegoGeneral.minijuegosCompletados = PlayerPrefs.GetInt("Progreso_Slot_" + slotActual, 0);
        
        Debug.Log("¡Partida cargada desde el Slot " + slotActual + "!");
    }

   
    public static bool ExistePartidaEnSlot(int slot)
    {
   
        return PlayerPrefs.HasKey("Vidas_Slot_" + slot);
    }


    public static void BorrarSlot(int slot)
    {
        PlayerPrefs.DeleteKey("Vidas_Slot_" + slot);
        PlayerPrefs.DeleteKey("Progreso_Slot_" + slot);
        Debug.Log("Datos del Slot " + slot + " eliminados.");
    }
}
