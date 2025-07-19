using Config;
using MenuUtils;
using SoundUtils;
using StudentUtils;

public class StudentsOptions
{
    public static void Execute(string studentName)
    {
        bool loop = true;

        while (loop)
        {
            int choiceSelected = Menu.InteractiveMenu(
                $"Estas editando al estudiante {studentName}",
                [
                    "Editar nombre",
                    "Editar puntuacion",
                    "Editar participacion",
                    "Borrar su historial",
                    "Borrar al estudiante",
                ]
            );

            if (choiceSelected == -1)
                loop = false;
            // Editar nombre
            else if (choiceSelected == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Ingresa el nuevo nombre del estudiante: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "(Escribe -1 para cancelar o presiona Enter sin escribir nada)\n"
                );
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Nombre > ");
                Console.ResetColor();

                string newStudentName = Console.ReadLine()!.Trim();

                if (
                    newStudentName != "-1"
                    && !StringUtils.Methods.IsNullOrWhiteSpace(newStudentName)
                )
                {
                    string student = StudentManager.getStudentByName(studentName);
                    StudentManager.editName(ref student, newStudentName);
                    loop = false;
                    Execute(newStudentName);
                }
            }
            // Editar puntuacion
            else if (choiceSelected == 1)
            {
                Console.Clear();
                string student = StudentManager.getStudentByName(studentName);
                string[] studentData = StudentManager.parseData(student);
                string studentRoles = studentData[3];

                if (studentRoles.Length <= 0)
                {
                    Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                    Sound.PlayAudio();
                    Sound.SetVolume(0.3f);

                    Console.ForegroundColor = TextColor.Error;
                    Console.WriteLine(
                        "[ERROR]: El estudiante seleccionado no ha participado en ningun rol, por ende no tiene ninguna puntuacion valida!"
                    );
                    Console.ForegroundColor = TextColor.Warning;
                    Console.WriteLine("\nPresiona cualquier tecla para volver...");
                    Console.ReadKey(true);
                }
                else
                {
                    string[] studentRolesParsed = StringUtils.Methods.Split(studentRoles, ',');

                    bool loop2 = true;

                    while (loop2)
                    {
                        int roleSelected =
                            studentRolesParsed.Length == 1
                                ? 0
                                : Menu.InteractiveMenu(
                                    $"Selecciona el rol que deseas editar el puntaje",
                                    studentRolesParsed
                                );

                        if (roleSelected == -1)
                            loop2 = false;
                        else
                        {
                            bool loop3 = true;
                            while (loop3)
                            {
                                string roleName = studentRolesParsed[roleSelected];
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(
                                    $"Ingresa la nueva puntuacion de {studentName} con respecto a su desempeño en el rol {roleName}: "
                                );
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("(Escribe -1 para cancelar)\n");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write("Puntaje > ");
                                Console.ResetColor();

                                string newScore = Console.ReadLine()!.Trim();

                                if (int.TryParse(newScore, out int newScoreParsed))
                                {
                                    if (newScoreParsed == -1)
                                    {
                                        if (studentRolesParsed.Length == 1)
                                            loop2 = false;
                                        break;
                                    }

                                    Console.Clear();
                                    StudentManager.SetParticipationScore(
                                        ref student,
                                        roleName,
                                        newScoreParsed
                                    );
                                    Console.ForegroundColor = TextColor.Success;
                                    Console.WriteLine(
                                        $"Has modificado exitosamente la puntuacion de {studentName} en el rol {roleName}"
                                    );
                                    Console.WriteLine(
                                        "Puedes confirmar en el historial del programa"
                                    );
                                    Console.ForegroundColor = TextColor.Warning;
                                    Console.WriteLine(
                                        "\nPresiona cualquier tecla para continuar..."
                                    );
                                    Console.ReadKey(true);
                                    loop2 = false;
                                    break;
                                }
                                else
                                {
                                    Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                                    Sound.PlayAudio();
                                    Sound.SetVolume(0.3f);

                                    Console.ForegroundColor = TextColor.Error;
                                    Console.WriteLine(
                                        "\n[ERROR]: Pero y ute ta loco eh, ingrese un numero valido mio!\n"
                                    );
                                    Console.ForegroundColor = TextColor.Warning;
                                    Console.WriteLine(
                                        "Presiona cualquier tecla para volver a intentarlo..."
                                    );
                                    Console.ReadKey(true);
                                }
                            }
                        }
                    }
                }
            }
            // Editar participacion
            else if (choiceSelected == 2)
            {
                Console.Clear();
                string student = StudentManager.getStudentByName(studentName);
                string[] studentData = StudentManager.parseData(student);
                string studentRoles = studentData[3];

                if (studentRoles.Length <= 0)
                {
                    Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                    Sound.PlayAudio();
                    Sound.SetVolume(0.3f);

                    Console.ForegroundColor = TextColor.Error;
                    Console.WriteLine(
                        "[ERROR]: El estudiante seleccionado no ha participado en ningun rol, por ende no tiene ninguna participacion valida!"
                    );
                    Console.ForegroundColor = TextColor.Warning;
                    Console.WriteLine("\nPresiona cualquier tecla para volver...");
                    Console.ReadKey(true);
                }
                else
                {
                    string[] studentRolesParsed = StringUtils.Methods.Split(studentRoles, ',');

                    bool loop2 = true;

                    while (loop2)
                    {
                        int roleSelected =
                            studentRolesParsed.Length == 1
                                ? 0
                                : Menu.InteractiveMenu(
                                    $"Selecciona el rol que deseas editar la participacion",
                                    studentRolesParsed
                                );

                        if (roleSelected == -1)
                            loop2 = false;
                        else
                        {
                            bool loop3 = true;
                            while (loop3)
                            {
                                string roleName = studentRolesParsed[roleSelected];
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(
                                    $"Ingresa la nueva cantidad de participaciones de {studentName} con respecto al rol {roleName}:"
                                );
                                Console.WriteLine(
                                    "Ejemplo: Si quieres ponerle 2 participaciones, el programa lo guardara en el historial como que solo ha participado 2 veces como dicho rol"
                                );
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("(Escribe -1 para cancelar)\n");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write("Participaciones > ");
                                Console.ResetColor();

                                string newParticipations = Console.ReadLine()!.Trim();

                                if (
                                    int.TryParse(newParticipations, out int newParticipationsParsed)
                                )
                                {
                                    if (
                                        newParticipationsParsed
                                        > RuleSettings.MaxParticipationAmount
                                    )
                                    {
                                        Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                                        Sound.PlayAudio();
                                        Sound.SetVolume(0.3f);

                                        Console.ForegroundColor = TextColor.Error;
                                        Console.WriteLine(
                                            $"\n[ERROR]: Has ingresado un numero de participaciones mayor al maximo permitido ({RuleSettings.MaxParticipationAmount})!\n"
                                        );
                                        Console.ForegroundColor = TextColor.Warning;
                                        Console.WriteLine(
                                            "Presiona cualquier tecla para volver a intentarlo..."
                                        );
                                        Console.ReadKey(true);
                                    }
                                    else
                                    {
                                        if (newParticipationsParsed == -1)
                                        {
                                            if (studentRolesParsed.Length == 1)
                                                loop2 = false;
                                            break;
                                        }

                                        Console.Clear();
                                        StudentManager.editParticipation(
                                            ref student,
                                            roleName,
                                            newParticipationsParsed
                                        );
                                        Console.ForegroundColor = TextColor.Success;
                                        Console.WriteLine(
                                            $"Has modificado exitosamente las participaciones de {studentName} en el rol {roleName}"
                                        );
                                        Console.WriteLine(
                                            "Puedes confirmar en el historial del programa"
                                        );
                                        Console.ForegroundColor = TextColor.Warning;
                                        Console.WriteLine(
                                            "\nPresiona cualquier tecla para continuar..."
                                        );
                                        Console.ReadKey(true);
                                        loop2 = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                                    Sound.PlayAudio();
                                    Sound.SetVolume(0.3f);

                                    Console.ForegroundColor = TextColor.Error;
                                    Console.WriteLine(
                                        "\n[ERROR]: Pero y ute ta loco eh, ingrese un numero valido mio!\n"
                                    );
                                    Console.ForegroundColor = TextColor.Warning;
                                    Console.WriteLine(
                                        "Presiona cualquier tecla para volver a intentarlo..."
                                    );
                                    Console.ReadKey(true);
                                }
                            }
                        }
                    }
                }
            }
            // Borrar su historial
            else if (choiceSelected == 3)
            {
                bool loop2 = true;

                while (loop2)
                {
                    int confirmDelete = Menu.InteractiveMenu(
                        $"Estas seguro que deseas borrar los datos del estudiante {studentName}?\nEsto implica resetear su historial",
                        ["No, he cambiado de opinion.", "Si, estoy totalmente seguro."]
                    );

                    if (confirmDelete == -1 || confirmDelete == 0)
                        break;
                    else
                    {
                        Console.Clear();
                        StudentManager.resetStudentData(studentName);
                        Console.ForegroundColor = TextColor.Success;
                        Console.WriteLine(
                            $"Has borrado todos los datos exitosamente de {studentName}"
                        );
                        Console.ForegroundColor = TextColor.Warning;
                        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                        Console.ReadKey(true);
                        break;
                    }
                }
            }
            // Borrar al estudiante
            else if (choiceSelected == 4)
            {
                bool loop2 = true;

                while (loop2)
                {
                    int confirmDelete = Menu.InteractiveMenu(
                        $"Estas seguro que deseas borrar al estudiante {studentName}?",
                        ["No, he cambiado de opinion.", "Si, estoy totalmente seguro."]
                    );

                    if (confirmDelete == -1 || confirmDelete == 0)
                        break;
                    else
                    {
                        Console.Clear();
                        StudentManager.removeStudent(studentName);
                        Console.ForegroundColor = TextColor.Success;
                        Console.WriteLine(
                            $"Abusador, has borrado exitosamente al pobre diablo de {studentName}"
                        );
                        Console.ForegroundColor = TextColor.Warning;
                        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                        Console.ReadKey(true);
                        loop = false;
                        break;
                    }
                }
            }
        }
    }
}
