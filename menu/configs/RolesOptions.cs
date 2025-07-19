using Config;
using MenuUtils;
using RoleUtils;
using StudentUtils;
using SoundUtils;

public class RolesOptions
{
    public static void Execute(string roleName)
    {
        bool loop = true;

        while (loop)
        {
            int choiceSelected = Menu.InteractiveMenu(
                $"Estas editando el rol {roleName}",
                ["Editar nombre", "Borrar el rol"]
            );

            if (choiceSelected == -1)
                loop = false;
            // Editar nombre
            else if (choiceSelected == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Ingresa el nuevo nombre del rol: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "(Escribe -1 para cancelar o presiona Enter sin escribir nada)\n"
                );
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Nombre > ");
                Console.ResetColor();

                string newRoleName = Console.ReadLine()!.Trim();

                if (newRoleName != "-1" && !StringUtils.Methods.IsNullOrWhiteSpace(newRoleName))
                {
                    RoleManager.EditRoleName(roleName, newRoleName);
                    StudentManager.editAllRoleNameData(roleName, newRoleName);
                    loop = false;
                    Execute(newRoleName);
                }
            }
            else if (choiceSelected == 1)
            {
                bool loop2 = true;

                while (loop2)
                {
                    int confirmDelete = Menu.InteractiveMenu(
                        $"Estas seguro que deseas borrar el rol {roleName}?\nOJO: SI BORRAS ESTE ROL, SE BORRARAN TODAS LAS PARTICIPACIONES\nDE LOS ESTUDIANTES RELACIONADAS AL ROL.",
                        ["No, he cambiado de opinion.", "Si, estoy totalmente seguro."]
                    );

                    if (confirmDelete == -1 || confirmDelete == 0)
                        break;
                    else
                    {
                        if (RoleManager.RolesList.Length == RuleSettings.MinRolesRequired)
                        {
                            Sound.LoadAudio(SoundSettings.InvalidAudioPath);
                    Sound.PlayAudio();
                    Sound.SetVolume(0.3f);

                            Console.ForegroundColor = TextColor.Error;
                            Console.WriteLine(
                                "\n[ERROR]: No es posible borrar este rol. Ya tienes la cantidad minima requerida de roles activos."
                            );
                            Console.ForegroundColor = TextColor.Warning;
                            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                            Console.ReadKey(true);
                            loop2 = false;
                        }
                        else
                        {
                            Console.Clear();
                            RoleManager.RemoveRole(roleName);
                            StudentManager.deleteAllRoleData(roleName);
                            Console.ForegroundColor = TextColor.Success;
                            Console.WriteLine(
                                $"Has removido exitosamente el rol {roleName} de la lista de roles!"
                            );
                            Console.ForegroundColor = TextColor.Warning;
                            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                            Console.ReadKey(true);
                            loop = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}
