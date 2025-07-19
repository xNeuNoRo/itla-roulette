namespace ArrayUtils
{
    public class Methods
    {
        public static T[] Copy<T>(T[] originalArr, T[] copyArr)
        {
            for (int i = 0; i < originalArr.Length; i++)
            {
                copyArr[i] = originalArr[i];
            }

            return copyArr;
        }

        public static T[] Concat<T>(T[] arr1, T[] arr2)
        {
            T[] arrResult = new T[arr1.Length + arr2.Length];
            arrResult = Copy(arr1, arrResult);

            for (int i = 0; i < arr2.Length; i++)
                arrResult[arr1.Length + i] = arr2[i];

            return arrResult;
        }

        public static bool Includes<T>(T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (object.Equals(arr[i], value))
                    return true;
            }
            return false;
        }

        public static int FindIndex<T>(T[] arr, T valueToSearch)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (object.Equals(arr[i], valueToSearch))
                    return i;
            }

            return -1;
        }

        public static string Join(string[] arr, string separator)
        {
            string strAcc = "";

            for (int i = 0; i < arr.Length; i++)
            {
                strAcc += arr[i] + (i + 1 == arr.Length ? "" : separator);
            }
            return strAcc;
        }

        public static T Pop<T>(ref T[] arr, bool remove = false)
        {
            T lastValue = arr[arr.Length - 1];

            if (remove)
            {
                T[] newArr = new T[arr.Length - 1];
                arr = Copy(arr, newArr);
            }

            return lastValue;
        }

        public static string[] Randomizer(string[] arr)
        {
            string[] newArray = Copy(arr, new string[arr.Length]); // Copia original
            Random random = new Random();

            for (int i = 0; i < newArray.Length; i++)
            {
                int randomIndex = random.Next(i, newArray.Length); // indice aleatorio entre i y el final
                // Intercambiar posiciones
                string temp = newArray[i];
                newArray[i] = newArray[randomIndex];
                newArray[randomIndex] = temp;
            }

            return newArray;
        }

        public static string[] Swap(string[] arr, string value, int position)
        {
            if (
                arr.Length <= 0
                || StringUtils.Methods.IsNullOrWhiteSpace(value)
                || position < 0
                || position >= arr.Length
            )
                return [];

            int index = FindIndex(arr, value);
            if (index < 0 || index >= arr.Length)
                return arr;
                
            string swapValue = arr[position];

            arr[position] = value;
            arr[index] = swapValue;

            return arr;
        }

        // Para replicar el Push de un array
        public static void Push<T>(ref T[] arr, T value)
        {
            T[] newArr = new T[arr.Length + 1];
            T[] copyNewArr = Copy(arr, newArr);

            copyNewArr[arr.Length] = value;

            arr = copyNewArr;
        }

        public static int FindCoincidences<T>(T[] arr, T value)
        {
            int coincidences = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (object.Equals(arr[i], value))
                    coincidences++;
            }

            return coincidences;
        }

        public static int getMax(int[] numbers)
        {
            int max = numbers[0];

            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > max)
                    max = numbers[i];
            }

            return max;
        }

        public static T[] TakeFirstN<T>(T[] array, int start, int limit)
        {
            // Validaciones iniciales
            if (array == null || array.Length == 0 || start >= array.Length || limit < 1)
                return [];

            if (limit > array.Length)
                limit = array.Length;

            T[] newArray = [];

            for (int i = start; i < limit; i++)
            {
                Push(ref newArray, array[i]);
            }

            return newArray;
        }

        public static T[] Filter<T>(T[] arr, Func<T, bool> arrowFn)
        {
            T[] result = [];
            for (int i = 0; i < arr.Length; i++)
            {
                T value = arr[i];
                if (arrowFn(value))
                    Push(ref result, value);
            }
            return result;
        }

        public static TResult[] Map<TInput, TResult>(
            TInput[] inputArray,
            Func<TInput, TResult> arrowFn
        )
        {
            if (inputArray == null || arrowFn == null)
                return [];

            TResult[] result = new TResult[inputArray.Length];

            for (int i = 0; i < inputArray.Length; i++)
            {
                result[i] = arrowFn(inputArray[i]);
            }

            return result;
        }
    }
}
