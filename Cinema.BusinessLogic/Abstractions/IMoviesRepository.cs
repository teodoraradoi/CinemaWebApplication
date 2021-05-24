using Cinema.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.BusinessLogic.Abstractions
{
    public interface IMoviesRepository
    {
        IEnumerable<Movies> GetCurrentMovies();
        IEnumerable<Movies> GetComingSoonMovies();

        Movies GetMovieById(Guid Id);

        IEnumerable<Movies> GetByName(string Name);
    }
}
