using BenchmarkDotNet.Attributes;

namespace dxStudyYieldReturn;

[MemoryDiagnoser]
public class BenchmarkTest
{
    [Benchmark]
    public void ProcessCustomer()
    {
        var listCustomers = GetCustomers(1000000);
        foreach (var c in listCustomers)
        {
            if (c.Id >= 1000)
                break;

            Console.WriteLine($"ID: {c.Id}, Name: {c.Name}");
        }
    }

    [Benchmark]
    public void ProcessCustomerYield()
    {
        var listCustomers = GetCustomersYield(1000000);
        foreach (var c in listCustomers)
        {
            if (c.Id >= 1000)
                break;

            Console.WriteLine($"ID: {c.Id}, Name: {c.Name}");
        }
    }

    public IEnumerable<Customer> GetCustomers(int count)
    {
        var listCustomers = new List<Customer>();
        for (int i = 0; i < count; i++)
        {
            var customer = new Customer()
            {
                Id = i,
                Name = $"Name{i}"
            };

            listCustomers.Add(customer);
        }

        return listCustomers;
    }

    public IEnumerable<Customer> GetCustomersYield(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new Customer()
            {
                Id = i,
                Name = $"Name{i}"
            };
        }
    }
}
