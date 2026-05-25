public static class EstadoMundo {
    public enum EstadoNpc { PrimeraVez, VolvioDerrotado, VolvioVictorioso, YaTerminoTodo }
    public static EstadoNpc estadoActual = EstadoNpc.PrimeraVez;
}