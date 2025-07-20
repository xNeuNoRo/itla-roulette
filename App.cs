using Config;
using Handlers;
using MenuUtils;
using SoundUtils;
using StudentUtils;

// Interactive Roulette Project
namespace IR_Project
{
    class App
    {
        static void Main()
        {
            Console.CursorVisible = AppSettings.HideCursor;

            // Intro del programa
            if (!AppSettings.SkipIntro)
            {
                Console.Clear();
                Sound.LoadAudio(SoundSettings.IntroAudioPath);
                Sound.PlayAudio();
                Sound.SetVolume(SoundSettings.DefaultVolume);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Menu.AnimatedLogo();

                Console.ForegroundColor = TextColor.Warning;

                Console.WriteLine("\nPresiona cualquier tecla para entrar al programa...");
                Console.ReadKey(true);

                Sound.StopAudio();
                Console.ResetColor();
                Console.Clear();
            }

            // Manejar estudiantes con participacion previa pero que no han sido puntuados
            string[] studentsWithActiveRole = ArrayUtils.Methods.Filter(
                StudentManager.studentsList,
                student => StudentManager.hasActiveRole(student)
            );

            if (studentsWithActiveRole.Length > 0)
            {
                bool confirmLoop = true;
                while (confirmLoop)
                {
                    int confirmChoice = Menu.InteractiveMenu(
                        "Se ha detectado que hay estudiantes con roles activos!\nPosiblemente de una ejecucion previa que no finalizaste debidamente\nDeseas establecer su puntaje (si ya participo)\no deseas que se reseteen sus datos (si no ha participado)?",
                        [
                            "Ver los estudiantes afectados",
                            "Deseo puntuar los estudiantes",
                            "Deseo resetear los datos de los estudiantes",
                        ]
                    );

                    if (confirmChoice == 0)
                    {
                        // Se espera max 1 estudiante por rol activo, pero en caso de que haya mas, lo ideal seria escalarlo con paginacion
                        Menu.InteractiveMenu(
                            "Estudiantes con roles activos",
                            ArrayUtils.Methods.Map(
                                studentsWithActiveRole,
                                student => StudentManager.getName(student)
                            )
                        );
                    }
                    else if (confirmChoice == 1)
                    {
                        confirmLoop = false;
                        ScoreParticipants.Execute(studentsWithActiveRole);
                    }
                    else
                    {
                        confirmLoop = false;
                        foreach (string student in studentsWithActiveRole)
                            StudentManager.resetStudentIncompleteData(student);
                    }
                }
            }

            // Menu de inicio
            bool loop = true;

            while (loop)
            {
                int choiceSelected = Menu.InteractiveMenu(
                    "Interactive Roulette Project\nDeveloped By Angel G. M. 2025-1122",
                    MenuHandler.StartMenuOptions,
                    null,
                    0,
                    0,
                    false,
                    false,
                    true
                );

                if (choiceSelected == -1)
                {
                    Console.ResetColor();
                    break;
                }
                else
                    MenuHandler.StartMenu(choiceSelected);
            }
        }
    }
}
