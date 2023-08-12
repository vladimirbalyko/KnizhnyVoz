using KnizhnyVoz.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

namespace KnizhnyVoz.Services
{
    public interface IBooksApi
    {
        IList<Book> GetBooks();
        IList<AudioFile> GetFiles(string id);
    }

    public class BooksApi : IBooksApi
    {
        private readonly string _url = "https://knizhnyvoz.herokuapp.com/{0}";

        public IList<Book> GetBooks()
        {
            return JsonSerializer.Deserialize<IList<Book>>(GetResponse("books"));
        }

        public IList<AudioFile> GetFiles(string id)
        {
            return JsonSerializer.Deserialize<IList<AudioFile>>(GetResponse($"books/{id}"));
        }

        private string GetResponse(string method)
        {
            var request = WebRequest.Create(string.Format(_url, method));
            request.Method = "GET";

            using var webResponse = request.GetResponse();
            using var webStream = webResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            return reader.ReadToEnd();
        }
    }
}
