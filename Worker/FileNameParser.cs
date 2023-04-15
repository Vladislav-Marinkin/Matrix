namespace Worker
{
    internal class FileNameParser
    {
        private readonly string _filePath;

        public FileNameParser(string filePath)
        {
            _filePath = filePath;
        }

        public int? GetFileNumber()
        {
            string fileName = Path.GetFileNameWithoutExtension(_filePath);
            if (string.IsNullOrEmpty(fileName))
            {
                // Обработка ошибки, если имя файла пустое или null
                return null;
            }

            string[] nameParts = fileName.Split('_');
            if (nameParts.Length < 2)
            {
                // Обработка ошибки, если имя файла не содержит номера
                return null;
            }

            string numberPart = nameParts[nameParts.Length - 1];
            if (!int.TryParse(numberPart, out int fileNumber))
            {
                // Обработка ошибки, если номер в имени файла не является целым числом
                return null;
            }

            return fileNumber;
        }
    }
}
