using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Programa principal para el seguimiento de la campaña de vacunación COVID-19
/// </summary>
class Program
{
    /// <summary>
    /// Clase que representa a un ciudadano en el sistema de vacunación
    /// </summary>
    class Ciudadano
    {
        // Propiedades básicas
        public int Id { get; set; }
        public string Nombre { get; set; }
        
        // Estado de vacunación
        public bool TienePrimeraDosis { get; set; }
        public bool TieneSegundaDosis { get; set; }
        public string TipoVacuna { get; set; }

        /// <summary>
        /// Constructor para crear un nuevo ciudadano
        /// </summary>
        /// <param name="id">Identificador único del ciudadano</param>
        /// <param name="nombre">Nombre del ciudadano</param>
        public Ciudadano(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
            TienePrimeraDosis = false;
            TieneSegundaDosis = false;
            TipoVacuna = "";
        }

        /// <summary>
        /// Devuelve una representación en cadena del ciudadano
        /// </summary>
        public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Estado: {ObtenerEstadoVacunacion()}";
        }

        /// <summary>
        /// Obtiene el estado de vacunación en formato legible
        /// </summary>
        public string ObtenerEstadoVacunacion()
        {
            if (!TienePrimeraDosis) 
                return "No vacunado";
                
            if (!TieneSegundaDosis)
                return $"1 dosis de {TipoVacuna}";
                
            return $"2 dosis de {TipoVacuna}";
        }
    }

    /// <summary>
    /// Punto de entrada principal del programa
    /// </summary>
    static void Main(string[] args)
    {
        // Configuración inicial
        ConfigurarConsola();
        
        // 1. Generar población inicial
        var ciudadanos = GenerarPoblacion(500);
        
        // 2. Simular proceso de vacunación
        SimularVacunacion(ciudadanos, 75, 75);
        
        // 3. Generar y mostrar reportes
        GenerarReportes(ciudadanos);
    }

    /// <summary>
    /// Configura la consola para mostrar caracteres especiales
    /// </summary>
    static void ConfigurarConsola()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Sistema de Seguimiento de Vacunación COVID-19";
    }

    /// <summary>
    /// Genera una lista de ciudadanos con identificadores secuenciales
    /// </summary>
    /// <param name="totalPoblacion">Número total de ciudadanos a generar</param>
    /// <returns>Lista de objetos Ciudadano</returns>
    static List<Ciudadano> GenerarPoblacion(int totalPoblacion)
    {
        var poblacion = new List<Ciudadano>();
        
        for (int i = 1; i <= totalPoblacion; i++)
        {
            poblacion.Add(new Ciudadano(i, $"Ciudadano {i}"));
        }
        
        return poblacion;
    }

    /// <summary>
    /// Simula el proceso de vacunación de la población
    /// </summary>
    /// <param name="poblacion">Lista de ciudadanos a vacunar</param>
    /// <param name="vacunadosConPfizer">Cantidad a vacunar con Pfizer</param>
    /// <param name="vacunadosConAstraZeneca">Cantidad a vacunar con AstraZeneca</param>
    static void SimularVacunacion(List<Ciudadano> poblacion, int vacunadosConPfizer, int vacunadosConAstraZeneca)
    {
        var random = new Random();
        
        // Crear una lista de índices aleatorios sin repetición
        var indicesAleatorios = Enumerable.Range(0, poblacion.Count)
                                         .OrderBy(x => random.Next())
                                         .ToList();
        
        // Vacunar con Pfizer
        AplicarVacuna(poblacion, 
                     indicesAleatorios.Take(vacunadosConPfizer).ToList(), 
                     "Pfizer");
        
        // Vacunar con AstraZeneca
        AplicarVacuna(poblacion, 
                     indicesAleatorios.Skip(vacunadosConPfizer)
                                    .Take(vacunadosConAstraZeneca)
                                    .ToList(), 
                     "AstraZeneca");
    }
    
    /// <summary>
    /// Aplica la vacuna a un grupo específico de ciudadanos
    /// </summary>
    /// <param name="poblacion">Lista completa de ciudadanos</param>
    /// <param name="indices">Índices de los ciudadanos a vacunar</param>
    /// <param name="tipoVacuna">Tipo de vacuna a aplicar</param>
    static void AplicarVacuna(List<Ciudadano> poblacion, List<int> indices, string tipoVacuna)
    {
        var random = new Random();
        
        foreach (var indice in indices)
        {
            var ciudadano = poblacion[indice];
            ciudadano.TienePrimeraDosis = true;
            ciudadano.TipoVacuna = tipoVacuna;
            
            // 50% de probabilidad de recibir segunda dosis
            ciudadano.TieneSegundaDosis = random.Next(2) == 0;
        }
    }

    /// <summary>
    /// Genera y muestra los reportes de vacunación
    /// </summary>
    /// <param name="poblacion">Lista de ciudadanos a analizar</param>
    static void GenerarReportes(List<Ciudadano> poblacion)
    {
        // 1. Obtener estadísticas
        var estadisticas = CalcularEstadisticas(poblacion);
        
        // 2. Mostrar resumen
        MostrarResumen(estadisticas);
        
        // 3. Mostrar ejemplos detallados
        MostrarEjemplosDetallados(estadisticas);
    }
    
    /// <summary>
    /// Calcula las estadísticas de vacunación usando operaciones de conjuntos
    /// </summary>
    /// <param name="poblacion">Lista de ciudadanos a analizar</param>
    /// <returns>Diccionario con las estadísticas por categoría</returns>
    static Dictionary<string, List<Ciudadano>> CalcularEstadisticas(List<Ciudadano> poblacion)
    {
        // Conjunto universal: todos los ciudadanos
        var todos = new HashSet<Ciudadano>(poblacion);
        
        // Conjunto de ciudadanos con al menos una dosis
        var conPrimeraDosis = new HashSet<Ciudadano>(
            poblacion.Where(c => c.TienePrimeraDosis));
            
        // Conjunto de ciudadanos con segunda dosis
        var conSegundaDosis = new HashSet<Ciudadano>(
            poblacion.Where(c => c.TieneSegundaDosis));
            
        // Conjunto de ciudadanos vacunados con Pfizer
        var vacunadosPfizer = new HashSet<Ciudadano>(
            poblacion.Where(c => c.TipoVacuna == "Pfizer"));
            
        // Conjunto de ciudadanos vacunados con AstraZeneca
        var vacunadosAstraZeneca = new HashSet<Ciudadano>(
            poblacion.Where(c => c.TipoVacuna == "AstraZeneca"));
        
        // Operaciones de conjuntos
        var noVacunados = new HashSet<Ciudadano>(todos);
        noVacunados.ExceptWith(conPrimeraDosis);  // Diferencia: Todos - ConPrimeraDosis
        
        var ambasDosis = new HashSet<Ciudadano>(conPrimeraDosis);
        ambasDosis.IntersectWith(conSegundaDosis);  // Intersección: PrimeraDosis ∩ SegundaDosis
        
        var soloPrimeraDosis = new HashSet<Ciudadano>(conPrimeraDosis);
        soloPrimeraDosis.ExceptWith(conSegundaDosis);  // Diferencia: PrimeraDosis - SegundaDosis
        
        var soloPfizer = new HashSet<Ciudadano>(soloPrimeraDosis);
        soloPfizer.IntersectWith(vacunadosPfizer);  // Intersección: SoloPrimeraDosis ∩ VacunadosPfizer
        
        var soloAstraZeneca = new HashSet<Ciudadano>(soloPrimeraDosis);
        soloAstraZeneca.IntersectWith(vacunadosAstraZeneca);  // Intersección: SoloPrimeraDosis ∩ VacunadosAstraZeneca
        
        return new Dictionary<string, List<Ciudadano>>
        {
            ["No vacunados"] = noVacunados.ToList(),
            ["Ambas dosis"] = ambasDosis.ToList(),
            ["Solo 1 dosis de Pfizer"] = soloPfizer.ToList(),
            ["Solo 1 dosis de AstraZeneca"] = soloAstraZeneca.ToList()
        };
    }
    
    /// <summary>
    /// Muestra un resumen de las estadísticas de vacunación
    /// </summary>
    /// <param name="estadisticas">Diccionario con las estadísticas por categoría</param>
    static void MostrarResumen(Dictionary<string, List<Ciudadano>> estadisticas)
    {
        Console.WriteLine("=== REPORTE DE VACUNACIÓN COVID-19 ===\n");
        Console.WriteLine($"Total de ciudadanos: {estadisticas.Values.Sum(grupo => grupo.Count)}");
        
        foreach (var grupo in estadisticas)
        {
            Console.WriteLine($"- {grupo.Key}: {grupo.Value.Count} ciudadanos");
        }
    }
    
    /// <summary>
    /// Muestra ejemplos detallados de cada categoría
    /// </summary>
    /// <param name="estadisticas">Diccionario con las estadísticas por categoría</param>
    static void MostrarEjemplosDetallados(Dictionary<string, List<Ciudadano>> estadisticas)
    {
        const int EJEMPLOS_A_MOSTRAR = 5;
        
        Console.WriteLine("\n=== DETALLES (mostrando primeros 5 de cada categoría) ===");
        
        foreach (var grupo in estadisticas)
        {
            MostrarEjemplos($"\n{grupo.Key}:", 
                          grupo.Value.Take(EJEMPLOS_A_MOSTRAR).ToList());
        }
    }
    
    /// <summary>
    /// Muestra los ejemplos de ciudadanos de una categoría específica
    /// </summary>
    /// <param name="titulo">Título de la categoría</param>
    /// <param name="ciudadanos">Lista de ciudadanos a mostrar</param>
    static void MostrarEjemplos(string titulo, List<Ciudadano> ciudadanos)
    {
        Console.WriteLine(titulo);
        
        if (ciudadanos.Any())
        {
            foreach (var ciudadano in ciudadanos)
            Console.WriteLine($"  {ciudadano}");
        }
        else
        {
            Console.WriteLine("  No hay ciudadanos en esta categoría.");
        }
    }
}
