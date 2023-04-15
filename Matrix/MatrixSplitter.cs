namespace Matrix
{
    /*Объявление интерфейса IMatrixSplitter, 
     * содержащего метод SplitMatrix, 
     * который будет реализован в классе MatrixSplitter.
     */
    public interface IMatrixSplitter
    {
        List<int[,]> GetAllSubMatrices(int[,] matrix);
    }

    /*Определение класса MatrixSplitter, 
     * который реализует интерфейс IMatrixSplitter и метод GetAllSubMatrices. 
     * Создание списка subMatrices, который будет содержать подматрицы.
     */
    public class MatrixSplitter : IMatrixSplitter
    {
        public List<int[,]> GetAllSubMatrices(int[,] matrix)
        {
            List<int[,]> subMatrices = new List<int[,]>();
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            // Перебираем все строки и столбцы матрицы
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    // Рассматриваем все возможные подматрицы, содержащие данный элемент
                    for (int k = 2; k <= Math.Min(numRows - i, numCols - j); k++)
                    {
                        // Получаем подматрицу
                        int[,] subMatrix = GetSubMatrix(matrix, i, j, k);

                        // Добавляем подматрицу в список, если она не является исходной матрицей
                        if (!IsSameMatrix(matrix, subMatrix))
                        {
                            subMatrices.Add(subMatrix);
                        }
                    }
                }
            }
            return subMatrices;
        }

        // Этот метод возвращает подматрицу заданного размера из матрицы matrix,
        // начиная с позиции (startRow, startCol)
        private int[,] GetSubMatrix(int[,] matrix, int startRow, int startCol, int size)
        {
            // Создаем новую матрицу размером size x size, которая будет хранить подматрицу
            int[,] subMatrix = new int[size, size];

            // Перебираем все элементы подматрицы и копируем их из матрицы matrix
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    subMatrix[i, j] = matrix[startRow + i, startCol + j];
                }
            }

            // Возвращаем полученную подматрицу
            return subMatrix;
        }

        // Метод для сравнения двух матриц
        private bool IsSameMatrix(int[,] matrix1, int[,] matrix2)
        {
            // Проверяем размеры матриц
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
            {
                return false;
            }
            // Перебираем элементы матриц и сравниваем их
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    if (matrix1[i, j] != matrix2[i, j])
                    {
                        return false;
                    }
                }
            }
            // Если все элементы совпадают, то матрицы равны
            return true;
        }
    }
}
