using MvcMovie.Models;

namespace MvcMovie.Services
{
    public interface IFileservice
    {
        CSVModel? GetById(int? id);

        CSVModels? GetCollection();

        void Add(CSVModel item);

        void Update(CSVModel item);
    }
}
