using DogGoMVC.Models;
using System.Collections.Generic;

namespace DogGoMVC.Models.ViewModels
{
    public class WalksDeleteFormModel
    {
        public Walk Walk { get; set; }
        public List<Walk> Walks { get; set; }
        public List<ItemToDelete> Items { get; set; }
    }
}
