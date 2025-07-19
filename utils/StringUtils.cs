namespace StringUtils
{
    // TODO:
    // --Trim
    public class Methods
    {
        public static string[] Split(string stringToSplit, char separator)
        {
            // Declaramos el acumulador del string, el array que guardara todo y el index inicial
            string stringAccumulator = "";
            string[] stringArr = [];

            // Comprobamos todos los caracteres del string uno por uno
            for (int i = 0; i < stringToSplit.Length; i++)
            {
                char actualChar = stringToSplit[i];
                // Si el caracter es igual al separador,
                // guardamos todo el string en el array, reseteamos el acumulador y aumentamos el index del array
                if (separator == actualChar)
                {
                    ArrayUtils.Methods.Push(ref stringArr, stringAccumulator);
                    stringAccumulator = "";
                }
                // En el caso contrario seguimos almacenando los caracteres en el acumulador
                else
                {
                    stringAccumulator += actualChar;
                }
            }

            // Agrego el ultimo string acumulado
            ArrayUtils.Methods.Push(ref stringArr, stringAccumulator);

            return stringArr;
        }

        public static string Join<T>(T[] arr, string separator)
        {
            string strAcc = "";
            for (int i = 0; i < arr.Length; i++)
            {
                strAcc += i < arr.Length - 1 ? arr[i] + separator : arr[i];
            }
            return strAcc;
        }

        public static bool IsNullOrWhiteSpace(string stringToFilter)
        {
            if (stringToFilter is null)
                return true;

            if (stringToFilter.Length == 0)
                return true;

            for (int i = 0; i < stringToFilter.Length; i++)
            {
                char strToFilterChar = stringToFilter[i];

                if (
                    strToFilterChar != ' ' // Espacio en blanco
                    && strToFilterChar != '\t' // tabulacion
                    && strToFilterChar != '\n' // salto de linea
                    && strToFilterChar != '\r' // retorno al inicio de la linea
                )
                    return false;
            }

            return true;
        }

        public static bool EndsWith(string strInput, string expectedEnd)
        {
            if (expectedEnd.Length > strInput.Length)
                return false;

            for (int i = 0; i < expectedEnd.Length; i++)
            {
                int expectedEndPos = strInput.Length - expectedEnd.Length;
                char expectedEndChar = strInput[expectedEndPos + i];
                if (expectedEndChar != expectedEnd[i])
                    return false;
            }

            return true;
        }

        public static bool StartsWith(string strInput, string expectedStart)
        {
            if (expectedStart.Length > strInput.Length)
                return false;

            for (int i = 0; i < expectedStart.Length; i++)
            {
                if (strInput[i] != expectedStart[i])
                    return false;
            }

            return true;
        }

        public static string fillLeft(string strInput, int spacesToFill, char charToAdd = ' ')
        {
            if (strInput.Length >= spacesToFill)
                return strInput;

            int realSpacesToFill = spacesToFill - strInput.Length;

            string strAcc = "";

            for (int i = realSpacesToFill; i != 0; i--)
            {
                strAcc += charToAdd;
            }
            strAcc += strInput;

            return strAcc;
        }

        public static string fillRight(string strInput, int spacesToFill, char charToAdd = ' ')
        {
            if (strInput.Length >= spacesToFill)
                return strInput;

            int realSpacesToFill = spacesToFill - strInput.Length;

            string strAcc = strInput;

            for (int i = realSpacesToFill; i != 0; i--)
            {
                strAcc += charToAdd;
            }

            return strAcc;
        }

        public static string Substring(
            string strInput,
            int startIndex,
            int? expectedEndIndex = null
        )
        {
            if (IsNullOrWhiteSpace(strInput))
                return "";

            int endIndex = expectedEndIndex ?? strInput.Length;

            if (startIndex < 0 || endIndex > strInput.Length || startIndex >= endIndex)
                return "";

            int expectedLength = endIndex - startIndex;
            char[] result = new char[expectedLength];

            for (int i = 0; i < expectedLength; i++)
            {
                result[i] = strInput[startIndex + i];
            }

            return new string(result);
        }

        public static bool Match(string strSource, string ValToCheck, int minCoincidences = 5)
        {
            if (IsNullOrWhiteSpace(strSource) || IsNullOrWhiteSpace(ValToCheck))
                return false;

            if (ValToCheck.Length > strSource.Length)
                return false;

            if (minCoincidences > ValToCheck.Length)
                minCoincidences = ValToCheck.Length;

            int limit = strSource.Length - minCoincidences;

            for (int i = 0; i <= limit; i++)
            {
                if (
                    Substring(strSource.ToLower(), i, i + minCoincidences)
                    == Substring(ValToCheck.ToLower(), 0, minCoincidences)
                )
                    return true;
            }

            return false;
        }

        public static bool Contains(string strSource, string ValToCheck)
        {
            if (strSource == null || ValToCheck == null)
                return false;
            else if (ValToCheck.Length > strSource.Length)
                return false;

            int limit = strSource.Length - ValToCheck.Length;

            for (int i = 0; i <= limit; i++)
            {
                if (Substring(strSource, i, i + ValToCheck.Length) == ValToCheck)
                    return true;
            }

            return false;
        }

        public static string Replace(string strInput, string strToReplace, string strNewReplace)
        {
            if (IsNullOrWhiteSpace(strInput))
                return strInput;

            string result = "";

            for (int i = 0; i < strInput.Length; )
            {
                if (
                    i + strToReplace.Length <= strInput.Length
                    && Substring(strInput, i, i + strToReplace.Length) == strToReplace
                )
                {
                    result += strNewReplace;
                    i += strToReplace.Length;
                }
                else if (i + strToReplace.Length > strInput.Length)
                {
                    // Agregar el resto del string
                    result += Substring(strInput, i);
                    break;
                }
                else
                {
                    result += strInput[i];
                    i++; // avanzar normalmente
                }
            }

            return result;
        }
    }
}
