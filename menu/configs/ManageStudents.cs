using Config;
using FileUtils;
using MenuUtils;
using StudentUtils;
using SoundUtils;

public class ManageStudents
{
    public static void Execute()
    {
        string menuTitle = "Lista de Estudiantes";
        string[] students = ArrayUtils.Methods.Map(
            StudentManager.studentsList,
            student => $"{StudentManager.parseData(student)[0]}"
        );

        int rows = 10;
        int totalPages = students.Length / rows;
        if (students.Length % rows != 0)
            totalPages++;
        int currentPage = 0;

        string[][] pages = [];

        for (int i = 1; i <= totalPages; i++)
            ArrayUtils.Methods.Push(ref pages, Menu.GetPagination(students, i, 10));

        bool loop = true;
        while (loop)
        {
            int choiceSelected = Menu.InteractiveMenu(
                menuTitle,
                pages[currentPage],
                pages,
                currentPage,
                rows,
                true
            );

            if (choiceSelected == -2 || choiceSelected == -3)
                Menu.HandlePagination(ref choiceSelected, pages, ref currentPage);

            if (choiceSelected == -1)
                loop = false;
            else if (choiceSelected == -999)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Ingresa el nombre del estudiante que deseas agregar: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(Escribe -1 para cancelar)\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Nombre > ");
                Console.ResetColor();

                string studentName = Console.ReadLine()!.Trim();
                if (studentName == "-1")
                    Execute();

                string[] newStudentsList = StudentManager.studentsList;
                ArrayUtils.Methods.Push(
                    ref newStudentsList,
                    StringUtils.Methods.Join(StudentManager.parseData(studentName), "|")
                );
                StudentManager.studentsList = newStudentsList;
                Execute();
            }
            else if (choiceSelected == -998)
            {
                string[] studentsArray = StudentManager.studentsList;
                string[] studentsNamesList = ArrayUtils.Methods.Map(
                    studentsArray,
                    str => StudentManager.parseData(str)[0].ToLower()
                );

                string[] TXTcontentFiltered = ArrayUtils.Methods.Filter(
                    TXT.Import(FileSettings.PlainTextPath),
                    str => !ArrayUtils.Methods.Includes(studentsNamesList, str.ToLower())
                );

                bool loop2 = true;
                while (loop2)
                {
                    if (TXTcontentFiltered.Length >= 1)
                    {
                        int ImportChoice = Menu.InteractiveMenu(
                            $"Estas seguro que deseas importar {TXTcontentFiltered.Length} {(TXTcontentFiltered.Length == 1 ? "estudiante nuevo" : "estudiantes nuevos")}?",
                            ["Si, estoy totalmente seguro", "No, no quiero importar nada"]
                        );

                        if (ImportChoice == -1 || ImportChoice == 1)
                            break;

                        if (ImportChoice == 0)
                        {
                            string[] studentsParsed = ArrayUtils.Methods.Map(
                                TXTcontentFiltered,
                                student =>
                                    StringUtils.Methods.Join(StudentManager.parseData(student), "|")
                            );

                            StudentManager.studentsList = ArrayUtils.Methods.Concat(
                                studentsArray,
                                studentsParsed
                            );
                            StudentManager.SaveChanges();

                            Console.Clear();
                            Console.ForegroundColor = TextColor.Success;
                            Console.WriteLine(
                                $"Has importado exitosamente un total de {TXTcontentFiltered.Length} {(TXTcontentFiltered.Length == 1 ? "estudiante nuevo" : "estudiantes nuevos")} a la lista!"
                            );
                            Console.ForegroundColor = TextColor.Warning;
                            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                            Console.ReadKey(true);
                            loop2 = false;
                            loop = false;
                            Execute();
                        }
                    }
                    else
                    {
                        Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.3f);

                        Console.Clear();
                        Console.ForegroundColor = TextColor.Error;
                        Console.WriteLine(
                            $"[ERROR]: No hay ningun estudiante nuevo para importar!\nDebes agregar uno nuevo al TXT {FileSettings.PlainTextPath} antes de importar nuevamente."
                        );
                        Console.ForegroundColor = TextColor.Warning;
                        Console.WriteLine("\nPresiona cualquier tecla para continuar");
                        Console.ReadKey(true);
                        break;
                    }
                }
            }
            else if (choiceSelected >= 0)
            {
                StudentsOptions.Execute(students[choiceSelected]);
                loop = false;
                Execute();
            }
        }
    }
}
