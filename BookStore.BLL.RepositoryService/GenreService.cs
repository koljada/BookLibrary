using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;

namespace BookStore.BLL.RepositoryService
{
    public class GenreService:StoreService<Genre>,IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private Genre _genre = new Genre();
        public GenreService(IGenreRepository repository):base(repository)
        {
            _genreRepository = repository;
        }

        public IQueryable<Genre> Genres
        {
            get
            {
                return _genreRepository.GetAll();
            }
        }
    }
}
