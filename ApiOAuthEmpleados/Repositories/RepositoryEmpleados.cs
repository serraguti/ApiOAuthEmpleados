using ApiOAuthEmpleados.Data;
using ApiOAuthEmpleados.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiOAuthEmpleados.Repositories
{
    public class RepositoryEmpleados
    {
        private EmpleadosContext context;

        public RepositoryEmpleados(EmpleadosContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await
                this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int idempleado)
        {
            return await
                this.context.Empleados.FirstOrDefaultAsync
                (x => x.IdEmpleado == idempleado);
        }

        public async Task<Empleado> ExisteEmpleadoAsync
            (string apellido, int idempleado)
        {
            return await
                this.context.Empleados
                .FirstOrDefaultAsync(x => x.Apellido == apellido
                && x.IdEmpleado == idempleado);
        }
    }
}
