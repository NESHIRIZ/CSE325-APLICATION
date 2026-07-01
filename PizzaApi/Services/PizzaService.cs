using PizzaApi.Models;

namespace PizzaApi.Services;

public sealed class PizzaService
{
    private static readonly object SyncRoot = new();
    private static readonly List<Pizza> Store = new();

    static PizzaService()
    {
        Seed();
    }

    public IReadOnlyList<Pizza> GetAll()
    {
        lock (SyncRoot)
        {
            return Store.Select(Clone).ToList();
        }
    }

    public Pizza? Get(int id)
    {
        lock (SyncRoot)
        {
            return Store.FirstOrDefault(item => item.Id == id) is { } pizza ? Clone(pizza) : null;
        }
    }

    public Pizza Add(Pizza pizza)
    {
        ArgumentNullException.ThrowIfNull(pizza);

        lock (SyncRoot)
        {
            if (Store.Any(existing => existing.Id == pizza.Id))
            {
                throw new InvalidOperationException($"A pizza with id {pizza.Id} already exists.");
            }

            var newPizza = Clone(pizza);
            Store.Add(newPizza);
            return Clone(newPizza);
        }
    }

    public bool Update(Pizza pizza)
    {
        ArgumentNullException.ThrowIfNull(pizza);

        lock (SyncRoot)
        {
            var existing = Store.FirstOrDefault(item => item.Id == pizza.Id);
            if (existing is null)
            {
                return false;
            }

            existing.Name = pizza.Name;
            existing.IsGlutenFree = pizza.IsGlutenFree;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (SyncRoot)
        {
            var target = Store.FirstOrDefault(item => item.Id == id);
            if (target is null)
            {
                return false;
            }

            Store.Remove(target);
            return true;
        }
    }

    private static void Seed()
    {
        Store.Add(new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false });
        Store.Add(new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true });
        Store.Add(new Pizza { Id = 3, Name = "BBQ Chicken & Bacon", IsGlutenFree = false });
    }

    private static Pizza Clone(Pizza pizza) => new()
    {
        Id = pizza.Id,
        Name = pizza.Name,
        IsGlutenFree = pizza.IsGlutenFree
    };
}
