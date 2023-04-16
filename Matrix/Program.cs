using FileManager;

namespace Matrix
{
    internal class Program
    {
        static void Main()
        {
            // Инициализируем экземпляр класса IMatrixSplitter, IMatrixManager, IArrayMaxFinder
            IMatrixSplitter matrixSplitter = new MatrixSplitter();
            IMatrixManager matrixManager = new MatrixManager();
            IArrayMaxFinder arrayMaxFinder = new ArrayMaxFinder();

            // Объявляем переменную fileManager и инициализируем ее экземпляром класса MatrixFileManager, который реализует интерфейс IMatrixFileManager
            IMatrixFileManager fileMatrixManager = new MatrixFileManager();

            // Инициализируем меню и запускаем метод старт
            var menu = new Menu(matrixManager, matrixSplitter, fileMatrixManager, arrayMaxFinder);
            menu.Start();
        }

    }
}