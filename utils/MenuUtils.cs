using Config;
using SoundUtils;
using StudentUtils;

namespace MenuUtils
{
    public class Menu
    {
        // Para ocultar los consejos
        private static bool ShouldHideHints = false;

        public static void ProgramLogo(ConsoleColor color)
        {
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(
                    @"
/* ▐▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▌ */
/* ▐                                                                               ▌ */
/* ▐                                                                               ▌ */
/* ▐     ___       _                      _   _                                    ▌ */
/* ▐    |_ _|_ __ | |_ ___ _ __ __ _  ___| |_(_)_   _____                          ▌ */
/* ▐     | || '_ \| __/ _ \ '__/ _` |/ __| __| \ \ / / _ \                         ▌ */
/* ▐     | || | | | ||  __/ | | (_| | (__| |_| |\ V /  __/                         ▌ */
/* ▐    |___|_| |_|\__\___|_|  \__,_|\___|\__|_|_\_/ \___|     _           _       ▌ */
/* ▐    |  _ \ ___  _   _| | ___| |_| |_ ___  |  _ \ _ __ ___ (_) ___  ___| |_     ▌ */
/* ▐    | |_) / _ \| | | | |/ _ \ __| __/ _ \ | |_) | '__/ _ \| |/ _ \/ __| __|    ▌ */
/* ▐    |  _ < (_) | |_| | |  __/ |_| ||  __/ |  __/| | | (_) | |  __/ (__| |_     ▌ */
/* ▐    |_| \_\___/ \__,_|_|\___|\__|\__\___| |_|   |_|  \___// |\___|\___|\__|    ▌ */
/* ▐                                                        |__/                   ▌ */
/* ▐                                                                               ▌ */
/* ▐                                                                               ▌ */
/* ▐▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▌ */
                    "
                );
                Console.ResetColor();
            }
            catch (Exception err)
            {
                // Si ocurre un error lo arrojamos en consola
                Console.WriteLine($"Ha ocurrido un error inesperado:\n{err}");
                // Tambien lo guardamos en el archivo de logs
                File.AppendAllText(FileSettings.ProgramLogPath, err.Message + Environment.NewLine);
            }
        }

        private static void DrawInteractiveMenu(
            string menuTitle,
            string[] choices,
            int selectedIndex,
            int width
        )
        {
            Console.Clear();
            int leftHalf = width / 2;
            int rightHalf = width - leftHalf;
            string[] menuTitleSplitted = StringUtils.Methods.Split(menuTitle, '\n');

            int maxTitleLength = ArrayUtils.Methods.getMax(
                ArrayUtils.Methods.Map(menuTitleSplitted, title => title.Length)
            );
            int maxChoicesLength = ArrayUtils.Methods.getMax(
                ArrayUtils.Methods.Map(choices, c => c.Length)
            );

            int maxLengthInMenu =
                maxTitleLength >= maxChoicesLength ? maxTitleLength : maxChoicesLength;

            int calculatedTabsVal =
                maxLengthInMenu > 70 ? 2
                : maxLengthInMenu > 50 ? 3
                : 4;

            string calculatedTabs = StringUtils.Methods.fillRight("\t", calculatedTabsVal, '\t');

            // Espacios para alinear el menu
            Console.WriteLine("\n\n");

            // Header
            WriteColorLines(
                $"{calculatedTabs}╭" + StringUtils.Methods.fillRight("━", leftHalf, '━'),
                ConsoleColor.Blue
            );
            WriteColorLines(
                StringUtils.Methods.fillRight("━", rightHalf, '━') + "╮",
                ConsoleColor.Red
            );

            // Menu Title
            foreach (string menuTitleParsed in menuTitleSplitted)
            {
                if (!StringUtils.Methods.IsNullOrWhiteSpace(menuTitleParsed))
                {
                    string centeredText = CenterText(menuTitleParsed, width);
                    int splitIndex = centeredText.Length / 2;

                    WriteColorLines(
                        $"\n{calculatedTabs}┃"
                            + StringUtils.Methods.Substring(centeredText, 0, splitIndex),
                        ConsoleColor.Blue
                    );
                    WriteColorLines(
                        StringUtils.Methods.Substring(centeredText, splitIndex) + "┃",
                        ConsoleColor.Red
                    );
                }
                else
                {
                    WriteColorLines(
                        StringUtils.Methods.fillRight($"\n{calculatedTabs}┃", width + 2),
                        ConsoleColor.Blue
                    );
                    WriteColorLines(
                        StringUtils.Methods.fillRight("┃", width + 2),
                        ConsoleColor.Red
                    );
                }
            }

            // Footer
            WriteColorLines(
                $"\n{calculatedTabs}┃" + StringUtils.Methods.fillRight("━", leftHalf, '━'),
                ConsoleColor.Blue
            );
            WriteColorLines(
                StringUtils.Methods.fillRight("━", rightHalf, '━') + "┃\n",
                ConsoleColor.Red
            );

            // Opciones
            for (int i = 0; i < choices.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = TextColor.Selector;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                // Choices
                Console.WriteLine($"{calculatedTabs}┃" + CenterText(choices[i], width) + "┃");
            }

            // Choices Footer
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(
                $"{calculatedTabs}╰" + StringUtils.Methods.fillRight("━", width, '━') + "╯"
            );
            Console.ResetColor();
        }

        private static void WriteColorLines(string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(str);
        }

        public static int InteractiveMenu(
            string menuTitle,
            string[] choices,
            string[][]? pages = null,
            int currentPage = 0,
            int rowsPerPage = 10,
            bool isStudentsList = false,
            bool isRolesList = false,
            bool isMainMenu = false
        )
        {
            // TODO:
            // Mapear controles WASD-Barra de Espacio/Flechas-Enter
            int width = 50;
            int maxLengthChoices = ArrayUtils.Methods.getMax(
                ArrayUtils.Methods.Map(choices, c => c.Length)
            );
            int maxLengthMenuTitle = ArrayUtils.Methods.getMax(
                ArrayUtils.Methods.Map(
                    StringUtils.Methods.Split(menuTitle, '\n'),
                    menuPart => menuPart.Length
                )
            );

            // Wao no me vuelvan a meter a diseñar menus de nuevo, despues de 2h llegue a esta conclusion:
            // width - 5 para mantener un margen de 5 y que no ocurran desalineados medio raros
            if (width - 5 <= maxLengthMenuTitle || width - 5 <= maxLengthChoices)
            {
                width =
                    maxLengthMenuTitle > maxLengthChoices
                        ? maxLengthMenuTitle + 4
                        : maxLengthChoices + 4;
            }

            int selectedIndex = 0;

            while (true)
            {
                DrawInteractiveMenu(menuTitle, choices, selectedIndex, width);

                if (pages != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (isStudentsList)
                        Console.WriteLine(
                            $"\t\t\tPagina {currentPage + 1}/{pages.Length} - [E] Agregar nuevo estudiante | [R] Importar estudiantes"
                        );
                    else if (isRolesList)
                        Console.WriteLine(
                            $"\t\t\tPagina {currentPage + 1}/{pages.Length} - [E] Agregar nuevo rol"
                        );
                    else
                        Console.WriteLine($"Pagina {currentPage + 1}/{pages.Length}");

                    Console.ForegroundColor = TextColor.Warning;
                    if (!ShouldHideHints)
                        Console.WriteLine(
                            "\n\t\t\tNavegacion: [↑][←][↓][→] / [W][A][S][D]  -  Interaccion: [Espacio] / [⏎] Enter"
                        );
                    Console.ForegroundColor = TextColor.Success;
                    Console.WriteLine(
                        "\n\t\t\tPresiona [H] para mostrar/ocultar las sugerencias de navegacion/interaccion."
                    );
                    Console.ResetColor();
                    int? result = HandleInteractiveKeys(
                        Console.ReadKey(true).Key,
                        ref selectedIndex,
                        choices,
                        true,
                        pages.Length,
                        currentPage,
                        rowsPerPage,
                        isStudentsList,
                        isRolesList,
                        isMainMenu
                    );
                    if (result != null)
                        return (int)result;
                }
                else
                {
                    Console.ForegroundColor = TextColor.Warning;
                    if (!ShouldHideHints)
                        Console.WriteLine(
                            "\n\t\t\tNavegacion: [↑][↓] / [W][S]\n\t\t\tInteraccion: [Espacio] / [⏎] Enter"
                        );
                    Console.ForegroundColor = TextColor.Success;
                    Console.WriteLine(
                        "\n\t\t\tPresiona [H] para mostrar/ocultar las sugerencias de navegacion/interaccion."
                    );
                    int? result = HandleInteractiveKeys(
                        Console.ReadKey(true).Key,
                        ref selectedIndex,
                        choices,
                        false,
                        0,
                        currentPage,
                        rowsPerPage,
                        isStudentsList,
                        isRolesList,
                        isMainMenu
                    );
                    if (result != null)
                        return (int)result;
                }
            }
        }

        public static int? HandleInteractiveKeys(
            ConsoleKey Key,
            ref int selectedIndex,
            string[] choices,
            bool pagination,
            int totalPages,
            int currentPage,
            int rowsPerPage,
            bool isStudentsList,
            bool isRoleList,
            bool isMainMenu
        )
        {
            if (pagination)
            {
                switch (Key)
                {
                    case ConsoleKey.Escape:
                    {
                        Sound.LoadAudio(SoundSettings.BackAudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.3f);

                        return -1;
                    }
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        {
                            Sound.LoadAudio(SoundSettings.BackAudioPath);
                            Sound.PlayAudio();
                            Sound.SetVolume(0.3f);
                            selectedIndex =
                                (selectedIndex == 0) ? choices.Length - 1 : selectedIndex - 1;
                        }
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        selectedIndex =
                            (selectedIndex == choices.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        if (totalPages > 1)
                            return -2;
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        if (totalPages > 1)
                            return -3;
                        break;
                    case ConsoleKey.E:
                        if (isStudentsList || isRoleList)
                            return -999;
                        break;
                    case ConsoleKey.R:
                        if (isStudentsList)
                            return -998;
                        break;
                    case ConsoleKey.H:
                        ShouldHideHints = !ShouldHideHints;
                        break;

                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                    {
                        Console.Clear();
                        Sound.LoadAudio(SoundSettings.EnterAudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.3f);
                        return (currentPage * rowsPerPage) + selectedIndex;
                    }
                }
            }
            else
            {
                switch (Key)
                {
                    case ConsoleKey.Escape:
                    {
                        Sound.LoadAudio(SoundSettings.BackAudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.3f);

                        if (isMainMenu)
                        {
                            int selectedChoice = InteractiveMenu(
                                "Estas seguro que deseas salir?",
                                ["Si, deseo salir.", "No, no quiero salir ahora."]
                            );

                            if (selectedChoice == 0)
                            {
                                Console.Clear();
                                return -1;
                            }
                        }
                        else
                            return -1;

                        break;
                    }
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        selectedIndex =
                            (selectedIndex == 0) ? choices.Length - 1 : selectedIndex - 1;
                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        selectedIndex =
                            (selectedIndex == choices.Length - 1) ? 0 : selectedIndex + 1;
                        break;

                    case ConsoleKey.H:
                        ShouldHideHints = !ShouldHideHints;
                        break;

                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                    {
                        while (Console.KeyAvailable)
                            Console.ReadKey(true); // limpia el buffer
                        Sound.LoadAudio(SoundSettings.EnterAudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.3f);
                        Console.Clear();
                        return selectedIndex;
                    }
                }
            }

            return null;
        }

        public static T[] GetPagination<T>(T[] array, int page = 1, int rowsPerPage = 15)
        {
            if (array == null || array.Length == 0)
                return [];

            int totalPages = array.Length / rowsPerPage;

            if (array.Length % rowsPerPage != 0)
                totalPages++;

            if (page < 1 || page > totalPages)
                page = 1;

            int offset = rowsPerPage * (page - 1);

            return ArrayUtils.Methods.TakeFirstN(array, offset, offset + rowsPerPage);
        }

        public static void HandlePagination(
            ref int choiceSelected,
            string[][] pages,
            ref int currentPage
        )
        {
            while (choiceSelected == -2 || choiceSelected == -3)
            {
                // ConsoleKey.LeftArrow
                if (choiceSelected == -2)
                {
                    currentPage = currentPage == 0 ? pages.Length - 1 : currentPage - 1;
                }
                // ConsoleKey.RightArrow
                else if (choiceSelected == -3)
                {
                    currentPage = currentPage == pages.Length - 1 ? 0 : currentPage + 1;
                }
                choiceSelected = -4;
            }
        }

        private static string CenterText(string textToCenter, int widthOfMenu)
        {
            // "               Hola mundo               " => 40 caracteres en total
            // "Hola mundo" => 10 caracteres
            // 40-10 = 30 caracteres de espacios en blanco
            // 30/2 = 15 caracteres de ambos lados en blanco
            // 40-10-caracteresIzquierda = 15 (si fuera impar se ajustaria mejor)
            int whiteSpacesBetweenTextLeft = (widthOfMenu - textToCenter.Length) / 2;

            if (whiteSpacesBetweenTextLeft < 0)
                whiteSpacesBetweenTextLeft = 0;

            int whiteSpacesBetweenTextRight =
                widthOfMenu - textToCenter.Length - whiteSpacesBetweenTextLeft;

            return StringUtils.Methods.fillRight(" ", whiteSpacesBetweenTextLeft)
                + textToCenter
                + StringUtils.Methods.fillRight(" ", whiteSpacesBetweenTextRight);
        }

        public static void DrawRoulettePanel(string[] studentsSelected, int width = 80)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("╭" + StringUtils.Methods.fillRight("━", width, '━') + "╮");

            // Se evaluaran de 2 en 2
            for (int i = 0; i < studentsSelected.Length; i += 2)
            {
                string leftName = StudentManager.getName(studentsSelected[i]);
                string leftRole = "- Rol: " + StudentManager.getActiveRole(studentsSelected[i]);

                string rightName =
                    i + 1 < studentsSelected.Length
                        ? StudentManager.getName(studentsSelected[i + 1])
                        : "";
                string rightRole =
                    i + 1 < studentsSelected.Length
                        ? "- Rol: " + StudentManager.getActiveRole(studentsSelected[i + 1])
                        : "";

                string line1 = $"{leftName} | {rightName}";
                string line2 = $"{leftRole} | {rightRole}";

                int leftHalf = width / 2;

                WriteColorLines(
                    "┃" + StringUtils.Methods.Substring(CenterText(line1, width), 0, leftHalf),
                    ConsoleColor.Blue
                );
                WriteColorLines(
                    StringUtils.Methods.Substring(CenterText(line1, width), leftHalf) + "┃",
                    ConsoleColor.Red
                );

                WriteColorLines(
                    "\n┃" + StringUtils.Methods.Substring(CenterText(line2, width), 0, leftHalf),
                    ConsoleColor.Blue
                );
                WriteColorLines(
                    StringUtils.Methods.Substring(CenterText(line2, width), leftHalf) + "┃",
                    ConsoleColor.Red
                );

                // Espacio en caso de que sean 4 por ej
                if (studentsSelected.Length > 2)
                {
                    WriteColorLines(
                        StringUtils.Methods.fillRight("\n┃", width + 1),
                        ConsoleColor.Blue
                    );
                    WriteColorLines("┃", ConsoleColor.Red);
                }
            }

            Console.WriteLine("\n╰" + StringUtils.Methods.fillRight("━", width, '━') + "╯");
            Console.ResetColor();
        }

        public static void DrawRouletteAnim(
            string[] students,
            string role,
            ConsoleColor selectorColor
        )
        {
            if (students.Length <= 0)
                return;

            // Para alinear la ruleta
            Console.WriteLine("\n\n\n");

            int topNames = students.Length / 2;
            int separator = 4;
            string defaultSelector = $"{role}  ⮞  ";
            string selector = StringUtils.Methods.fillLeft(
                defaultSelector,
                27 + defaultSelector.Length
            );
            int pad = selector.Length + separator * topNames;

            for (int i = 0; i < topNames; i++)
            {
                string[] data = StringUtils.Methods.Split(students[i], ' ');
                string current = "";

                for (int j = 0; j < data.Length; j++)
                {
                    if (j == 2)
                    {
                        current += $"{data[j][0]}.";
                        break;
                    }

                    current += $"{data[j]} ";
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(StringUtils.Methods.fillLeft("", pad) + current);
                pad -= separator;
            }

            Console.ForegroundColor = selectorColor;
            Console.WriteLine(selector + students[topNames]);
            pad = selector.Length + separator;

            for (int i = topNames + 1; i < students.Length; i++)
            {
                string[] data = StringUtils.Methods.Split(students[i], ' ');
                string current = "";

                for (int j = 0; j < data.Length; j++)
                {
                    if (j == 2)
                    {
                        current += $"{data[j][0]}.";
                        break;
                    }

                    current += $"{data[j]} ";
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(StringUtils.Methods.fillLeft("", pad) + current);
                pad += separator;
            }

            //Console.ResetColor();
        }
    }
}
