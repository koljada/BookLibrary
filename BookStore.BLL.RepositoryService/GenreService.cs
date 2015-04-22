using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Interface.Abstract;
using BookStore.DLL.Interface.Abstract;
using BookStore.DO.Entities;

namespace BookStore.DLL.RepositoryService
{
    public class GenreService:StoreService<Genre>,IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository repository):base(repository)
        {
            _genreRepository = repository;
        }
     
    }
}
