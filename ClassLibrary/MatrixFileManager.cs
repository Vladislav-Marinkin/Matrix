namespace FileManager
{
    /*Объявление интерфейса IMatrixFileManager, 
     * содержащего методами ReadNumberFromFile, WriteSubMatrixToFile и ReadSubMatrixFromFile, 
     * которые будут реализованны в классе MatrixFileManager.
     */

    public interface IMatrixFileManager
    {
        public void WriteNumberToFile(int number, string fileName);
        public void WriteSubMatrixToFile(int[,] subMatrix, string fileName);
        public int[,] ReadSubMatrixFromFile(string filePath);
        public int ReadNumberFromFile(string filePath);
        public string CreateMarkerFile(string markerFileName);
        public void WaitForMarkerFile(string markerFileName);
        public bool WaitForDirectoryEmpty();
        public string GetUnlockedFilePath();
        public void DeleteMarkerFile(string filePath);
        public List<string> GetAllFilesInDirectory();
    }

    public class MatrixFileManager : IMatrixFileManager
    {
        private string rootPath = "../../../../";
        private Mutex mutex = new Mutex();

        public void WriteSubMatrixToFile(int[,] subMatrix, string fileName)
        {
            try
            {
                string filePath = rootPath + "tasks/";
                CreateDirectoryIfNotExists(filePath);
                filePath += fileName;

                // Ожидаем доступ к файлу, пока он не освободится
                mutex.WaitOne();

                // Открываем файловый поток с указанным путем и режимом доступа на запись
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    // Открываем потоковый писатель, который будет записывать данные в файловый поток
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        // Получаем размер подматрицы
                        int size = subMatrix.GetLength(0);

                        // Перебираем элементы подматрицы и записываем их в файл через пробел
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                writer.Write(subMatrix[i, j] + " ");
                            }
                            writer.WriteLine();
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                // Если возникла ошибка ввода-вывода, выводим сообщение об ошибке
                Console.WriteLine($"Произошла ошибка при записи в файл: {ex.Message}");
            }
            finally
            {
                // Освобождаем доступ к файлу
                mutex.ReleaseMutex();
            }
        }

        public int[,] ReadSubMatrixFromFile(string filePath)
        {
            int[,] subMatrix = null;

            try
            {
                // Ожидаем доступ к файлу, пока он не освободится
                mutex.WaitOne();

                // Открываем файловый поток с указанным путем и режимом доступа на чтение
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Открываем потоковый читатель, который будет считывать данные из файлового потока
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        // Считываем первую строку, чтобы определить размер подматрицы
                        string firstLine = reader.ReadLine();
                        int size = firstLine.Trim().Split(' ').Length;

                        // Создаем массив для подматрицы заданного размера
                        subMatrix = new int[size, size];

                        // Заполняем массив элементами из файла
                        subMatrix[0, 0] = int.Parse(firstLine.Trim().Split(' ')[0]);
                        subMatrix[0, 1] = int.Parse(firstLine.Trim().Split(' ')[1]);
                        for (int i = 1; i < size; i++)
                        {
                            string line = reader.ReadLine();
                            string[] elements = line.Trim().Split(' ');
                            for (int j = 0; j < size; j++)
                            {
                                subMatrix[i, j] = int.Parse(elements[j]);
                            }
                        }
                    }
                }
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                // Если возникла ошибка ввода-вывода, выводим сообщение об ошибке
                Console.WriteLine($"Произошла ошибка при чтении файла: {ex.Message}");
            }
            finally
            {
                // Освобождаем доступ к файлу
                mutex.ReleaseMutex();
            }
            return subMatrix;
        }

        public int ReadNumberFromFile(string filePath)
        {
            int number = 0;
            try
            {
                // Проверяем, существует ли файл
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Файл '{filePath}' не найден.");
                }

                // Проверяем, заблокирован ли файл
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // Проверяем, что файл не слишком большой для чтения числа
                    if (stream.Length > sizeof(int))
                    {
                        throw new IOException($"Файл '{filePath}' слишком велик для чтения числа.");
                    }

                    // Читаем число из файла
                    using (var reader = new BinaryReader(stream))
                    {
                        number = reader.ReadInt32();
                    }
                }

                File.Delete(filePath);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            return number;
        }

        public string CreateMarkerFile(string markerFileName)
        {
            string markerFilePath = Path.Combine(rootPath, markerFileName);
            File.WriteAllText(markerFilePath, "Файл маркера для приложения матричного сплиттера");
            return markerFilePath;
        }

        public void DeleteMarkerFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    Console.WriteLine("Файл не существует");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        public void WaitForMarkerFile(string markerFileName)
        {

            string markerFilePath = rootPath + markerFileName;

            while (!File.Exists(markerFilePath))
            {
                Console.WriteLine($"Файл маркера не найден");
                Console.WriteLine("Ожидание...");
                Thread.Sleep(1000);
            }
        }

        private void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        // Метод ожидает, пока директория по заданному пути не станет пустой
        // Возвращает true, если директория пуста, и false в противном случае
        public bool WaitForDirectoryEmpty()
        {
            // Задаем путь к директории, которую нужно проверить на пустоту
            string filePath = rootPath + "tasks/";
            CreateDirectoryIfNotExists(filePath);

            // Изначально считаем, что директория не пуста
            bool isEmpty = false;

            try
            {
                // Создаем объект для работы с директорией
                var directory = new DirectoryInfo(filePath);

                // Создаем объект для отслеживания изменений в директории
                var watcher = new FileSystemWatcher(filePath);

                // Создаем объект SemaphoreSlim, который будет использоваться для синхронизации
                // между основным потоком и обработчиком событий FileSystemWatcher
                var semaphore = new SemaphoreSlim(0);

                // Создаем обработчик событий FileSystemEventHandler, который будет вызван при удалении файлов
                FileSystemEventHandler handler = (object sender, FileSystemEventArgs e) =>
                {
                    // Если удаляются файлы и в директории больше нет файлов, то директория пуста
                    if (e.ChangeType == WatcherChangeTypes.Deleted && directory.GetFiles().Length == 0)
                    {
                        isEmpty = true;
                        semaphore.Release();
                    }
                };

                // Включаем отслеживание событий
                watcher.EnableRaisingEvents = true;

                // Отключаем отслеживание изменений во вложенных директориях
                watcher.IncludeSubdirectories = false;

                // Отслеживаем изменения в имени файла, размере и имени директории
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.DirectoryName;

                // Устанавливаем обработчик событий удаления файлов
                watcher.Deleted += handler;

                // Ждем, пока семафор не будет освобожден
                semaphore.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            // Возвращаем флаг пустоты директории
            return isEmpty;
        }

        public string GetUnlockedFilePath()
        {
            string filesPath = rootPath + "tasks/";
            CreateDirectoryIfNotExists(filesPath);

            string[] files = Directory.GetFiles(filesPath);

            foreach (string filePath in files)
            {
                try
                {
                    // Попытаемся открыть файл в режиме только для чтения, чтобы проверить, заблокирован он или нет
                    using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        // Если файл не заблокирован, вернём его путь
                        return filePath;
                    }
                }
                catch (IOException)
                {
                    // Файл заблокирован, пропускаем его
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    // Файл заблокирован, пропускаем его
                    continue;
                }
            }

            // Если не найден ни один не заблокированный файл, вернём null
            return null;
        }

        public void WriteNumberToFile(int number, string fileName)
        {
            try
            {
                string filePath = rootPath + "results/";
                CreateDirectoryIfNotExists(filePath);
                filePath += fileName;

                // Проверяем, заблокирован ли файл
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // Проверяем, что число не выходит за диапазон допустимых значений для данного типа файла
                    if (stream.Length + sizeof(double) > int.MaxValue)
                    {
                        throw new IOException($"Файл '{filePath}' слишком велик для записи числа.");
                    }

                    // Записываем число в файл
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write(number);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        public List<string> GetAllFilesInDirectory()
        {
            string filePath = rootPath + "results/";
            CreateDirectoryIfNotExists(filePath);

            // Проверяем, существует ли папка по указанному пути
            if (!Directory.Exists(filePath))
            {
                throw new ArgumentException("Directory does not exist");
            }

            List<string> filePaths = new List<string>();

            try
            {
                // Получаем все файлы в папке, включая подпапки
                string[] files = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories);

                // Добавляем каждый файл в список filePaths
                foreach (string file in files)
                {
                    // Проверяем, заблокирован ли файл другим процессом
                    bool isFileLocked = IsFileLocked(file);

                    // Если файл не заблокирован, добавляем его путь в список filePaths
                    if (!isFileLocked)
                    {
                        filePaths.Add(file);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ловим исключение, если произошла ошибка
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return filePaths;
        }

        // Метод, который проверяет, заблокирован ли файл другим процессом
        private bool IsFileLocked(string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // Если файл не заблокирован, закрываем его и возвращаем false
                    stream.Close();
                    return false;
                }
            }
            catch (IOException)
            {
                // Если файл заблокирован, возвращаем true
                return true;
            }
        }
    }
}
