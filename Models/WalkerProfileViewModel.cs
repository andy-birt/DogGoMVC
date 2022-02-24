using System;
using System.Collections.Generic;

namespace DogGoMVC.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        public Walker Walker { get; set; }
        public List<Walk> Walks { get; set; }
    }
}
