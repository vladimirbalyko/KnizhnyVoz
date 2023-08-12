using KnizhnyVoz.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace KnizhnyVoz.Services
{
    interface IFolderStructureService
    {
        void CreateFolderStructure(string root, IEnumerable<Book> books);
    }

    internal class FolderStructureService : IFolderStructureService
    {
        private readonly IFileSystemService _fileSystemService;

        public FolderStructureService(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        public void CreateFolderStructure(string root, IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                var folderPath = _fileSystemService.CreateDirectory(book.Name);
                if (!string.IsNullOrWhiteSpace(folderPath))
                {
                    if (!string.IsNullOrWhiteSpace(book.ImageUri))
                    {
                        var client = new WebClient();
                        client.DownloadFile(book.ImageUri, $"{folderPath}\\banner.jpeg");
                    }

                    CreateBookSummary(folderPath, book);
                }
            }
        }

        private void CreateBookSummary(string path, Book book)
        {
            var text = new StringBuilder(book.Name);
            text.AppendLine();
            text.Append($"author: {book.Author}");
            text.AppendLine();
            text.Append($"description: {book.Description}");
            _fileSystemService.CreateFile(path, "summary", text.ToString());
        }
    }
}
