using Config;
using FileUtils;

namespace RoleUtils
{
    public class RoleManager
    {
        public static string[] RolesList = LoadRoles();

        private static string[] LoadRoles()
        {
            if (!File.Exists(FileSettings.RolesListPath))
            {
                return LoadDefaultRoles();
            }

            string[] rolesList = TXT.Import(FileSettings.RolesListPath);

            if (rolesList.Length <= 0)
                return LoadDefaultRoles();

            return TXT.Import(FileSettings.RolesListPath);
        }

        private static string[] LoadDefaultRoles()
        {
            File.WriteAllLines(FileSettings.RolesListPath, RuleSettings.DefaultRoles);
            return RuleSettings.DefaultRoles;
        }

        public static void SaveChanges()
        {
            File.WriteAllLines(FileSettings.RolesListPath, RolesList);
        }

        public static void ResetRoles()
        {
            RolesList = RuleSettings.DefaultRoles;
            SaveChanges();
        }

        public static void AddNewRole(string role)
        {
            ArrayUtils.Methods.Push(ref RolesList, role);
            SaveChanges();
        }

        public static bool RemoveRole(string role)
        {
            if (RolesList.Length <= RuleSettings.MinRolesRequired)
                return false;

            RolesList = ArrayUtils.Methods.Filter(RolesList, r => r.ToLower() != role.ToLower());
            SaveChanges();

            if (
                !ArrayUtils.Methods.Includes(
                    ArrayUtils.Methods.Map(RolesList, r => r.ToLower()),
                    role.ToLower()
                )
            )
                return true;
            else
                return false;
        }

        public static bool EditRoleName(string roleToEdit, string newRoleName)
        {
            int roleToEditIndex = ArrayUtils.Methods.FindIndex(RolesList, roleToEdit);
            if (roleToEditIndex != -1)
            {
                RolesList[roleToEditIndex] = newRoleName;
                SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}
