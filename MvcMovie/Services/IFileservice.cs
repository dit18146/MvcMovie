using MvcMovie.Models;

namespace MvcMovie.Services
{
    public interface IFileservice
    {
        CSVModel? GetById(long newsletterId);

        CSVModels? GetCollection();

        void Add(CSVModel item);

        void DeleteAll();


    }
}
