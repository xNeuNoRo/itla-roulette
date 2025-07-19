using Config;
using MenuUtils;
using SelectorUtils;
using StudentUtils;
using SoundUtils;

public class RouletteTimer
{
    public static void Execute(string[] studentsSelected)
    {
        int timeParsed = 0;
        bool loop = true;
        while (loop)
        {
            Console.Clear();
            Console.ForegroundColor = TextColor.Questions;

            Console.WriteLine("────────────────────────────────────────");
            Console.WriteLine("Ingresa el tiempo de la cuenta regresiva");
            Console.WriteLine("────────────────────────────────────────\n");
            Console.ForegroundColor = TextColor.Answer;
            Console.Write("Tiempo en minutos > ");
            string timeInMinutes = Console.ReadLine()!;

            if (!int.TryParse(timeInMinutes, out timeParsed) || timeParsed <= 0)
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
            }
            else
            {
                loop = false;
            }
        }

        int[] roleLengths = ArrayUtils.Methods.Map(
            studentsSelected,
            student => ("- Rol: " + StudentManager.getActiveRole(student)).Length
        );

        int[] nameLengths = ArrayUtils.Methods.Map(
            studentsSelected,
            student => StudentManager.getName(student).Length
        );

        int panelWidth =
            ArrayUtils.Methods.getMax(ArrayUtils.Methods.Concat(roleLengths, nameLengths)) * 2 + 6;

        Console.Clear();
        Menu.DrawRoulettePanel(studentsSelected, panelWidth);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(
            "TIP: Puedes finalizar el temporizador en cualquier momento presionando ESC\nPresiona [P] para pausar la cuenta regresiva y [R] para reanudarla.\n"
        );
        Console.ResetColor();
        Selector.CountdownTimer(timeParsed);

        Console.ForegroundColor = TextColor.Warning;
        Console.WriteLine("\nPresiona cualquier tecla para colocar la puntuacion...");
        Console.ReadKey(true);

        Console.ResetColor();
        ScoreParticipants.Execute(studentsSelected);
    }
}
