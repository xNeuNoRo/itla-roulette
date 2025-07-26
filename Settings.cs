// Configuraciones default
namespace Config
{
    // Configuracion general de los archivos que maneja el programa
    public class FileSettings
    {
        // Carpeta principal donde se almacenara todo en %appdata%/local
        // private const string MainDir = "FP_2025_C2";

        // Subcarpeta del mainDir donde se guardara todo lo relacionado a este programa
        // private const string SubDir = "Roulette_Project";

        // Ruta principal
        // private static readonly string MainPath = Path.Combine( // Path.Combine combina rutas
        //     Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), // Ruta de %appdata%/Local
        //     MainDir, // Carpeta principal
        //     SubDir // Carpeta del proyecto
        // );

        // Ruta principal
        private static readonly string MainPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "data"
        );

        // En la misma ruta de ejecucion, el archivo 'estudiantes.txt'
        public static readonly string PlainTextPath = "estudiantes.txt";

        // Ruta donde se guardara el historial de selecciones mas a detalle
        public static readonly string SelectionHistoryPath = Path.Combine(
            MainPath, // Ruta principal
            "selection_history.txt" // Archivo .txt donde se guardara el historial de selecciones
        );

        // Ruta donde se guardara la lista de roles
        public static readonly string RolesListPath = Path.Combine(MainPath, "roles_list.txt");

        // Ruta donde se guardaran los logs para depurar errores del programa
        public static readonly string ProgramLogPath = "logs.txt";

        // Modificar dinamicamente el .txt inicial de la lista de estudiantes,
        // Osea si incluye una lista ir borrando los nombres a medida q se procesen dentro del programa
        // public static readonly bool DynamicModify = false;

        // Automaticamente importar los estudiantes de la lista evitando duplicados pero sin consultar nada
        // (OJO, SI BORRAS UN ESTUDIANTE Y ESTE SIGUE EN EL estudiantes.txt POSIBLEMENTE SE IMPORTE NUEVAMENTE)
        public static readonly bool DynamicImportStudents = false;

        // Constructor de la clase
        static FileSettings()
        {
            // Verificamos si existe la carpeta, y si no, la creamos
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);
        }
    }

    // Configuracion general de las reglas para la seleccion
    public class RuleSettings
    {
        // Lista de roles default
        public static readonly string[] DefaultRoles =
        [
            "Desarrollador en vivo",
            "Facilitador de ejercicio",
        ];

        // Cantidad minima de roles
        public static readonly int MinRolesRequired = 2;

        // Si se permite que salgan mas de 1 persona por rol
        public static readonly bool AllowRepeatRoles = false;

        // Cantidad de personas por rol
        // Solo tiene efecto si 'AllowRepeatRoles' es true
        public static readonly int MaxStudentsPerRole = 1;

        // Si se permite que el mismo estudiante para participar nuevamente
        public static readonly bool AllowRepeatStudents = false;

        // Cantidad de participaciones si desea repetir el mismo estudiante para participar
        // Solo tiene efecto si 'AllowRepeatStudents' es true
        public static readonly int MaxParticipationAmount = 1;
    }

    public class TextColor
    {
        public static readonly ConsoleColor Selector = ConsoleColor.DarkMagenta;

        public static readonly ConsoleColor Attendance = ConsoleColor.Cyan;

        public static readonly ConsoleColor Questions = ConsoleColor.Cyan;

        public static readonly ConsoleColor Answer = ConsoleColor.Yellow;

        public static readonly ConsoleColor Error = ConsoleColor.Red;

        public static readonly ConsoleColor Warning = ConsoleColor.Yellow;

        public static readonly ConsoleColor Success = ConsoleColor.Green;
    }

    // Configuracion general de efectos para la seleccion
    public class EffectSettings
    {
        // Si se permite la animacion de la ruleta
        public static readonly bool RouletteAnimation = true;

        // Para manejar la maxima velocidad de la ruleta
        public static readonly int RouletteAnimMaxSpeed = 800;

        // Para manejar los fotogramas/pasos de la animacion
        public static readonly int RouletteAnimSteps = 120;

        // Para desactivar/activar el sonido de la ruleta
        public static readonly bool RouletteAnimSound = true;
    }

    public class SoundSettings
    {
        public static readonly string IntroAudioPath = Path.GetFullPath(
            @"../../../audio/intro.mp3",
            AppContext.BaseDirectory
        );

        public static readonly string RouletteAudioPath = Path.GetFullPath(
            @"../../../audio/roulette.mp3",
            AppContext.BaseDirectory
        );

        public static readonly string SelectedAudioPath = Path.GetFullPath(
            @"../../../audio/selected.mp3",
            AppContext.BaseDirectory
        );

        public static readonly string ChoicesAudioPath = Path.GetFullPath(
            @"../../../audio/choices.mp3",
            AppContext.BaseDirectory
        );

        public static readonly string EnterAudioPath = Path.GetFullPath(
            @"../../../audio/enter.mp3",
            AppContext.BaseDirectory
        );

        public static readonly string BackAudioPath = Path.GetFullPath(
            @"../../../audio/back.mp3",
            AppContext.BaseDirectory
        );

        public static readonly string InvalidAudioPath = Path.GetFullPath(
            @"../../../audio/invalid.mp3",
            AppContext.BaseDirectory
        );

        public static readonly string Timer60AudioPath = Path.GetFullPath(
            @"../../../audio/timer60.mp3",
            AppContext.BaseDirectory
        );

        public static readonly float DefaultVolume = 0.5f;
    }

    public class AppSettings
    {
        // Si se omite la intro del programa
        public static readonly bool SkipIntro = false;

        // Si se oculta el cursor de la consola
        public static readonly bool HideCursor = false;
    }
}
