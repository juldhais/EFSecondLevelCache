using Bogus;
using EFSecondLevelCache.Entities;

namespace EFSecondLevelCache.Repositories;

public static class DummyDataGenerator
{
    public static void Generate(this DataContext context)
    {
        if (context.Products.Any()) return;

        var faker = new Faker<Product>()
            .RuleFor(prop => prop.Code, opt => opt.Commerce.Ean8())
            .RuleFor(prop => prop.Description, opt => opt.Commerce.ProductName())
            .RuleFor(prop => prop.Category, opt => opt.Commerce.Product());

        var products = faker.Generate(100_000);
        context.AddRange(products);
        context.SaveChanges();
    }
}