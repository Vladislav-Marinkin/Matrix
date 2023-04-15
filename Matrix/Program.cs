using FileManager;

namespace Matrix
{
    internal class Program
    {
        static void Main()
        {
            IMatrixSplitter matrixSplitter = new MatrixSplitter();
            IMatrixFileManager fileMatrixManager = new MatrixFileManager();
            IMatrixManager matrixManager = new MatrixManager();
            IArrayMaxFinder arrayMaxFinder = new ArrayMaxFinder();

            // Инициализируем меню и запускаем метод старт
            var menu = new Menu(matrixManager, matrixSplitter, fileMatrixManager, arrayMaxFinder);
            menu.Start();
        }

    }
}