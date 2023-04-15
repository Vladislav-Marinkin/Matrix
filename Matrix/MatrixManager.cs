using FileManager;

namespace Matrix
{
    public interface IMatrixManager
    {
        int[,] LoadMatrix(int size);
        int[,] LoadMatrix(int size, int minValue, int maxValue);
    }

    public class MatrixManager : IMatrixManager
    {
        // Метод рандомного заполнения матрицы
        public int[,] LoadMatrix(int size, int minValue, int maxValue)
        {
            Random random = new Random();
            int[,] matrix = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = random.Next(minValue, maxValue);
                }
            }

            return matrix;
        }

        public int[,] LoadMatrix(int size)
        {
            int[,] matrix = new int[size, size];

            // Хардкодим матрицы
            switch (size)
            {
                case 3:
                    matrix = new int[,] { { -50, 30, 40 }, { -10, -20, 70 }, { 80, -90, 60 } };
                    break;
                case 5:
                    matrix = new int[,] { { -80, 10, -20, 50, 70 }, { -30, -90, -50, -40, 20 }, { 60, 40, 30, 20, -60 }, { 50, -10, 0, 80, -70 }, { -50, 90, -30, -70, -80 } };
                    break;
                case 10:
                    matrix = new int[,] { { 30, 50, -70, -20, 50, 80, -10, 0, 40, 60 }, { -80, -20, -60, -90, -10, -80, -50, 90, 30, -70 }, { 20, 70, -50, -80, 60, -70, -20, 80, 30, -30 }, { 80, 10, -10, -40, 60, -90, 0, -60, -20, -50 }, { -70, -30, 90, -10, 80, 60, 20, -90, 50, 30 }, { -10, -30, -40, 90, 40, 70, 80, 20, -60, -20 }, { -90, -50, -80, 20, -10, -70, 60, -30, -60, 80 }, { 10, -80, 50, 30, 70, 40, -60, 20, 90, -50 }, { -20, -70, -30, -40, -90, -50, -10, 60, 40, 70 }, { 50, 60, 0, -30, -20, -40, -70, -80, -90, 30 } };
                    break;
                default:
                    throw new ArgumentException("Недопустимый размер матрицы. Доступные размеры: 3, 5, 10");
            }

            return matrix;
        }
    }
}
