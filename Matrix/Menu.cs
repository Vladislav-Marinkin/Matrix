using FileManager;

namespace Matrix
{
    public class Menu
    {
        private readonly IMatrixManager matrixManager;
        private readonly IMatrixSplitter matrixSplitter;
        private readonly IMatrixFileManager fileMatrixManager;
        private readonly IArrayMaxFinder arrayMaxFinder;

        public Menu(IMatrixManager matrixManager, IMatrixSplitter matrixSplitter, IMatrixFileManager fileMatrixManager, IArrayMaxFinder arrayMaxFinder)
        {
            this.matrixManager = matrixManager;
            this.matrixSplitter = matrixSplitter;
            this.fileMatrixManager = fileMatrixManager;
            this.arrayMaxFinder = arrayMaxFinder;
        }

        public void Start()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Выберите опцию:");
                Console.WriteLine("1. Загрузить матрицу 3х3");
                Console.WriteLine("2. Загрузить матрицу 5х5");
                Console.WriteLine("3. Загрузить матрицу 10х10");
                Console.WriteLine("4. Создать случайную матрицу");
                Console.WriteLine("5. Выход");

                int option;

                // проверка корректности ввода
                while (!int.TryParse(Console.ReadLine(), out option))
                {
                    Console.WriteLine("Некорректная опция. Пожалуйста, введите число.");
                }

                Console.Clear();

                switch (option)
                {
                    case 1:
                        LoadMatrix(3, 0, 0, false);
                        break;
                    case 2:
                        LoadMatrix(5, 0, 0, false);
                        break;
                    case 3:
                        LoadMatrix(10, 0, 0, false);
                        break;
                    case 4:
                        CreateRandomMatrix();
                        break;
                    case 5:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный вариант. Пожалуйста, введите номер правильного варианта.");
                        break;
                }

                List<int> sums = new List<int>();

                foreach (string path in fileMatrixManager.GetAllFilesInDirectory())
                {
                    sums.Add(fileMatrixManager.ReadNumberFromFile(path));
                }

                int max = arrayMaxFinder.FindMax(sums.ToArray());

                Console.WriteLine($"Максимальная сумма из всех подматриц: {max}");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private void LoadMatrix(int size, int minValue, int maxValue, bool random)
        {
            try
            {
                int[,] matrix;

                Console.WriteLine("Загружена матрица:");
                if (random)
                {
                    matrix = matrixManager.LoadMatrix(size, minValue, maxValue);
                }
                else
                {
                    matrix = matrixManager.LoadMatrix(size);
                }
                PrintMatrix(matrix);

                List<int[,]> subMatrices = matrixSplitter.GetAllSubMatrices(matrix);
                Console.WriteLine($"Будет создано {subMatrices.Count} подматриц.");
                Console.WriteLine();

                string markerFilePath = fileMatrixManager.CreateMarkerFile("tasks_maker_marker_file.txt");
                Console.WriteLine($"Создан файл маркера: {markerFilePath}");
                Console.WriteLine();

                int key = 0;

                foreach (int[,] subMatrix in subMatrices)
                {
                    fileMatrixManager.WriteSubMatrixToFile(subMatrix, $"Task_{key++}");
                }

                Console.WriteLine("Ожидание завершения подсчета сумм всех подматриц");
                fileMatrixManager.WaitForDirectoryEmpty();

                fileMatrixManager.DeleteMarkerFile(markerFilePath);
                Console.WriteLine($"Удален файл маркера: {markerFilePath}");

                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the matrix: {ex.Message}");
            }
        }

        private void CreateRandomMatrix()
        {
            Console.WriteLine("Введите размер матрицы:");
            int size = GetPositiveIntegerInput();

            Console.WriteLine("Введите минимальное значение элементов:");
            int minValue = GetIntegerInput();

            Console.WriteLine("Введите максимальное значение элементов:");
            int maxValue = GetIntegerInput();

            LoadMatrix(size, minValue, maxValue, true);
        }

        private int GetIntegerInput()
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Введите целое число.");
            }

            return input;
        }

        private int GetPositiveIntegerInput()
        {
            int input;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out input) && input > 0)
                {
                    return input;
                }

                Console.WriteLine("Введите положительное целое число.");
            }
        }

        private void PrintMatrix(int[,] matrix)
        {
            Console.WriteLine("Матрица:");

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }

                Console.WriteLine();
            }
        }
    }
}
