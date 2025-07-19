using Config;
using MenuUtils;
using RoleUtils;
using SoundUtils;

public class ManageRoles
{
    public static void Execute()
    {
        string menuTitle = "Lista de Roles";
        string[] rolesList = RoleManager.RolesList;

        int rows = 10;
        int totalPages = rolesList.Length / rows;
        if (rolesList.Length % rows != 0)
            totalPages++;
        int currentPage = 0;

        string[][] pages = [];

        for (int i = 1; i <= totalPages; i++)
            ArrayUtils.Methods.Push(ref pages, Menu.GetPagination(rolesList, i, 10));

        bool loop = true;
        while (loop)
        {
            int choiceSelected = Menu.InteractiveMenu(
                menuTitle,
                pages[currentPage],
                pages,
                currentPage,
                rows,
                false,
                true
            );

            if (choiceSelected == -1)
                loop = false;
            else if (choiceSelected == -999)
            {
                bool loop2 = true;
                while (loop2)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Ingresa el nombre del rol que deseas agregar: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("(Escribe -1 para cancelar)\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("Rol > ");
                    Console.ResetColor();

                    string roleName = Console.ReadLine()!.Trim();
                    if (roleName == "-1")
                        Execute();

                    if (
                        ArrayUtils.Methods.Includes(
                            ArrayUtils.Methods.Map(rolesList, role => role.ToLower()),
                            roleName.ToLower()
                        )
                    )
                    {
                        Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.3f);
                        
                        Console.ForegroundColor = TextColor.Error;
                        Console.WriteLine(
                            "\n[ERROR]: El rol que ingresaste ya se encuentra en la lista de roles actual!\nVuelve a intentarlo con un rol distinto"
                        );
                        Console.ForegroundColor = TextColor.Warning;
                        Console.WriteLine("\nPresiona cualquier tecla para volver a intentarlo...");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        RoleManager.AddNewRole(roleName);
                        loop = false;
                        Execute();
                        break;
                    }
                }
            } else if (choiceSelected >= 0)
            {
                RolesOptions.Execute(rolesList[choiceSelected]);
                loop = false;
                Execute();
            }
        }
    }
}
