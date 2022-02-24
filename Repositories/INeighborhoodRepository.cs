using System.Collections.Generic;
using DogGoMVC.Models;

namespace DogGoMVC.Repositories
{
    public interface INeighborhoodRepository
    {
        List<Neighborhood> GetAll();
    }
}
