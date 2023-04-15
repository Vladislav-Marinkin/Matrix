using FileManager;

namespace Worker
{
    internal class Program
    {
        static void Main()
        {
            // Объявляем переменную logger и инициализируем ее экземпляром класса ConsoleLogger, который реализует интерфейс ILogger
            ILogger logger = new ConsoleLogger();

            // Объявляем переменную fileManager и инициализируем ее экземпляром класса MatrixFileManager, который реализует интерфейс IMatrixFileManager
            IMatrixFileManager fileManager = new MatrixFileManager();

            // Создаем экземпляр класса MatrixSummarizer, используя ранее созданные объекты logger и fileManager
            MatrixSummarizer summarizer = new MatrixSummarizer(fileManager, logger);

            // Бесконечный цикл do-while, который выполняется до тех пор, пока не будет вызван break
            do
            {
                // Ожидаем маркерный файл для задания
                fileManager.WaitForMarkerFile("tasks_maker_marker_file.txt");

                // Получаем непрочитанный путь к файлу с подматрицей
                string path = fileManager.GetUnlockedFilePath();

                // Если путь получен, читаем подматрицу из файла
                int[,] matrix;

                if (path != null)
                {
                    matrix = fileManager.ReadSubMatrixFromFile(path);

                    // Вычисляем сумму элементов подматрицы с помощью метода SumElements, который определен в объекте summarizer
                    int sum = summarizer.SumElements(matrix);

                    // Разбираем имя файла и получаем номер файла
                    FileNameParser parser = new FileNameParser(path);
                    int? fileNumber = parser.GetFileNumber();

                    // Записываем результат в файл, используя номер файла в имени файла
                    fileManager.WriteNumberToFile(sum, $"result_{fileNumber}");
                }
            } while (true);
        }
    }
}