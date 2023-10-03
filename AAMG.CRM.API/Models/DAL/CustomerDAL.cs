using AAMG.CRMAPI.Models.EN;
using Microsoft.EntityFrameworkCore;

namespace AAMG.CRMAPI.Models.DAL
{
    public class CustomerDAL
    {
        readonly AAMGContext context;

        public CustomerDAL(AAMGContext aAMGContext)
        {
            context = aAMGContext;
        }
        public async Task<int> Create(Customer customer)
        {
            context.Add(customer);
            return await context.SaveChangesAsync();
        }

        public async Task<Customer> GetById(int id)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            return customer != null ? customer : new Customer();
        }

        public async Task<int> Edit(Customer customer)
        {
            int result = 0;
            var customerUpdate = await GetById(customer.Id);
            if (customerUpdate.Id != 0)
            {
                customerUpdate.Name = customer.Name;
                customerUpdate.LastName = customer.LastName;
                customerUpdate.Address = customer.Address;
                result = await context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Delete(int id)
        {
            int result = 0;
            var customerDelete = await GetById(id);
            if (customerDelete.Id > 0)
            {
                context.Customers.Remove(customerDelete);
                return await context.SaveChangesAsync();
            }
            return result;
        }

        //Metodo para buscar clientes por un filtro - IQueryable.
        public IQueryable<Customer> Query(Customer customer)
        {
            var query = context.Customers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(customer.Name))
                query = query.Where(s => s.Name.Contains(customer.Name));
            if (!string.IsNullOrWhiteSpace(customer.LastName))
                query = query.Where(s => s.LastName.Contains(customer.LastName));

            return query;
        }

        //Metodo para contar la cantidad de resultados de busqueda con filtros.
        public async Task<int> CountSearch(Customer customer)
        {
            return await Query(customer).CountAsync();
        }

        //Metodo para buscar clientes por filtros, paginacion y ordenamiento.
        public async Task<List<Customer>> Search(Customer customer, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;
            var query = Query(customer);
            query = query.OrderByDescending(s => s.Id).Skip(skip).Take(take);
            return await query.ToListAsync();
        }
    }
}
