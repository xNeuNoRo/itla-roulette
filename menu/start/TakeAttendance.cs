using Config;
using MenuUtils;
using StudentUtils;

public class TakeAttendance
{
    public static void Execute(string[]? absentStudents = null)
    {
        if (absentStudents != null && absentStudents.Length > 0)
        {
            for (int i = 0; i < absentStudents.Length; i++)
            {
                Console.ForegroundColor = TextColor.Attendance;
                int attendanceChoiceSelected = Menu.InteractiveMenu(
                    $"Se encuentra presente {StudentManager.getName(absentStudents[i])}?",
                    ["Si, se encuentra presente.", "No, no se encuentra presente."]
                );
                if (attendanceChoiceSelected == -1)
                    return;
                StudentManager.TakeAttendance(
                    ref absentStudents[i],
                    attendanceChoiceSelected == 0 ? true : false
                );
            }

            string[] absentStudentsNew = ArrayUtils.Methods.Filter(
                StudentManager.studentsList,
                student => !StudentManager.isPresent(student)
            );

            Console.ForegroundColor =
                absentStudentsNew.Length > 0 ? TextColor.Warning : TextColor.Success;
            Console.WriteLine(
                $"Se encuentran presentes {StudentManager.studentsList.Length - absentStudentsNew.Length} de {StudentManager.studentsList.Length} estudiantes el dia de hoy!"
            );
            Console.ForegroundColor = TextColor.Warning;
            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey(true);
            return;
        }

        string[] studentsArray = StudentManager.studentsList;

        for (int i = 0; i < studentsArray.Length; i++)
        {
            Console.ForegroundColor = TextColor.Attendance;
            int attendanceChoiceSelected = Menu.InteractiveMenu(
                $"Se encuentra presente {StudentManager.getName(studentsArray[i])}?",
                ["Si, se encuentra presente.", "No, no se encuentra presente."]
            );
            if (attendanceChoiceSelected == -1)
                return;
            StudentManager.TakeAttendance(
                ref studentsArray[i],
                attendanceChoiceSelected == 0 ? true : false
            );
        }

        Console.ResetColor();

        string[] absentStudentsArray = ArrayUtils.Methods.Filter(
            StudentManager.studentsList,
            student => !StudentManager.isPresent(student)
        );

        if (absentStudentsArray.Length > 0)
        {
            int confirmChoiceSelected = Menu.InteractiveMenu(
                "Deseas repetir el pase de lista para las personas ausentes?",
                ["No, estoy totalmente seguro.", "Si, deseo pasar lista nuevamente."]
            );

            if (confirmChoiceSelected == 1)
            {
                Execute(absentStudentsArray);
            }
        }
        else
        {
            Console.ForegroundColor = TextColor.Success;
            Console.WriteLine(
                $"Se encuentran presentes {StudentManager.studentsList.Length} de {StudentManager.studentsList.Length} estudiantes el dia de hoy!"
            );
            Console.ForegroundColor = TextColor.Warning;
            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}
