using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheHiddenSquirrel.Controllers;
using TheHiddenSquirrel.Data;
using TheHiddenSquirrel.Models;

namespace TheHiddenSquirrelTests;

[TestClass]
public class CategoriesControllerTests
{
    // class level vars shared among all tests & instantiated in TestInitialize
    private ApplicationDbContext _context;
    CategoriesController controller;

    [TestInitialize]
    public void TestInitialize()
    {
        // this runs automatically before each test to set up objects used in every test
        // setup & create in-memory db while running tests
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        // populate in-memory db w/some mock Category data
        _context.Category.Add(new Category { CategoryId = 99, Name = "First Category" });
        _context.Category.Add(new Category { CategoryId = 44, Name = "Category 44" });
        _context.Category.Add(new Category { CategoryId = 128, Name = "The BEST Category" });
        _context.SaveChanges();

        // instantiate CategoriesController using our in-memory db
        controller = new CategoriesController(_context);
    }

    [TestMethod]
    public void IndexReturnsView() // check correct view
    {
        // arrange - all done in TestInitialize

        // act
        var result = (ViewResult)controller.Index();

        // assert
        Assert.AreEqual("Index", result.ViewName);
    }

    [TestMethod]
    public void IndexReturnsCategories() // check correct data
    {
        // arrange - all done in TestInitialize

        // act
        var result = (ViewResult)controller.Index();

        // assert - is category list in db the same as the data shown in the view?
        CollectionAssert.AreEqual(_context.Category.ToList(), (List<Category>)result.Model);
    }

    [TestMethod]
    public void EditGetInvalidIdReturns404()
    {
        // act
        var result = (ViewResult)controller.Edit(-4);

        // assert
        Assert.AreEqual("404", result.ViewName);
    }

    [TestMethod]
    public void EditGetValidIdReturnsView()
    {
        // act - must use valid id from in-memory db above
        var result = (ViewResult)controller.Edit(128);

        // assert
        Assert.AreEqual("Edit", result.ViewName);
    }

    [TestMethod]
    public void EditGetValidIdReturnsCategory()
    {
        // act - use valid id from in-memory db
        var result = (ViewResult)controller.Edit(128);

        // assert
        Assert.AreEqual(_context.Category.Find(128), (Category)result.Model);
    }
}
