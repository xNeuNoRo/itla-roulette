using Config;
using SoundUtils;
using StudentUtils;

public class ScoreParticipants
{
    public static void Execute(string[] studentsSelected)
    {
        for (int i = 0; i < studentsSelected.Length; i++)
        {
            Console.Clear();
            string student = studentsSelected[i];
            string studentRole = StudentManager.getActiveRole(student);

            Console.ForegroundColor = TextColor.Questions;
            Console.WriteLine("─────────────────────────────────────────────────────────");
            Console.WriteLine(
                $"Del 0-10 como puntuacion considerada\ncomo consideras que fue el desempeño del estudiante ``{StudentManager.getName(student)}``\nmientras se desenvolvia en el papel como ``{studentRole}``?"
            );
            Console.WriteLine("─────────────────────────────────────────────────────────");

            Console.ForegroundColor = TextColor.Answer;
            Console.Write("\nPuntuacion > ");
            string studentScore = Console.ReadLine()!;

            if (!int.TryParse(studentScore, out int scoreParsed))
            {
                Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                Sound.PlayAudio();
                Sound.SetVolume(0.3f);

                Console.ForegroundColor = TextColor.Error;
                Console.WriteLine(
                    "\n[ERROR]: Has insertado un numero invalido!\nIngresa nuevamente un numero entero valido.\n"
                );
                Console.ResetColor();
                Console.WriteLine("Presiona cualquier tecla para volver a intentarlo...");
                Console.ReadKey(true);

                i--;
                continue;
            }

            if (scoreParsed < 0 || scoreParsed > 10)
            {
                Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                Sound.PlayAudio();
                Sound.SetVolume(0.3f);

                Console.ForegroundColor = TextColor.Error;
                Console.WriteLine(
                    "\n[ERROR]: Debes ingresar una puntuacion de 0-10!\nIngresa nuevamente una puntuacion valida.\n"
                );
                Console.ResetColor();
                Console.WriteLine("Presiona cualquier tecla para volver a intentarlo...");
                Console.ReadKey(true);

                i--;
                continue;
            }

            StudentManager.SetParticipationScore(ref student, studentRole, scoreParsed);
            StudentManager.disableActiveRole(ref student);
        }

        Console.ForegroundColor = TextColor.Warning;
        Console.WriteLine("\nPresiona cualquier tecla para volver al menu principal...");
        Console.ReadKey(true);
        Console.ResetColor();
    }
}
