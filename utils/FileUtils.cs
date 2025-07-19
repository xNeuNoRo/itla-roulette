using System.Reflection.Metadata;
using Config;
using StudentUtils;

namespace FileUtils
{
    public class TXT
    {
        public static string[] Import(string Path)
        {
            string contentRaw = File.ReadAllText(Path); // Leemos el txt
            string[] contentSplitted = StringUtils.Methods.Split(contentRaw, '\n'); // partimos el contenido del .txt en base al salto de linea \n
            // Filtramos los posibles espacios en blancos o strings vacios en 'contentSplitted' y despues lo volvemos a pasar a Array
            string[] contentFiltered = ArrayUtils.Methods.Map(
                ArrayUtils.Methods.Filter(
                    contentSplitted,
                    str => !StringUtils.Methods.IsNullOrWhiteSpace(str)
                ),
                str => str.Trim()
            );

            // Devolvemos el contenido filtrado de los estudiantes
            return contentFiltered;
        }

        // Usamos ref para que modifique el array directo q se le pasa
        public static void Reload()
        {
            string[] studentsArrayToCompare = StudentManager.studentsList;
            string[] studentsNamesList = ArrayUtils.Methods.Map(
                studentsArrayToCompare,
                str => StudentManager.parseData(str)[0].ToLower()
            );

            string[] contentFiltered = ArrayUtils.Methods.Filter(
                Import(FileSettings.PlainTextPath),
                str => !ArrayUtils.Methods.Includes(studentsNamesList, str.ToLower())
            );

            // Si hay estudiantes nuevos en el listado
            if (contentFiltered.Length >= 1)
            {
                string[] studentsParsed = ArrayUtils.Methods.Map(
                    contentFiltered,
                    student => StringUtils.Methods.Join(StudentManager.parseData(student), "|")
                );

                StudentManager.studentsList = ArrayUtils.Methods.Concat(
                    StudentManager.studentsList,
                    studentsParsed
                );
                StudentManager.SaveChanges();
            }
            // Concatenamos ambos arrays y lo asignamos al array previo
        }

        // Agregar una nueva linea al TXT, por defecto al final de todas las que existan (si es que tiene alguna)
        public static int AddLine(string PathToTXT, string value)
        {
            // 0: Modificacion exitosa
            // 1: Error

            // File.AppendAllText(...) agrega texto al final del archivo
            // Le pasamos la ruta, el nombre que se agregara + una nueva linea con Enviroment.NewLine, generalmente \n
            try
            {
                File.AppendAllText(PathToTXT, value + Environment.NewLine);
                return 0;
            }
            catch (Exception err)
            {
                // Si ocurre un error lo arrojamos en consola
                Console.WriteLine($"Ha ocurrido un error inesperado:\n{err}");
                // Tambien lo guardamos en el archivo de logs
                File.AppendAllText(FileSettings.ProgramLogPath, err.Message + Environment.NewLine);
                return 1;
            }
        }

        public static int RemoveLine(string PathToTXT, string value)
        {
            // 0: Modificacion exitosa
            // 1: Error

            try
            {
                // Leer el archivo
                string[] file = File.ReadAllLines(PathToTXT);
                // Filtrar donde la linea que coincida con el studentName proporcionado como parametro
                string[] filteredFile = ArrayUtils.Methods.Filter(
                    file,
                    str =>
                        !StringUtils.Methods.IsNullOrWhiteSpace(str)
                        && str.Trim().ToLower() != value.Trim().ToLower()
                );
                // Volver a reescribir el archivo filtrado sin el nombre que se especifico
                File.WriteAllLines(PathToTXT, filteredFile);
                return 0;
            }
            catch (Exception err)
            {
                // Si ocurre un error lo arrojamos en consola
                Console.WriteLine($"Ha ocurrido un error inesperado:\n{err}");
                // Tambien lo guardamos en el archivo de logs
                File.AppendAllText(FileSettings.ProgramLogPath, err.Message + Environment.NewLine);
                return 1;
            }
        }
    }
}
