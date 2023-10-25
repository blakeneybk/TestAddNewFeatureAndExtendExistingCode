using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Litmus.Core.Logging;
using NUnit.Framework;
using Sakila.Data;

namespace Sakila.Test.Data
{
    [TestFixture]
    public class CategoryRepositoryFixture
    {
        private readonly CategoryRepository repository;

        public CategoryRepositoryFixture()
        {
            repository = new CategoryRepository(new SakilaSqliteDatabaseConnection(new ConsoleStructuredLogger()));
        }

        [Test]
        public async Task GetAllCategoriesReturnsExpectedValues()
        {
            var categories = await repository.GetAllCategories(CancellationToken.None);

            CollectionAssert.IsNotEmpty(categories);
            var firstCategory = categories.FirstOrDefault();

            Assert.That(firstCategory.CategoryId, Is.EqualTo(1));
            Assert.That(firstCategory.Name, Is.EqualTo("Action"));
        }

        [Test]
        public async Task GetCategoryByIdReturnsExpectedValue()
        {
            var rv = await repository.GetCategoryByIdAsync(5,CancellationToken.None);

            Assert.IsNotNull(rv);

            Assert.That(rv.CategoryId, Is.EqualTo(5));
            Assert.That(rv.Name, Is.EqualTo("Comedy"));
        }
    }
}
