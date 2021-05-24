using Cinema.BusinessLogic.Abstractions;
using Cinema.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinema.DataAccess
{
    public class EFMoviesRepository : IMoviesRepository
    {
        private readonly CinemaDbContext dataContext;

        public EFMoviesRepository(CinemaDbContext context)
        {
            dataContext = context;
        }

        IEnumerable<Movies> IMoviesRepository.GetCurrentMovies()
        {
            DateTime date = DateTime.Now;
            return dataContext.Movies.Where(m => m.ReleaseDate <= date).ToList();
        }

        IEnumerable<Movies> IMoviesRepository.GetComingSoonMovies()
        {
            DateTime date = DateTime.Now;
            return dataContext.Movies.Where(m => m.ReleaseDate > date).ToList();
        }

        IEnumerable<Movies> IMoviesRepository.GetByName(string name)
        {
            return dataContext.Movies.Where(movie => movie.Name.Contains(name));
        }

        Movies IMoviesRepository.GetMovieById(Guid id)
        {
            return dataContext.Movies.Single(movie => movie.Id.Equals(id));
        }
    }
}
