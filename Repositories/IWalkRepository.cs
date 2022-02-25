using System.Collections.Generic;
using DogGoMVC.Models;

namespace DogGoMVC.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetAllWalks();
        List<Walk> GetWalksByWalkerId(int it);
        void AddWalk(Walk walk);
        void DeleteWalks(List<int> ids);
    }
}
