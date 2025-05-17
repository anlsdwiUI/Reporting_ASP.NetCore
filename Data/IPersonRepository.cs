using Reporting.Models;

namespace Reporting.Data
{
    public interface IPersonRepository
    {
        List<Person> GetAllPersons();
    }
}
