using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;

        public PieController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        public IActionResult List()
        {
            PieListViewModel pieViewModel = new PieListViewModel(_pieRepository.AllPies,"Cheese cake");
            return View(pieViewModel);
        }
    }
}
