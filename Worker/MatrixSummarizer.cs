using FileManager;

namespace Worker
{
    public interface IMatrixSummarizer
    {
        int SumElements(int[,] matrix);
    }

    public class MatrixSummarizer : IMatrixSummarizer
    {
        private readonly IMatrixFileManager fileManager;
        private readonly ILogger logger;

        public MatrixSummarizer(IMatrixFileManager fileManager, ILogger logger)
        {
            this.fileManager = fileManager;
            this.logger = logger;
        }

        public int SumElements(int[,] matrix)
        {
            int sum = 0;
            string message;

            try
            {
                // Проходим по всем строкам матрицы
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    // Проходим по всем столбцам матрицы
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        // Суммируем элементы матрицы
                        sum += matrix[i, j];
                    }
                }
            }
            // Обрабатываем исключение NullReferenceException
            catch (Exception ex)
            {
                // Формируем сообщение об ошибке
                message = $"Ошибка подсчета суммы '{ex.Message}'";
                // Логируем сообщение об ошибке с уровнем LogLevel.Error
                logger.Log(message, LogLevel.Error);

                // Возвращаем значение int.MinValue, которое будет обозначать ошибку при вычислении суммы элементов матрицы
                return int.MinValue;
            }

            // Формируем сообщение о успешном вычислении суммы элементов матрицы
            message = $"Сумма элементов матрицы равна {sum}";
            // Логируем сообщение о вычислении суммы элементов матрицы с уровнем LogLevel.Info
            logger.Log(message, LogLevel.Info);

            // Возвращаем значение суммы элементов матрицы
            return sum;
        }
    }
}
