
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public abstract class Persona
{
    
    public string Rut{get;set;}
    public string Nombre{get;set;}
    public Persona(string r, string n){Rut = r; Nombre = n;}
    public abstract decimal CalcularSueldo();

}

public class ProfesorHora : Persona {
    public int Horas { get; set; }
    public decimal Valor { get; set; }
    public ProfesorHora(string r, string n, int h, decimal v) : base(r, n) { Horas = h; Valor = v; }
    public override decimal CalcularSueldo() => Horas * Valor;


}
    

public class ProfesorPlanta : Persona
{
    public int Años{get;set;}
    public decimal SueldoFijo{get;set;}
    public ProfesorPlanta(string r, string n, int a, decimal s) : base(r, n) { Años = a; SueldoFijo = s; }

    public override decimal CalcularSueldo() =>(SueldoFijo * Años/100) + SueldoFijo;    
}

class Program {
    static List<Persona> lista = new List<Persona>(); 
    static string path = "profesores.txt";

    static void Main() {
        Cargar(); 
        while (true) {
            try { 
                Console.WriteLine("\n1-profesor Hora | 2- profesor planta| 3- buscar | 4- salir");
                Console.WriteLine("\nIngrese un numero para seleccionar opcion");
                int.TryParse(Console.ReadLine(), out int op);

                if (op == 4) { Guardar(); break; }
                if (op == 3) {
                    Console.Write("Rut: "); string r = Console.ReadLine();
                    var p = lista.FirstOrDefault(x => x.Rut == r); 
                    Console.WriteLine(p != null ? $"Sueldo: {p.CalcularSueldo()}" : "No existe");
                    continue;
                }

                Console.Write("Rut: "); string rut = Console.ReadLine();
                if (lista.Any(x => x.Rut == rut)) { Console.WriteLine("Repetido"); continue; }
                Console.Write("Nombre: "); string nom = Console.ReadLine();
                  if (op == 1) {
                    Console.Write("Horas: "); int h = int.Parse(Console.ReadLine());
                    Console.Write("Valor ($10k-$100k): "); decimal v = decimal.Parse(Console.ReadLine());
                    if (v < 10000 || v > 100000) throw new Exception("Rango inválido"); 
                    lista.Add(new ProfesorHora(rut, nom, h, v));
                } 
                else if (op == 2) {
                    Console.Write("Años: "); int a = int.Parse(Console.ReadLine());
                    Console.Write("Sueldo ($550k-$1.5M): "); decimal s = decimal.Parse(Console.ReadLine());
                    if (s < 550000 || s > 1500000) throw new Exception("Rango inválido");
                    lista.Add(new ProfesorPlanta(rut, nom, a, s));
                }
            } catch (Exception e) { Console.WriteLine("Error: " + e.Message); }
        }
    }
static void Guardar() {
        using (StreamWriter sw = new StreamWriter(path)) {
            foreach (var p in lista) {
                if (p is ProfesorHora h) sw.WriteLine($"H|{h.Rut}|{h.Nombre}|{h.Horas}|{h.Valor}");
                else if (p is ProfesorPlanta pl) sw.WriteLine($"P|{pl.Rut}|{pl.Nombre}|{pl.Años}|{pl.SueldoFijo}");
            }
        }
    }



    static void Cargar() {
        if (!File.Exists(path)) return; 
        foreach (var l in File.ReadAllLines(path)) {
            string[] d = l.Split('|');
            if (d[0] == "H") lista.Add(new ProfesorHora(d[1], d[2], int.Parse(d[3]), decimal.Parse(d[4])));
            else lista.Add(new ProfesorPlanta(d[1], d[2], int.Parse(d[3]), decimal.Parse(d[4])));
        }
    }
}

              