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
    public void TestMethod1()
    {
    }
}
