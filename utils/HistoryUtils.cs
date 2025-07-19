using Config;
using MenuUtils;
using RoleUtils;
using SoundUtils;
using StudentUtils;

namespace HistoryUtils
{
    public class HistoryFunctions
    {
        public static int actualScoreFilter = -1;

        public static string[] scoreChoices = ["Mayor a", "Menor a", "Igual a", "Diferente a"];

        private static string[] DrawHistoryPanelRows()
        {
            string[] studentsLists = StudentManager.studentsList;
            string[] allRows = [];

            for (int s = 0; s < studentsLists.Length; s++)
            {
                string student = studentsLists[s];
                string[] studentData = StudentManager.parseData(student);

                if (studentData.Length < 7)
                    continue;

                string fullname = studentData[0];
                string name =
                    fullname.Length > 20
                        ? StringUtils.Methods.fillRight(
                            StringUtils.Methods.Substring(fullname, 0, 16) + "...",
                            22
                        )
                        : StringUtils.Methods.fillRight(fullname, 22);

                string attendance = bool.TryParse(studentData[1], out bool attendanceParsed)
                    ? (attendanceParsed ? "Presente" : "Ausente")
                    : "N/A";

                string attDateTime = StringUtils.Methods.fillRight("N/A", 9);
                if (long.TryParse(studentData[2], out long attRaw))
                {
                    attDateTime = StringUtils.Methods.Substring(
                        $"{DateTimeOffset.FromUnixTimeMilliseconds(attRaw).DateTime}",
                        0,
                        10
                    );
                    attDateTime = StringUtils.Methods.Replace(attDateTime, " ", "");
                }

                string[] roles = StringUtils.Methods.Split(studentData[3], ',');
                string[] participations = StringUtils.Methods.Split(studentData[4], ',');
                string[] scores = StringUtils.Methods.Split(studentData[5], ',');
                string[] rolesDate = StringUtils.Methods.Split(studentData[6], ',');

                for (int i = 0; i < roles.Length; i++)
                {
                    string role = StringUtils.Methods.fillRight(
                        StringUtils.Methods.IsNullOrWhiteSpace(roles[i]) ? "N/A" : roles[i],
                        28
                    );
                    string particip =
                        i < participations.Length
                        && !StringUtils.Methods.IsNullOrWhiteSpace(participations[i])
                            ? participations[i]
                            : "N/A";
                    string score =
                        i < scores.Length && !StringUtils.Methods.IsNullOrWhiteSpace(scores[i])
                            ? scores[i]
                            : "N/A";

                    string roleDate = "N/A";
                    if (i < rolesDate.Length && long.TryParse(rolesDate[i], out long roleDateRaw))
                        roleDate = StringUtils.Methods.Replace(
                            StringUtils.Methods.Substring(
                                $"{DateTimeOffset.FromUnixTimeMilliseconds(roleDateRaw).DateTime}",
                                0,
                                10
                            ),
                            " ",
                            ""
                        );

                    ArrayUtils.Methods.Push(
                        ref allRows,
                        $"│ {name} │ {StringUtils.Methods.fillRight(attendance, 10)} │  {attDateTime}   │ {role} │     {StringUtils.Methods.fillRight(particip, 6)} │ {StringUtils.Methods.fillRight(score, 5)} │   {StringUtils.Methods.fillRight(roleDate, 12)} │"
                    );

                    // Console.WriteLine(
                    //     $"│ {shouldShowName} │ {shouldShowAttendance} │  {shouldShowAttDate}   │ {role} │     {StringUtils.Methods.fillRight(particip, 6)} │ {StringUtils.Methods.fillRight(score, 5)} │   {StringUtils.Methods.fillRight(roleDate, 12)} │"
                    // );

                    // Juntar los mismos datos de estudiantes y sus respectivos roles
                    // if (i == roles.Length - 1 && s != studentsLists.Length - 1)
                    // {
                    //     ArrayUtils.Methods.Push(
                    //         ref allRows,
                    //         "├────────────────────────┼────────────┼──────────────┼──────────────────────────────┼────────────┼───────┼────────────────┤"
                    //     );
                    // }
                }
            }

            return allRows;
        }

        private static void DrawHistoryHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                "╭────────────────────────┬────────────┬──────────────┬──────────────────────────────┬────────────┬───────┬────────────────╮"
            );
            Console.WriteLine(
                "│ Nombre                 │ Asistencia │ Fecha Asist. │ Rol que desempeño            │ Particip.  │ Nota  │ Fecha de part. │"
            );
            Console.WriteLine(
                "├────────────────────────┼────────────┼──────────────┼──────────────────────────────┼────────────┼───────┼────────────────┤"
            );
        }

        private static void DrawHistoryFooter(int currentPage, int totalPages, string filters)
        {
            Console.WriteLine(
                "╰────────────────────────┴────────────┴──────────────┴──────────────────────────────┴────────────┴───────┴────────────────╯"
            );
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"Pagina {currentPage + 1}/{totalPages} - [←] Anterior | [→] Siguiente\n[ESC] Salir | F7: Nombre | F8: Rol | F9: Asistencia | F10: Puntuacion | F4: Limpiar filtros | F12: Resetear todo el historial"
            );
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(filters);
            Console.ResetColor();
        }

        public static string[] ApplyHistoryFilters(
            string[] allRows,
            string name,
            string role,
            string attendance,
            string score
        )
        {
            string[] filtered = [];

            foreach (string row in allRows)
            {
                string[] rowSplitted = ArrayUtils.Methods.Map(
                    ArrayUtils.Methods.Filter(
                        StringUtils.Methods.Split(row, '│'),
                        param => !StringUtils.Methods.IsNullOrWhiteSpace(param)
                    ),
                    param => param.Trim()
                );
                if (
                    !StringUtils.Methods.IsNullOrWhiteSpace(name)
                    && !StringUtils.Methods.Match(rowSplitted[0].ToLower(), name.ToLower(), 3)
                )
                    continue;

                if (
                    !StringUtils.Methods.IsNullOrWhiteSpace(role)
                    && !StringUtils.Methods.Contains(rowSplitted[3].ToLower(), role.ToLower())
                )
                    continue;

                if (!StringUtils.Methods.IsNullOrWhiteSpace(attendance))
                {
                    bool isPresent = StringUtils.Methods.Contains(rowSplitted[1], "Presente");
                    if (attendance.ToLower() == "ausente" && isPresent)
                        continue;
                    if (attendance.ToLower() == "presente" && !isPresent)
                        continue;
                }

                if (
                    !StringUtils.Methods.IsNullOrWhiteSpace(score)
                    && int.TryParse(score, out int scoreParsed)
                )
                {
                    if (!int.TryParse(rowSplitted[5], out int rowScore))
                        continue;

                    //["Mayor a", "Menor a", "Igual a", "Diferente a"];

                    if (actualScoreFilter == 0 && scoreParsed >= rowScore)
                        continue;
                    else if (actualScoreFilter == 1 && scoreParsed <= rowScore)
                        continue;
                    else if (actualScoreFilter == 2 && scoreParsed != rowScore)
                        continue;
                    else if (actualScoreFilter == 3 && scoreParsed == rowScore)
                        continue;
                }

                ArrayUtils.Methods.Push(ref filtered, row);
            }

            return filtered;
        }

        private static string AskFilter(string tipo, string? scoreChoice = null)
        {
            Console.Clear();
            if (tipo == "asistencia")
            {
                string[] choicesArr = ["Presente", "Ausente"];
                int choiceSelected = Menu.InteractiveMenu(
                    "Selecciona el tipo de asistencia para filtrar",
                    choicesArr
                );
                if (choiceSelected == -1)
                    return "";
                return choicesArr[choiceSelected].ToLower();
            }
            else if (tipo == "rol")
            {
                int choiceSelected = Menu.InteractiveMenu(
                    "Selecciona el tipo de rol para filtrar",
                    RoleManager.RolesList
                );
                if (choiceSelected == -1)
                    return "";
                return RoleManager.RolesList[choiceSelected].Trim();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    $"Filtrar por {tipo}{(scoreChoice != null ? $" {scoreChoice}" : "")}:"
                );
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "(Escribe -1 para cancelar o presiona Enter sin escribir nada)\n"
                );
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"{tipo} > ");
                Console.ResetColor();
                string val = Console.ReadLine()!.Trim();
                if (val == "-1")
                    return "";
                else
                    return val;
            }
        }

        public static void ShowHistoryPages()
        {
            string[] allRows = DrawHistoryPanelRows();
            string filterName = "";
            string filterRole = "";
            string filterAttendance = "";
            string filterScore = "";

            int currentPage = 0;

            while (true)
            {
                string[] filteredRows = ApplyHistoryFilters(
                    allRows,
                    filterName,
                    filterRole,
                    filterAttendance,
                    filterScore
                );

                int rowsToShow = 6;
                int totalPages = filteredRows.Length / rowsToShow;
                if (filteredRows.Length % rowsToShow != 0)
                    totalPages++; // Si hay rows de sobra, agregar una pag extra

                while (true)
                {
                    Console.Clear();
                    DrawHistoryHeader();

                    int startRow = currentPage * rowsToShow;
                    int endRow = startRow + rowsToShow;

                    // Por si acaso xd
                    if (endRow > filteredRows.Length)
                        endRow = filteredRows.Length;

                    for (int i = startRow; i < endRow; i++)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(filteredRows[i]);

                        // Solo imprime si no es la ultima linea
                        if (i < endRow - 1 && i < filteredRows.Length - 1)
                        {
                            Console.WriteLine(
                                "├────────────────────────┼────────────┼──────────────┼──────────────────────────────┼────────────┼───────┼────────────────┤"
                            );
                        }
                    }

                    string actualFilters =
                        $"\nNombre: {(!StringUtils.Methods.IsNullOrWhiteSpace(filterName) ? filterName : "N/A")}, Roles: {(!StringUtils.Methods.IsNullOrWhiteSpace(filterRole) ? filterRole : "N/A")}, Asistencia: {(!StringUtils.Methods.IsNullOrWhiteSpace(filterAttendance) ? filterAttendance : "N/A")}, Puntuacion:{(actualScoreFilter != -1 ? $" {scoreChoices[actualScoreFilter]}" : "")} {(!StringUtils.Methods.IsNullOrWhiteSpace(filterScore) ? filterScore : "N/A")}";
                    DrawHistoryFooter(currentPage, totalPages, actualFilters);

                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.RightArrow && currentPage < totalPages - 1)
                    {
                        Sound.LoadAudio(SoundSettings.ChoicesAudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.3f);
                        currentPage++;
                    }
                    else if (key == ConsoleKey.LeftArrow && currentPage > 0)
                    {
                        Sound.LoadAudio(SoundSettings.ChoicesAudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.3f);
                        currentPage--;
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        Sound.LoadAudio(SoundSettings.BackAudioPath);
                        Sound.PlayAudio();
                        Sound.SetVolume(0.3f);

                        return;
                    }
                    else if (key == ConsoleKey.F7)
                    {
                        currentPage = 0;
                        filterName = AskFilter("nombre");
                    }
                    else if (key == ConsoleKey.F8)
                    {
                        currentPage = 0;
                        filterRole = AskFilter("rol");
                    }
                    else if (key == ConsoleKey.F9)
                    {
                        currentPage = 0;
                        filterAttendance = AskFilter("asistencia");
                    }
                    else if (key == ConsoleKey.F10)
                    {
                        int scoreChoice = Menu.InteractiveMenu(
                            "Selecciona como deseas filtrar la puntuacion",
                            scoreChoices
                        );

                        if (scoreChoice != -1)
                        {
                            currentPage = 0;
                            actualScoreFilter = scoreChoice;
                            filterScore = AskFilter("puntuacion", scoreChoices[scoreChoice]);
                        }
                    }
                    else if (key == ConsoleKey.F12)
                    {
                        int confirmChoice = Menu.InteractiveMenu(
                            "Estas seguro que deseas resetear todo el historial?\nEsta accion no se podra deshacer, piensalo bien.",
                            ["No, deseo retornar", "Si, estoy totalmente seguro"]
                        );

                        if (confirmChoice == 1)
                        {
                            StudentManager.resetAllStudentsData();

                            Console.Clear();
                            Console.ForegroundColor = TextColor.Success;
                            Console.WriteLine(
                                "Se reseteado el historial de todos los estudiantes exitosamente!"
                            );
                            Console.ForegroundColor = TextColor.Warning;
                            Console.WriteLine("OJO: Esta accion ya es imposible de deshacer.");
                            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                            Console.ReadKey(true);

                            // Ejecutar nuevamente para dibujar los rows actualizados
                            ShowHistoryPages();
                            // Retornar para evitar tener leaks de ejecuciones
                            return;
                        }
                    }
                    else if (key == ConsoleKey.F4)
                    {
                        currentPage = 0;
                        filterName = "";
                        filterRole = "";
                        filterAttendance = "";
                        filterScore = "";
                    }

                    break;
                }
            }
        }
    }
}
