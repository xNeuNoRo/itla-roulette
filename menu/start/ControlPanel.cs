using Handlers;
using MenuUtils;

public class ControlPanel
{
    public static void Execute()
    {
        bool loop = true;
        while (loop)
        {
            int choiceSelected = Menu.InteractiveMenu(
                "Panel De Control",
                MenuHandler.ControlPanelOptions
            );

            if (choiceSelected == -1)
                loop = false;

            if (choiceSelected == 0)
                ManageStudents.Execute();
            else if (choiceSelected == 1)
                ManageRoles.Execute();
        }
    }
}
