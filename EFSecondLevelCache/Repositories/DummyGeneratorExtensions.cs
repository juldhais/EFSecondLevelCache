using Bogus;
using EFSecondLevelCache.Entities;

namespace EFSecondLevelCache.Repositories;

public static class DummyGeneratorExtensions
{
    public static void GenerateDummyProducts(this DataContext context)
    {
        if (context.Products.Any()) return;

        var faker = new Faker<Product>()
            .RuleFor(prop => prop.Code, opt => opt.Commerce.Ean8())
            .RuleFor(prop => prop.Description, opt => opt.Commerce.ProductName());

        var products = faker.Generate(1000);
        context.AddRange(products);
        context.SaveChanges();
    }
}