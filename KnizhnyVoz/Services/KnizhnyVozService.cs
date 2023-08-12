using KnizhnyVoz.Models;
using KnizhnyVoz.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace KnizhnyVoz.Services
{
    interface IKnizhnyVozService
    {
        void Execute();
    }

    public class KnizhnyVozService : IKnizhnyVozService
    {
        private readonly IList<Book> _books;
        private readonly IBooksApi _api;
        private readonly IFileSystemService _fileSystemService;
        private readonly IFolderStructureService _folderStructureService;
        private readonly string _rootFolder = $"{Directory.GetCurrentDirectory()}/{"Knizhny Voz"}";
        public KnizhnyVozService()
        {
            _api = new BooksApi();
            _fileSystemService = new FileSystemService(_rootFolder);
            _folderStructureService = new FolderStructureService(_fileSystemService);

            _books = _api.GetBooks();
        }

        public void Execute()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Please press any key to start.");
                Console.ReadLine();
                int choice = Menu();

                while (choice != 0)
                {
                    switch (choice)
                    {
                        case 1:
                            DisplayBooks();
                            break;
                        case 2:
                            DownloadBook();
                            break;
                        case 3:
                            DownloadAllBooks();
                            break;
                    }
                    if (choice == 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        choice = Menu();
                    }
                }

                Console.WriteLine("Session ended");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exception);
            }
        }

        private int Menu()
        {
            // Console UI
            Console.WriteLine("Choose value:");
            Console.WriteLine("1 - Show book list;");
            Console.WriteLine("2 - Download book (by number / Id);");
            Console.WriteLine("3 - Download all;");
            Console.WriteLine("0 - Exit");
            Console.WriteLine();

            int.TryParse(Console.ReadLine(), out var choice);
            return choice;
        }

        private void DisplayBooks()
        {
            foreach (var (book, index) in _books.WithIndex())
            {
                Console.WriteLine($"{index} - '{book.Name}' ({book.Author}) [{book.Id}]");
            }
        }

        private void DownloadBook()
        {
            Console.WriteLine("Please enter book number or GUID.");
            var bookId = Console.ReadLine();
            if (int.TryParse(bookId, out var id))
            {
                bookId = _books[id].Id;
            }

            var books = _api.GetBooks().Where(p => p.Id == bookId)?.ToList();

            DownloadBooks(books);
        }

        private void DownloadAllBooks()
        {
            var books = _api.GetBooks();
            DownloadBooks(books);
        }

        private void DownloadBooks(IList<Book> books)
        {
            _folderStructureService.CreateFolderStructure(_rootFolder, books);

            string ids = string.Empty;

            var max = books.Count();
            var current = 0;
            foreach (var book in books)
            {
                ids += $"{book.Id};";
                var folderPath = $"{_rootFolder}/{_fileSystemService.RemoveInvalidChars(book.Name)}";
                var files = _api.GetFiles(book.Id);

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var path = $"{folderPath}/{i + 1} - {_fileSystemService.RemoveInvalidChars(file.Name)}.mp3";
                    if (!File.Exists(path))
                    {
                        var client = new WebClient();
                        client.DownloadFile(file.Url, path);
                        Console.WriteLine($"The new '{path}' file was downloaded.");
                    }

                }

                current++;
                Console.WriteLine($"The '{book.Name}' book was downloaded.");
                Console.WriteLine($"{current * 100 / max} % was downloaded.", ConsoleColor.Blue);
            }

            _fileSystemService.CreateFile("summary", ids);

            Console.WriteLine("All books have been successfully downloaded.");
        }
    }
}
