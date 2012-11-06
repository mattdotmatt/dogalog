using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Linq;
using dogalog.Entities;
using dogalog.Models;

namespace dogalog.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IDocumentSession _session;

        public CategoryController(IDocumentSession session)
        {
            _session = session;
        }

        public ViewResult Index()
        {
            var categories = GetCategories();
            var categoryModels = AutoMapper.Mapper.Map<List<Category>, List<CategoryModel>>(categories);
            return View(categoryModels);
        }

        private List<Category> GetCategories()
        {
            List<Category> categories = (
                                            from company in _session.Query<Category>()
                                            select company
                                        )
                .ToList();
            return categories;
        }

        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult Add(CategoryModel category)
        {
            _session.Store(AutoMapper.Mapper.Map<CategoryModel, Category>(category));
            _session.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}