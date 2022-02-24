using System.Collections.Generic;
using DogGoMVC.Models;

namespace DogGoMVC.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetWalksByWalkerId(int it);
    }
}
