using System.Dynamic;
using Config;
using MenuUtils;
using RoleUtils;
using SelectorUtils;
using SoundUtils;
using StudentUtils;

public class StartRoulette
{
    public static void Execute(
        bool isRerolled = false,
        string? previousStudent = null,
        int? roleIndex = null
    )
    {
        string[] roles = RoleManager.RolesList;
        string[] studentsSelected = [];

        if (!StudentManager.CheckAttendance())
        {
            Sound.LoadAudio(SoundSettings.InvalidAudioPath);
            Sound.PlayAudio();
            Sound.SetVolume(0.3f);

            Console.ForegroundColor = TextColor.Error;
            Console.WriteLine("[ERROR]: Debes pasar lista de hoy antes de ejecutar la ruleta!");
            Console.ResetColor();
            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey(true);
            return;
        }

        bool abortOutsideLoop = false;
        bool noMoreStudentsReroll = false;

        for (int r = 0; r < RuleSettings.MaxStudentsPerRole && !abortOutsideLoop; r++)
        {
            for (int i = roleIndex != null ? (int)roleIndex : 0; i < roles.Length; i++)
            {
                string[] elegibleStudents = Selector.getEligibleStudents(roles[i], previousStudent);
                string studentSelected =
                    isRerolled && previousStudent != null
                        ? Selector.Reroll(ref previousStudent, roles[i])
                        : Selector.Roll(roles[i]);
                if (!EffectSettings.RouletteAnimation && isRerolled)
                    Console.Clear();
                if (studentSelected.Length <= 0)
                {
                    if (studentsSelected.Length >= RoleManager.RolesList.Length)
                    {
                        Console.ForegroundColor = TextColor.Warning;
                        Console.WriteLine(
                            "[OJO]: Ya no hay mas estudiantes para seleccionar, pero se continuara con la seleccion actual!"
                        );
                        Console.ResetColor();
                        abortOutsideLoop = true;
                        break;
                    }
                    Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                    Sound.PlayAudio();
                    Sound.SetVolume(0.3f);

                    Console.ForegroundColor = TextColor.Error;
                    Console.WriteLine("[ERROR]: Ya no hay mas estudiantes para seleccionar!");
                    Console.ResetColor();
                    Console.WriteLine("\nPresiona una tecla para continuar...");
                    Console.ReadKey(true);

                    if (!isRerolled)
                    {
                        StudentManager.disableAllActiveRoles();
                        return;
                    }
                    else
                    {
                        if (previousStudent != null)
                        {
                            studentSelected = previousStudent;
                            noMoreStudentsReroll = true;
                        }
                    }
                }
                string studentSelectedName = StudentManager.getName(studentSelected);

                if (EffectSettings.RouletteAnimation && !noMoreStudentsReroll)
                {
                    if (EffectSettings.RouletteAnimSound)
                        Sound.PlayAudioLoop(SoundSettings.RouletteAudioPath);

                    RouletteAnim.Execute(
                        elegibleStudents,
                        studentSelectedName,
                        roles[i],
                        EffectSettings.RouletteAnimSteps
                    );

                    if (EffectSettings.RouletteAnimSound)
                        Sound.StopAudio();
                }

                if (noMoreStudentsReroll)
                {
                    Sound.StopAudio();
                    Console.Clear();
                    
                    // Le asignamos el rol
                    StudentManager.addRole(ref studentSelected, roles[i]);
                    // Habilitamos el rol
                    StudentManager.enableActiveRole(ref studentSelected, roles[i]);
                    // Establecemos la fecha de participacion
                    StudentManager.SetLastParticipationDate(ref studentSelected, roles[i]);

                    Console.ForegroundColor = TextColor.Success;
                    Console.WriteLine(
                        $"\nEl estudiante: ``{studentSelectedName}`` ha sido seleccionado por descarte para el rol ``{roles[i]}``."
                    );
                    Console.ForegroundColor = TextColor.Warning;
                    Console.WriteLine(
                        $"\nPresiona cualquier tecla para continuar{(i != roles.Length - 1 ? " con la seleccion del proximo rol" : "")}..."
                    );
                    Console.ReadKey(true);
                    ArrayUtils.Methods.Push(ref studentsSelected, studentSelected);
                }
                else
                {
                    Sound.LoadAudio(SoundSettings.SelectedAudioPath);
                    Sound.PlayAudio();
                    Sound.SetVolume(SoundSettings.DefaultVolume);

                    Console.ForegroundColor = TextColor.Success;
                    Console.WriteLine(
                        $"\nEl estudiante: ``{studentSelectedName}`` ha sido seleccionado para el rol ``{roles[i]}``."
                    );
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        "Presiona [E] para volver a seleccionar otro estudiante nuevo para este rol"
                    );
                    Console.ForegroundColor = TextColor.Warning;
                    Console.WriteLine(
                        $"\nPresiona cualquier tecla (excepto [E]) para continuar{(i != roles.Length - 1 ? " con la seleccion del proximo rol" : "")}..."
                    );
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.E:
                            Execute(true, studentSelected, i);
                            return;
                        default:
                            ArrayUtils.Methods.Push(ref studentsSelected, studentSelected);
                            break;
                    }
                    Sound.StopAudio();
                }
            }
        }

        int choiceSelected = Menu.InteractiveMenu(
            $"Deseas iniciar un tiempo limite para esta seleccion?",
            ["Si, deseo iniciar una cuenta regresiva.", "No, dejalo ahi que ta' bien."]
        );

        if (choiceSelected == 0)
        {
            RouletteTimer.Execute(studentsSelected);
        }
        else if (choiceSelected == 1)
        {
            ScoreParticipants.Execute(studentsSelected);
        }
    }
}
