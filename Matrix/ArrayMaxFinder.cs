namespace Matrix
{
    public interface IArrayMaxFinder
    {
        int FindMax(int[] array);
    }

    public class ArrayMaxFinder : IArrayMaxFinder
    {
        // Реализация метода FindMax для однопоточного поиска максимального значения в массиве
        public int FindMax(int[] array)
        {
            // Проверяем, что массив не пустой или null
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Массив пустой");
            }

            // Инициализируем переменную max первым элементом массива
            int max = array[0];

            // Проходим по остальным элементам массива и сравниваем их с максимальным значением
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                }
            }

            return max;
        }
    }
}
