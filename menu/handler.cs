using MenuUtils;
using StudentUtils;

namespace Handlers
{
    public class MenuHandler
    {
        public static readonly string[] StartMenuOptions =
        [
            "Iniciar seleccion aleatoria",
            "Tomar asistencia del dia",
            "Ver historial de selecciones",
            "Panel de control",
        ];

        public static readonly string[] ControlPanelOptions =
        [
            "Administrar Estudiantes",
            "Administrar Roles",
        ];

        public static bool StartMenu(int choiceSelected)
        {
            if (choiceSelected == 0)
            {
                int confirmChoice = Menu.InteractiveMenu(
                    "Estas seguro que deseas iniciar una seleccion aleatoria?\nNo podras salir de esta hasta que los\nestudiantes seleccionados hayan participado\ny cada uno de estos haya sido puntuado debidamente",
                    ["Si, estoy totalmente seguro", "No, deseo retornar"]
                );

                if (confirmChoice == 0)
                    StartRoulette.Execute();
            }
            else if (choiceSelected == 1)
            {
                string[] absentStudentsArray = ArrayUtils.Methods.Filter(
                    StudentManager.studentsList,
                    student => !StudentManager.isPresent(student)
                );

                if (absentStudentsArray.Length == 0)
                {
                    int confirmChoice = Menu.InteractiveMenu(
                        "Todos los estudiantes se encuentran presentes!\nEstas seguro que deseas pasar la lista nuevamente?",
                        ["No, deseo retornar", "Si, estoy totalmente seguro"]
                    );

                    if (confirmChoice == 1)
                        TakeAttendance.Execute(absentStudentsArray);
                }
                else
                {
                    if (absentStudentsArray.Length != StudentManager.studentsList.Length)
                    {
                        int confirmChoice = Menu.InteractiveMenu(
                            $"Hay un total de {absentStudentsArray.Length} {(absentStudentsArray.Length == 1 ? "estudiante ausente" : "estudiantes ausentes")}, deseas pasar lista solamente de los ausentes?",
                            [
                                "Si, deseo pasar lista solo de los ausentes",
                                "No, deseo pasar lista con todos",
                            ]
                        );

                        if (confirmChoice == 0)
                            TakeAttendance.Execute(absentStudentsArray);
                        else if (confirmChoice == 1)
                            TakeAttendance.Execute();
                    }
                    else
                        TakeAttendance.Execute();
                }
            }
            else if (choiceSelected == 2)
            {
                StudentsRecord.Execute();
            }
            else if (choiceSelected == 3)
            {
                ControlPanel.Execute();
            }

            return false;
        }
    }
}
