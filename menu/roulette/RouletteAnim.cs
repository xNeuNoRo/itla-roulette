using Config;
using MenuUtils;
using StudentUtils;

public class RouletteAnim
{
    public static void Execute(
        string[] allStudents,
        string studentSelected,
        string role,
        int steps = 120
    )
    {
        int visibleStudentsCount = 9;

        if (allStudents.Length == 0 || visibleStudentsCount < 3)
            return;

        allStudents = ArrayUtils.Methods.Randomizer(
            ArrayUtils.Methods.Map(
                allStudents,
                student => StudentManager.getName(student)
            )
        );

        int total = allStudents.Length;
        int index = 0;

        // 'index' final cuando finalizan los steps de la ruleta
        int finalIndex = (steps - 1) % total;
        // visibleStudentsCount / 2 = el estudiante en el medio seleccionado
        // % total = para volver al principio del array en caso de que sea mayor al ultimo indice del array
        int selectedIndex = (finalIndex + visibleStudentsCount / 2) % total;

        // Cambiamos al estudiante que sera seleccionado a dicha posicion
        allStudents = ArrayUtils.Methods.Swap(allStudents, studentSelected, selectedIndex);

        bool shouldFinishRoulette = false;
        ConsoleColor selectorColor = ConsoleColor.White;
        Random randomStep = new Random();

        for (int s = 0; s < steps; s++)
        {
            int CalculatedDelay = shouldFinishRoulette
                ? GetDelayForStep(s, steps, 0, 0)
                : GetDelayForStep(s, steps, 0, EffectSettings.RouletteAnimMaxSpeed);
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    shouldFinishRoulette = true;
                }
            }

            string[] visibleStudents = new string[visibleStudentsCount];

            for (int i = 0; i < visibleStudentsCount; i++)
            {
                // rellenar el array con la vista actual
                int realIndex = (index + i) % total;
                visibleStudents[i] = allStudents[realIndex];
            }

            // steps - s = los steps restantes
            int WarningStep = randomStep.Next(25, 32);
            if (steps - s <= WarningStep)
                selectorColor = ConsoleColor.Yellow;

            int RedAlertStep = randomStep.Next(7, 15);
            if (steps - s <= RedAlertStep)
                selectorColor = ConsoleColor.Red;

            Console.Clear();
            Menu.DrawRouletteAnim(visibleStudents, role, selectorColor);

            Console.ForegroundColor = TextColor.Warning;
            // Imprimir siempre el mensaje, excepto en el ultimo step, por el eso el [steps-1]
            if (s < steps - 1)
                Console.WriteLine(
                    "\nPuedes finalizar la animacion de la ruleta cuando quieras presionando [R]      "
                );

            Thread.Sleep(CalculatedDelay);

            // Avanzar al siguiente indice + 1 y mantener dentro del limite del array con % total
            index = (index + 1) % total;
        }
    }

    public static int GetDelayForStep(
        int currentStep,
        int totalSteps,
        int minDelay = 100,
        int maxDelay = 800
    )
    {
        if (totalSteps <= 0)
            return maxDelay;

        double t = (double)currentStep / totalSteps;
        double factor = Math.Pow(t, 4); // Aumenta fuertemente hacia el final

        return (int)(minDelay + (maxDelay - minDelay) * factor);
    }
}
