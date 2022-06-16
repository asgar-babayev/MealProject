using MealProject.Models;
using System.Collections.Generic;

namespace MealProject.ViewModels
{
    public class HomeVm
    {
        public List<Menu> Menus { get; set; }
        public List<Category> Categories { get; set; }
    }
}
