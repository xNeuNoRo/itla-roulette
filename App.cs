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
