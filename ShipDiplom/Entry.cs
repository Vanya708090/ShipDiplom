namespace ShipDiplom;

public static class Entry
{
    /// <summary>
    /// Регистрация зависимостей уровня бизнес-логики
    /// </summary>
    /// <param name="serviceCollection">serviceCollection</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddDomain(this IServiceCollection serviceCollection)
    {
        //serviceCollection.AddTransient<IEntitySelector<Plan>, PlanSelector>();

        return serviceCollection;
    }
}