using KnizhnyVoz.Services;

namespace KnizhnyVoz
{
    internal class Program
    {
        /// <summary>
        /// Need to do:
        /// - Download book logo
        /// - Update summury to actions log
        /// - Add configuration file
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            IKnizhnyVozService service = new KnizhnyVozService();
            service.Execute();
        }
    }
}
