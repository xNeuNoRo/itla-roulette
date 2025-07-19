using Config;
using Handlers;
using MenuUtils;
using SoundUtils;

// Interactive Roulette Project
namespace IR_Project
{
    class App
    {
        static void Main()
        {
            // Intro del programa
            Console.Clear();
            Sound.LoadAudio(SoundSettings.IntroAudioPath);
            Sound.PlayAudio();
            Sound.SetVolume(SoundSettings.DefaultVolume);

            Menu.ProgramLogo(ConsoleColor.Cyan);
            Console.ForegroundColor = TextColor.Warning;

            Console.WriteLine("Presiona cualquier tecla para entrar al programa...");
            Console.ReadKey(true);

            Sound.StopAudio();
            Console.ResetColor();

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
