using System;
using System.Collections;



namespace SistemaDeReciclaje
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Inicializar materiales reciclables predefinidos
            InicializarMateriales();
            InicializarRecompensas();

            Console.WriteLine("Sistema de Gestión de Reciclaje");
            bool continuar = true;

            while (continuar)
            {
                Console.WriteLine("\nSeleccione una opción:");
                Console.WriteLine("1. Registrar Usuario");
                Console.WriteLine("2. Registrar Reciclaje");
                Console.WriteLine("3. Ver Estadísticas de Reciclaje");
                Console.WriteLine("4. Canjear Recompensas");
                Console.WriteLine("5. Salir");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        RegistrarUsuario();
                        break;
                    case "2":
                        RegistrarReciclaje();
                        break;
                    case "3":
                        VerEstadisticas();
                        break;
                    case "4":
                        CanjearRecompensas();
                        break;
                    case "5":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        static void InicializarMateriales()
        {
            DataManager.RegistrarMaterialReciclable("Papel", 10);
            DataManager.RegistrarMaterialReciclable("Cartón", 15);
            DataManager.RegistrarMaterialReciclable("Botellas", 20);
            DataManager.RegistrarMaterialReciclable("Otros", 5);
        }

        static void InicializarRecompensas()
        {
            DataManager.RegistrarRecompensa("Descuento en tienda", 100);
            DataManager.RegistrarRecompensa("Entrada gratuita a evento", 200);
            DataManager.RegistrarRecompensa("Producto gratis", 300);
        }

        static void RegistrarUsuario()
        {
            Console.WriteLine("Registrar Usuario:");
            Console.Write("Nombre del Usuario: ");
            string nombre = Console.ReadLine();
            DataManager.RegistrarUsuario(nombre);
            Console.WriteLine("Usuario registrado exitosamente.");
        }

        static void RegistrarReciclaje()
        {
            Console.WriteLine("Registrar Reciclaje:");
            MostrarUsuarios();
            Console.Write("ID del Usuario: ");
            if (int.TryParse(Console.ReadLine(), out int usuarioId))
            {
                MostrarMateriales();
                Console.Write("ID del Material Reciclable: ");
                if (int.TryParse(Console.ReadLine(), out int materialId))
                {
                    Console.Write("Cantidad: ");
                    if (double.TryParse(Console.ReadLine(), out double cantidad))
                    {
                        DataManager.RegistrarReciclaje(usuarioId, materialId, cantidad);
                        Console.WriteLine("Reciclaje registrado exitosamente.");
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida. Debe ingresar un número para la cantidad.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Debe ingresar un número entero para el ID del material.");
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida. Debe ingresar un número entero para el ID del usuario.");
            }
        }

        static void VerEstadisticas()
        {
            Console.WriteLine("Estadísticas de Reciclaje:");
            var registros = DataManager.ObtenerEstadisticasReciclaje();
            foreach (RegistroReciclaje registro in registros)
            {
                Console.WriteLine(registro);
            }
        }

        static void CanjearRecompensas()
        {
            MostrarUsuarios();
            Console.Write("ID del Usuario: ");
            if (int.TryParse(Console.ReadLine(), out int usuarioId))
            {
                var usuario = DataManager.ObtenerUsuarioPorId(usuarioId);
                if (usuario != null)
                {
                    Console.WriteLine($"Puntos del usuario {usuario.Nombre}: {usuario.Puntos}");
                    MostrarRecompensas();
                    Console.Write("ID de la Recompensa a canjear: ");
                    if (int.TryParse(Console.ReadLine(), out int recompensaId))
                    {
                        var recompensa = DataManager.ObtenerRecompensaPorId(recompensaId);
                        if (recompensa != null)
                        {
                            if (usuario.Puntos >= recompensa.PuntosRequeridos)
                            {
                                usuario.Puntos -= recompensa.PuntosRequeridos;
                                Console.WriteLine($"Recompensa '{recompensa.Nombre}' canjeada exitosamente.");
                            }
                            else
                            {
                                Console.WriteLine("No tienes suficientes puntos para canjear esta recompensa.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID de recompensa no válido.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida. Debe ingresar un número entero para el ID de la recompensa.");
                    }
                }
                else
                {
                    Console.WriteLine("ID de usuario no válido.");
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida. Debe ingresar un número entero para el ID del usuario.");
            }
        }

        static void MostrarUsuarios()
        {
            Console.WriteLine("Usuarios registrados:");
            foreach (Usuario usuario in DataManager.Usuarios)
            {
                Console.WriteLine(usuario);
            }
        }

        static void MostrarMateriales()
        {
            Console.WriteLine("Materiales reciclables registrados:");
            foreach (MaterialReciclable material in DataManager.Materiales)
            {
                Console.WriteLine(material);
            }
        }

        static void MostrarRecompensas()
        {
            Console.WriteLine("Recompensas disponibles:");
            foreach (Recompensa recompensa in DataManager.Recompensas)
            {
                Console.WriteLine(recompensa);
            }
        }
    }
}

public class MaterialReciclable
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int PuntosPorUnidad { get; set; }

    public MaterialReciclable(int id, string nombre, int puntosPorUnidad)
    {
        Id = id;
        Nombre = nombre;
        PuntosPorUnidad = puntosPorUnidad;
    }

    public override string ToString()
    {
        return $"ID: {Id}, {Nombre} (Puntos por unidad: {PuntosPorUnidad})";
    }
}

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Puntos { get; set; }

    public Usuario(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
        Puntos = 0;
    }

    public override string ToString()
    {
        return $"ID: {Id}, {Nombre} (Puntos: {Puntos})";
    }
}

public class RegistroReciclaje
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int MaterialReciclableId { get; set; }
    public double Cantidad { get; set; }
    public DateTime Fecha { get; set; }

    public RegistroReciclaje(int id, int usuarioId, int materialReciclableId, double cantidad)
    {
        Id = id;
        UsuarioId = usuarioId;
        MaterialReciclableId = materialReciclableId;
        Cantidad = cantidad;
        Fecha = DateTime.Now;
    }

    public override string ToString()
    {
        string usuarioNombre = DataManager.ObtenerUsuarioPorId(UsuarioId)?.Nombre;
        string materialNombre = DataManager.ObtenerMaterialPorId(MaterialReciclableId)?.Nombre;

        return $"Usuario: {usuarioNombre}, Material: {materialNombre}, Cantidad: {Cantidad}, Fecha: {Fecha}";
    }
}

public class Recompensa
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int PuntosRequeridos { get; set; }

    public Recompensa(int id, string nombre, int puntosRequeridos)
    {
        Id = id;
        Nombre = nombre;
        PuntosRequeridos = puntosRequeridos;
    }

    public override string ToString()
    {
        return $"ID: {Id}, {Nombre} (Puntos requeridos: {PuntosRequeridos})";
    }
}

public static class DataManager
{
    public static ArrayList Materiales { get; } = new ArrayList();
    public static ArrayList Usuarios { get; } = new ArrayList();
    public static ArrayList Registros { get; } = new ArrayList();
    public static ArrayList Recompensas { get; } = new ArrayList();

    public static int GetNextId(ArrayList list)
    {
        if (list.Count == 0) return 1;
        var maxId = 0;
        foreach (var item in list)
        {
            var prop = item.GetType().GetProperty("Id");
            if (prop != null)
            {
                var value = (int)prop.GetValue(item);
                if (value > maxId) maxId = value;
            }
        }
        return maxId + 1;
    }

    public static void RegistrarMaterialReciclable(string nombre, int puntosPorUnidad)
    {
        var nuevoMaterial = new MaterialReciclable(GetNextId(Materiales), nombre, puntosPorUnidad);
        Materiales.Add(nuevoMaterial);
    }

    public static void RegistrarUsuario(string nombre)
    {
        var nuevoUsuario = new Usuario(GetNextId(Usuarios), nombre);
        Usuarios.Add(nuevoUsuario);
    }

    public static void RegistrarReciclaje(int usuarioId, int materialId, double cantidad)
    {
        Usuario usuario = ObtenerUsuarioPorId(usuarioId);
        MaterialReciclable material = ObtenerMaterialPorId(materialId);

        if (usuario != null && material != null)
        {
            var nuevoRegistro = new RegistroReciclaje(GetNextId(Registros), usuarioId, materialId, cantidad);
            usuario.Puntos += (int)(cantidad * material.PuntosPorUnidad);

            Registros.Add(nuevoRegistro);
        }
    }

    public static void RegistrarRecompensa(string nombre, int puntosRequeridos)
    {
        var nuevaRecompensa = new Recompensa(GetNextId(Recompensas), nombre, puntosRequeridos);
        Recompensas.Add(nuevaRecompensa);
    }

    public static ArrayList ObtenerEstadisticasReciclaje()
    {
        return Registros;
    }

    public static Usuario ObtenerUsuarioPorId(int id)
    {
        foreach (Usuario usuario in Usuarios)
        {
            if (usuario.Id == id)
                return usuario;
        }
        return null;
    }

    public static MaterialReciclable ObtenerMaterialPorId(int id)
    {
        foreach (MaterialReciclable material in Materiales)
        {
            if (material.Id == id)
                return material;
        }
        return null;
    }

    public static Recompensa ObtenerRecompensaPorId(int id)
    {
        foreach (Recompensa recompensa in Recompensas)
        {
            if (recompensa.Id == id)
                return recompensa;
        }
        return null;
    }
}