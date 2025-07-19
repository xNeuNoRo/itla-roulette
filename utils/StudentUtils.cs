using Config;
using FileUtils;
using RoleUtils;

// TODO:
// -- Implementar asistencia con la logica de la ruleta

namespace StudentUtils
{
    public class StudentManager
    {
        public static string[] studentsList = LoadStudents();

        private static string[] LoadStudents()
        {
            if (!File.Exists(FileSettings.SelectionHistoryPath))
            {
                string[] studentsParsed = ArrayUtils.Methods.Map(
                    TXT.Import(FileSettings.PlainTextPath),
                    student => StringUtils.Methods.Join(parseData(student), "|")
                );
                File.WriteAllLines(FileSettings.SelectionHistoryPath, studentsParsed);
                return studentsParsed;
            }

            string[] studentsArray = TXT.Import(FileSettings.SelectionHistoryPath);

            if (FileSettings.DynamicImportStudents || studentsArray.Length <= 0)
            {
                string[] studentsNamesList = ArrayUtils.Methods.Map(
                    studentsArray,
                    str => parseData(str)[0].ToLower()
                );

                string[] TXTcontentFiltered = ArrayUtils.Methods.Filter(
                    TXT.Import(FileSettings.PlainTextPath),
                    str => !ArrayUtils.Methods.Includes(studentsNamesList, str.ToLower())
                );

                if (TXTcontentFiltered.Length >= 1)
                {
                    string[] studentsParsed = ArrayUtils.Methods.Map(
                        TXTcontentFiltered,
                        student => StringUtils.Methods.Join(parseData(student), "|")
                    );

                    studentsArray = ArrayUtils.Methods.Concat(studentsArray, studentsParsed);
                    File.WriteAllLines(FileSettings.SelectionHistoryPath, studentsArray);
                }
            }

            return studentsArray;
        }

        public static void SaveChanges()
        {
            File.WriteAllLines(FileSettings.SelectionHistoryPath, studentsList);
        }

        public static void updateStudentsList(string originalStudentData, string newStudentData)
        {
            int studentIndex = ArrayUtils.Methods.FindIndex(studentsList, originalStudentData);
            if (studentIndex != -1)
                studentsList[studentIndex] = newStudentData;

            SaveChanges();
        }

        public static string[] parseData(string student)
        {
            //nombre|asistencia|fecha_asistencia|roles|participacion_en_roles|puntuacion_de_participacion|fecha_de_participacion
            //Angel|true|1720890000000|Desarrollador en Vivo,Facilitador de Ejercicio|1,1|10,10|1720973345000,1720890000000
            string[] data = StringUtils.Methods.Split(student, '|');
            return data.Length == 7 ? data : new string[] { data[0], "", "", "", "", "", "" };
        }

        public static string getName(string student)
        {
            return parseData(student)[0];
        }

        public static string getStudentByName(string studentName)
        {
            string[] studentsNameList = ArrayUtils.Methods.Map(
                studentsList,
                student => parseData(student)[0]
            );
            int studentIndex = ArrayUtils.Methods.FindIndex(studentsNameList, studentName);
            return studentsList[studentIndex];
        }

        public static void editName(ref string student, string newName)
        {
            string originalData = student;
            string[] studentData = parseData(student);

            if (newName.ToLower() == studentData[0].ToLower())
                return;

            studentData[0] = newName;
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static void removeStudent(string studentName)
        {
            studentsList = ArrayUtils.Methods.Filter(
                studentsList,
                student => parseData(student)[0] != studentName
            );
            SaveChanges();
        }

        public static void resetStudentData(string studentName)
        {
            string student = getStudentByName(studentName);
            string[] studentData = parseData(student);

            // desde 1 para ignorar el nombre
            for (int i = 1; i < studentData.Length; i++)
            {
                studentData[i] = "";
            }

            string resettedStudent = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(student, resettedStudent);
        }

        public static void resetRoleStudentData(string studentName, string role)
        {
            string student = getStudentByName(studentName);
            string originalData = student;
            string[] studentData = parseData(student);
            string studentRoles = studentData[3];
            string studentRolesParticip = studentData[4];
            string studentRolesScore = studentData[5];
            string studentRolesDate = studentData[6];
            int roleIndex = getRoleIndex(student, role);

            if (roleIndex == -1)
                return;

            string[] studentRolesParsed =
                studentRoles.Length <= 0 ? [] : StringUtils.Methods.Split(studentRoles, ',');
            studentRolesParsed = ArrayUtils.Methods.Filter(studentRolesParsed, r => r != role);
            studentData[3] = StringUtils.Methods.Join(studentRolesParsed, ",");

            string[] studentRolesPartParsed =
                studentRolesParticip.Length <= 0
                    ? []
                    : StringUtils.Methods.Split(studentRolesParticip, ',');
            studentRolesPartParsed[roleIndex] = "";
            studentRolesPartParsed = ArrayUtils.Methods.Filter(
                studentRolesPartParsed,
                p => !StringUtils.Methods.IsNullOrWhiteSpace(p)
            );
            studentData[4] = StringUtils.Methods.Join(studentRolesPartParsed, ",");

            string[] studentRoleScoreParsed =
                studentRolesScore.Length <= 0
                    ? []
                    : StringUtils.Methods.Split(studentRolesScore, ',');
            studentRoleScoreParsed[roleIndex] = "";
            studentRoleScoreParsed = ArrayUtils.Methods.Filter(
                studentRoleScoreParsed,
                s => !StringUtils.Methods.IsNullOrWhiteSpace(s)
            );
            studentData[5] = StringUtils.Methods.Join(studentRoleScoreParsed, ",");

            string[] studentRoleDateParsed =
                studentRolesDate.Length <= 0
                    ? []
                    : StringUtils.Methods.Split(studentRolesDate, ',');
            studentRoleDateParsed[roleIndex] = "";
            studentRoleDateParsed = ArrayUtils.Methods.Filter(
                studentRoleDateParsed,
                d => !StringUtils.Methods.IsNullOrWhiteSpace(d)
            );
            studentData[6] = StringUtils.Methods.Join(studentRoleDateParsed, ",");

            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static void deleteAllRoleData(string role)
        {
            string[] studentsWithTheRole = ArrayUtils.Methods.Map(ArrayUtils.Methods.Filter(
                studentsList,
                student => hasRole(student, role) && !isActiveRole(student, role)
            ), student=>getName(student));

            for (int i = 0; i < studentsWithTheRole.Length; i++)
            {
                resetRoleStudentData(studentsWithTheRole[i], role);
            }
        }

        public static void editAllRoleNameData(string role, string newRoleName)
        {
            string[] studentsWithTheRole = ArrayUtils.Methods.Filter(
                studentsList,
                student => hasRole(student, role) && !isActiveRole(student, role)
            );

            for (int i = 0; i < studentsWithTheRole.Length; i++)
            {
                string student = studentsWithTheRole[i];
                string originalData = student;
                string[] studentData = parseData(student);
                string studentRoles = studentData[3];
                int roleIndex = getRoleIndex(student, role);
                string[] studentRolesParsed =
                    studentRoles.Length <= 0 ? [] : StringUtils.Methods.Split(studentRoles, ',');

                studentRolesParsed[roleIndex] = newRoleName;
                studentData[3] = StringUtils.Methods.Join(studentRolesParsed, ",");
                student = StringUtils.Methods.Join(studentData, "|");
                updateStudentsList(originalData, student);
            }
        }

        public static void resetAllStudentsData()
        {
            for (int i = 0; i < studentsList.Length; i++)
            {
                string[] studentData = parseData(studentsList[i]);
                for (int j = 1; j < studentData.Length; j++)
                {
                    studentData[j] = "";
                }
                studentsList[i] = StringUtils.Methods.Join(studentData, "|");
            }
            SaveChanges();
        }

        public static void TakeAttendance(ref string student, bool attendance = true)
        {
            string originalData = student;
            string[] studentData = parseData(student);
            DateTime actualDate = DateTime.Now;
            long actualDateToMilliseconds = new DateTimeOffset(actualDate).ToUnixTimeMilliseconds();

            studentData[1] = attendance ? "true" : "false";
            studentData[2] = $"{actualDateToMilliseconds}";

            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static bool CheckAttendance()
        {
            for (int i = 0; i < studentsList.Length; i++)
            {
                string student = studentsList[i];
                string[] studentData = parseData(student);

                if (studentData[2].Length <= 0)
                    return false;

                if (!long.TryParse(studentData[2], out long attendanceDateRaw))
                    return false;

                DateTime attendanceDateParsed = DateTimeOffset
                    .FromUnixTimeMilliseconds(attendanceDateRaw)
                    .LocalDateTime.Date;
                // Output: 15/07/2025 00:00:00

                // Ej: 17/7/2025 != 16/7/2025
                if (attendanceDateParsed != DateTime.Now.Date)
                    return false;
            }

            return true;
        }

        public static bool isPresent(string student)
        {
            string[] studentData = parseData(student);
            if (!bool.TryParse(studentData[1], out bool isPresent))
                return false;

            return isPresent;
        }

        public static void clearRoles(ref string student)
        {
            string originalData = student;
            string[] studentData = parseData(student);
            studentData[3] = "";
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static int getActiveRoleCount(string role)
        {
            int count = 0;
            for (int i = 0; i < studentsList.Length; i++)
            {
                string studentSelected = studentsList[i];
                string studentRoleActive = getActiveRole(studentSelected);
                if (studentRoleActive == role)
                    count++;
            }
            return count;
        }

        public static bool hasRole(string student, string role)
        {
            string[] studentData = parseData(student);
            string studentRoles = studentData[3];
            string[] studentRolesParsed =
                studentRoles.Length <= 0 ? [] : StringUtils.Methods.Split(studentRoles, ',');

            return ArrayUtils.Methods.Includes(
                ArrayUtils.Methods.Map(
                    studentRolesParsed,
                    r => StringUtils.Methods.Replace(r, ":1", "").ToLower()
                ),
                role.ToLower()
            );
        }

        public static bool hasActiveRole(string student)
        {
            return getActiveRole(student).Length > 0 ? true : false;
        }

        public static void removeRole(ref string student, string role)
        {
            string originalData = student;
            string[] studentData = parseData(student);
            string studentRoles = studentData[3];
            string[] studentRolesParsed =
                studentRoles.Length <= 0 ? [] : StringUtils.Methods.Split(studentRoles, ',');

            if (
                !ArrayUtils.Methods.Includes(
                    ArrayUtils.Methods.Map(studentRolesParsed, r => r.ToLower()),
                    role.ToLower()
                )
            )
                return;

            string[] rolesFiltered = ArrayUtils.Methods.Filter(
                studentRolesParsed,
                r => r.ToLower() != role.ToLower()
            );

            studentData[3] = StringUtils.Methods.Join(rolesFiltered, ",");
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static void removeActiveRole(ref string student)
        {
            string originalData = student;
            string activeRole = getActiveRole(student);
            if (activeRole.Length <= 0)
                return;

            removeRole(ref student, activeRole + ":1");
            updateStudentsList(originalData, student);
        }

        public static void addRole(ref string student, string role)
        {
            string originalData = student;
            string[] studentData = parseData(student);
            string studentRoles = studentData[3];
            string[] studentRolesParsed =
                studentRoles.Length <= 0 ? [] : StringUtils.Methods.Split(studentRoles, ',');

            if (ArrayUtils.Methods.Includes(studentRolesParsed, role))
                return;

            ArrayUtils.Methods.Push(ref studentRolesParsed, role);
            studentData[3] = StringUtils.Methods.Join(studentRolesParsed, ",");
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static string getActiveRole(string student)
        {
            string[] studentData = parseData(student);
            string studentRoles = studentData[3];
            string[] studentRolesParsed =
                studentRoles.Length <= 0 ? [] : StringUtils.Methods.Split(studentRoles, ',');

            string[] activeRole = ArrayUtils.Methods.Filter(
                studentRolesParsed,
                r => StringUtils.Methods.EndsWith(r, ":1")
            );

            if (activeRole.Length <= 0)
                return "";
            else
                return StringUtils.Methods.Substring(activeRole[0], 0, activeRole[0].Length - 2);
        }

        public static int getRoleIndex(string student, string role)
        {
            string[] studentData = parseData(student);
            string studentRoles = studentData[3];
            if (studentRoles.Length <= 0)
                return -1;

            if (getActiveRole(student).ToLower() == role.ToLower())
                role = $"{role}:1";

            string[] studentRolesParsed = StringUtils.Methods.Split(studentRoles, ',');

            return ArrayUtils.Methods.FindIndex(studentRolesParsed, role);
        }

        public static bool isActiveRole(string student, string role)
        {
            string active = getActiveRole(student);
            return active.ToLower() == role.ToLower();
        }

        public static void enableActiveRole(ref string student, string roleToActivate)
        {
            string originalData = student;

            if (getActiveRole(student).ToLower() == roleToActivate.ToLower())
                return;

            disableActiveRole(ref student);

            int roleToActivateIndex = getRoleIndex(student, roleToActivate);
            if (roleToActivateIndex == -1)
                return;

            string[] studentData = parseData(student);
            string studentRoles = studentData[3];
            string[] studentRolesParsed =
                studentRoles.Length <= 0 ? [] : StringUtils.Methods.Split(studentRoles, ',');

            studentRolesParsed[roleToActivateIndex] += ":1";

            studentData[3] = StringUtils.Methods.Join(studentRolesParsed, ",");
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static void disableActiveRole(ref string student)
        {
            string originalData = student;
            string activeRole = getActiveRole(student);
            if (activeRole.Length <= 0)
                return;

            int activeRoleIndex = getRoleIndex(student, activeRole + ":1");
            if (activeRoleIndex == -1)
                return;

            string[] studentData = parseData(student);
            string studentRoles = studentData[3];
            string[] studentRolesParsed =
                studentRoles.Length <= 0 ? [] : StringUtils.Methods.Split(studentRoles, ',');

            studentRolesParsed[activeRoleIndex] = activeRole;

            studentData[3] = StringUtils.Methods.Join(studentRolesParsed, ",");
            student = StringUtils.Methods.Join(studentData, "|");
            countParticipation(ref student, activeRole);
            updateStudentsList(originalData, student);
        }

        public static void disableAllActiveRoles()
        {
            for (int i = 0; i < studentsList.Length; i++)
                disableActiveRole(ref studentsList[i]);
        }

        public static int getRoleParticipations(string student, string role)
        {
            int roleParticipationIndex = getRoleIndex(student, role);
            if (roleParticipationIndex == -1)
                return 0;

            string[] studentData = parseData(student);
            string studentRolesParticipations = studentData[4];
            if (studentRolesParticipations.Length <= 0)
                return 0;

            string[] studentRolesParticipationsParsed = StringUtils.Methods.Split(
                studentRolesParticipations,
                ','
            );

            return int.Parse(studentRolesParticipationsParsed[roleParticipationIndex]);
        }

        public static void countParticipation(ref string student, string role)
        {
            string originalData = student;
            int roleToCountParticipationIndex = getRoleIndex(student, role);
            if (roleToCountParticipationIndex == -1)
                return;

            string[] studentData = parseData(student);
            string studentRolesParticipations = studentData[4];
            string[] studentRolesParticipationsParsed =
                studentRolesParticipations.Length <= 0
                    ? []
                    : StringUtils.Methods.Split(studentRolesParticipations, ',');

            if (
                studentRolesParticipationsParsed.Length == 0
                || studentRolesParticipationsParsed.Length == roleToCountParticipationIndex
            )
            {
                ArrayUtils.Methods.Push(ref studentRolesParticipationsParsed, "1");
            }
            else
            {
                string actualParticipations = studentRolesParticipationsParsed[
                    roleToCountParticipationIndex
                ];

                int newParticipations = Convert.ToInt16(actualParticipations) + 1;

                studentRolesParticipationsParsed[roleToCountParticipationIndex] =
                    $"{newParticipations}";
            }

            studentData[4] = StringUtils.Methods.Join(studentRolesParticipationsParsed, ",");
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static void editParticipation(ref string student, string role, int newParticipations)
        {
            string originalData = student;
            int roleToEditParticipationIndex = getRoleIndex(student, role);
            if (roleToEditParticipationIndex == -1)
                return;

            string[] studentData = parseData(student);
            string studentRolesParticipations = studentData[4];
            string[] studentRolesParticipationsParsed =
                studentRolesParticipations.Length <= 0
                    ? []
                    : StringUtils.Methods.Split(studentRolesParticipations, ',');

            if (studentRolesParticipationsParsed.Length == 0)
            {
                return;
            }
            else
            {
                studentRolesParticipationsParsed[roleToEditParticipationIndex] =
                    $"{newParticipations}";
            }

            studentData[4] = StringUtils.Methods.Join(studentRolesParticipationsParsed, ",");
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static void SetParticipationScore(ref string student, string role, int pScore)
        {
            string originalData = student;

            int roleToScoreIndex = getRoleIndex(student, role);
            if (roleToScoreIndex == -1)
                return;

            string[] studentData = parseData(student);
            string studentRolesScore = studentData[5];
            string[] studentRolesScoreParsed =
                studentRolesScore.Length <= 0
                    ? []
                    : StringUtils.Methods.Split(studentRolesScore, ',');

            if (
                studentRolesScoreParsed.Length == 0
                || studentRolesScoreParsed.Length == roleToScoreIndex
            )
            {
                ArrayUtils.Methods.Push(ref studentRolesScoreParsed, $"{pScore}");
            }
            else
            {
                studentRolesScoreParsed[roleToScoreIndex] = $"{pScore}";
            }

            studentData[5] = StringUtils.Methods.Join(studentRolesScoreParsed, ",");
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static void SetLastParticipationDate(ref string student, string role)
        {
            string originalData = student;

            int roleToSetDateIndex = getRoleIndex(student, role);
            if (roleToSetDateIndex == -1)
                return;

            string[] studentData = parseData(student);
            string studentRolesDate = studentData[6];
            string[] studentRolesDateParsed =
                studentRolesDate.Length <= 0
                    ? []
                    : StringUtils.Methods.Split(studentRolesDate, ',');

            DateTime actualDate = DateTime.Now;
            long actualDateToMilliseconds = new DateTimeOffset(actualDate).ToUnixTimeMilliseconds();

            if (
                studentRolesDateParsed.Length == 0
                || studentRolesDateParsed.Length == roleToSetDateIndex
            )
            {
                ArrayUtils.Methods.Push(ref studentRolesDateParsed, $"{actualDateToMilliseconds}");
            }
            else
            {
                studentRolesDateParsed[roleToSetDateIndex] = $"{actualDateToMilliseconds}";
            }

            studentData[6] = StringUtils.Methods.Join(studentRolesDateParsed, ",");
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }

        public static void removeParticipationDate(ref string student, string role)
        {
            string originalData = student;

            int roleToRemoveDateIndex = getRoleIndex(student, role);
            if (roleToRemoveDateIndex == -1)
                return;

            string[] studentData = parseData(student);
            string studentRolesDate = studentData[6];
            string[] studentRolesDateParsed =
                studentRolesDate.Length <= 0
                    ? []
                    : StringUtils.Methods.Split(studentRolesDate, ',');

            if (studentRolesDateParsed.Length > 0)
                studentRolesDateParsed[roleToRemoveDateIndex] = "";

            studentData[6] = StringUtils.Methods.Join(studentRolesDateParsed, ",");
            student = StringUtils.Methods.Join(studentData, "|");
            updateStudentsList(originalData, student);
        }
    }
}
