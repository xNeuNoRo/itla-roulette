using Config;
using SoundUtils;
using StudentUtils;

// TODO:
// -- Implementar asistencia con la logica de la ruleta

namespace SelectorUtils
{
    public class Selector
    {
        public static string Roll(string role, string? previousStudent = null)
        {
            string[] studentsWithThisRole = ArrayUtils.Methods.Filter(
                StudentManager.studentsList,
                student =>
                    StudentManager.hasRole(student, role)
                    && StudentManager.isActiveRole(student, role)
            );

            if (
                (!RuleSettings.AllowRepeatRoles && StudentManager.getActiveRoleCount(role) > 0)
                || (studentsWithThisRole.Length >= RuleSettings.MaxStudentsPerRole)
            )
            {
                for (int i = 0; i < studentsWithThisRole.Length; i++)
                {
                    StudentManager.disableActiveRole(ref studentsWithThisRole[i]);
                }
            }

            string[] eligibleStudents = getEligibleStudents(role, previousStudent);

            if (eligibleStudents.Length == 0)
            {
                return "";
            }

            // Seleccionar aleatoriamente
            Random randomNum = new Random();
            int randomIndex = randomNum.Next(eligibleStudents.Length);
            string studentSelected = eligibleStudents[randomIndex];

            // Le asignamos el rol
            StudentManager.addRole(ref studentSelected, role);
            // Habilitamos el rol
            StudentManager.enableActiveRole(ref studentSelected, role);
            // Establecemos la fecha de participacion
            StudentManager.SetLastParticipationDate(ref studentSelected, role);

            // Devolvemos el estudiante seleccionado para el rol
            return studentSelected;
        }

        public static string Reroll(ref string previousStudent, string role)
        {
            try
            {
                StudentManager.removeParticipationDate(ref previousStudent, role);
                StudentManager.removeActiveRole(ref previousStudent);

                // Ya habiendo removido el rol del anterior estudiante q fue seleccionado,
                // Volvemos a hacer otro Roll normalito
                string newStudentSelected = Roll(role, previousStudent);

                return newStudentSelected;
            }
            catch (Exception err)
            {
                // Si ocurre un error lo arrojamos en consola
                Console.WriteLine($"Ha ocurrido un error inesperado:\n{err}");
                // Tambien lo guardamos en el archivo de logs
                File.AppendAllText(FileSettings.ProgramLogPath, err.Message + Environment.NewLine);
                // Devolver string vacio
                return "";
            }
        }

        public static string[] getEligibleStudents(string role, string? previousStudent = null)
        {
            string[] eligibleStudents;

            if (!RuleSettings.AllowRepeatStudents)
            {
                eligibleStudents = ArrayUtils.Methods.Filter(
                    StudentManager.studentsList,
                    student =>
                        !StudentManager.hasRole(student, role)
                        && !StudentManager.hasActiveRole(student)
                        && StudentManager.isPresent(student)
                        && (previousStudent == null || student != previousStudent)
                );
            }
            else
            {
                eligibleStudents = ArrayUtils.Methods.Filter(
                    StudentManager.studentsList,
                    student =>
                        !StudentManager.hasActiveRole(student)
                        && StudentManager.isPresent(student)
                        && StudentManager.getRoleParticipations(student, role)
                            < RuleSettings.MaxParticipationAmount
                        && (previousStudent == null || student != previousStudent)
                );
            }

            return eligibleStudents;
        }

        public static void CountdownTimer(int minutes)
        {
            int totalSeconds = minutes * 60;
            bool hasPlayedAudio = false;
            bool hasPaused = false;
            int actualAudioTime = 0;

            while (totalSeconds >= 0)
            {
                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Escape)
                    {
                        Sound.StopAudio();
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nTiempo restante cancelado!");
                        Console.ResetColor();
                        return;
                    }
                    else if (key == ConsoleKey.P)
                    {
                        if (hasPlayedAudio)
                        {
                            Sound.StopAudio();
                            actualAudioTime = Sound.GetAudioCurrentTime();
                            hasPlayedAudio = false;
                            hasPaused = true;
                        }

                        ConsoleKey selectedKey = Console.ReadKey(true).Key;

                        bool pausedLoop = true;
                        while (pausedLoop)
                        {
                            if (selectedKey == ConsoleKey.R)
                            {
                                if (hasPaused)
                                {
                                    Sound.LoadAudio(SoundSettings.Timer60AudioPath);
                                    Sound.SeekAudio(actualAudioTime);
                                    Sound.SetVolume(0.1f);
                                    Sound.PlayAudio();
                                    hasPlayedAudio = true;
                                    hasPaused = false;
                                }
                                pausedLoop = false;
                            }
                        }
                    }
                    // Ignorar otras teclas
                }

                int mins = totalSeconds / 60;
                int secs = totalSeconds % 60;

                string timeMessage = $"Tiempo restante: {mins:D2}:{secs:D2}   "; // D2 => 0=00, 4=04, 8=08 y asi..

                if (totalSeconds <= 63)
                {
                    if (!hasPlayedAudio)
                    {
                        Sound.LoadAudio(SoundSettings.Timer60AudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.1f);
                        hasPlayedAudio = true;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (totalSeconds <= 120)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                Console.WriteLine(timeMessage);

                Thread.Sleep(1000);
                totalSeconds--;
            }

            Sound.StopAudio();
            Console.ResetColor();
            Console.WriteLine("\nTiempo finalizado!");
        }
    }
}
