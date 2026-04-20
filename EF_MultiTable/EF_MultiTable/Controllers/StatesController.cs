using EF_MultiTable.Services.Interfaces;
using EF_MultiTable.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EF_MultiTable.Controllers
{
    public class StatesController : Controller
    {

        private readonly IStateService _service;

        public StatesController(IStateService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var data = _service.GetAll();
            return View(data);
        }

        public IActionResult Create()
        {
            return View(new StateCityVM());
        }

        [HttpPost]
        public IActionResult Create(StateCityVM vm)
        {
            if (ModelState.IsValid)
            {
                _service.Create(vm);
                return RedirectToAction("Index");
            }
            return View(vm);

        }

        public IActionResult Edit(int id)
        {
            var data = _service.Get(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(StateCityVM vm)
        {
            if (ModelState.IsValid)
            {
                _service.Update(vm);
                return RedirectToAction("Index");
            }
            return View(vm);

        }

        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
