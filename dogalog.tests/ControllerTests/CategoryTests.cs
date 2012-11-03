using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Raven.Client;
using dogalog.Controllers;
using dogalog.Entities;
using dogalog.Models;

namespace dogalog.tests.ControllerTests
{
    [TestFixture]
    public class CategoryTests
    {

        private Mock<IDocumentSession> _mockDocumentSession;

        [SetUp]
        public void Setup()
        {
            _mockDocumentSession = new Mock<IDocumentSession>(MockBehavior.Strict);
        }

        [Test]
        public void AddCategoryShouldReturnAViewForTheCategoryAddition()
        {
            // Arrange

            // Act
            var sut = new CategoryController(_mockDocumentSession.Object);
            var result = sut.Add();

            // Assert
            _mockDocumentSession.Verify();
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void AddCategoryPostShouldRequireADescription()
        {
            // Arrange
            const string description = "Category";
            _mockDocumentSession.Setup(o => o.Store(It.Is<Category>(
                    x=>x.Description==description
                ))).Verifiable("Category should be saved");

            var categoryModel = new CategoryModel
                {
                    Description = description
                };

            // Act
            var sut = new CategoryController(_mockDocumentSession.Object);
            var result = sut.Add(categoryModel);

            // Assert
            _mockDocumentSession.Verify();
        }

        [Test]
        public void AddCategoryPostShouldRedirectToCategoryList()
        {
            // Arrange
            _mockDocumentSession.Setup(o => o.Store(It.IsAny<Category>())).Verifiable("Category should be saved");

            var category = new CategoryModel
            {
                Description = "Category"
            };

            // Act
            var sut = new CategoryController(_mockDocumentSession.Object);
            var result = sut.Add(category);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

        }
    }
}