using System.Web.Mvc;
using Raven.Client;
using dogalog.Entities;
using dogalog.Models;

namespace dogalog.Controllers
{
    public class CategoryController:Controller
    {
        private readonly IDocumentSession _session;

        public CategoryController(IDocumentSession session)
        {
            _session = session;
        }

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult Add(CategoryModel category)
        {
            AutoMapper.Mapper.CreateMap<CategoryModel, Category>();
            _session.Store(AutoMapper.Mapper.Map<CategoryModel,Category>(category));
            return RedirectToAction("Index");
        }

    }
}