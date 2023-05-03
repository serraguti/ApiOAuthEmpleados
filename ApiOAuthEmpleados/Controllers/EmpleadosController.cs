using ApiOAuthEmpleados.Models;
using ApiOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSwag.Annotations;
using System.Security.Claims;

namespace ApiOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("EMPLEADOS")]
    public class EmpleadosController : ControllerBase
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        // GET: api/Empleados
        /// <summary>
        /// Obtiene el conjunto de Empleados, tabla EMP.
        /// </summary>
        /// <remarks>
        /// Método para devolver todos los empleados de la BBDD
        /// Dicho método está protegido por TOKEN
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="401">Unathorized. No autorizado.</response> 
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<Empleado>>> GetEmpleados()
        {
            //USERDATA
            //HttpContext.User.FindFirst("USERDATA")
            return await this.repo.GetEmpleadosAsync();
        }

        // GET: api/Empleados/id
        /// <summary>
        /// Obtiene un Empleado por su Id, tabla EMP.
        /// </summary>
        /// <remarks>
        /// Permite buscar un empleado por su ID de empresa
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Empleado>> FindEmpleado(int id)
        {
            var empleado = await this.repo.FindEmpleadoAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return empleado;
        }

        // POST: api/empresa
        /// <summary>
        /// Crea una nueva Empresa en la BD, tabla EMPRESAS..
        /// </summary>
        /// <remarks>
        /// Este método crea una empresa enviando el Nombre de la empresa por URL
        /// </remarks>
        /// <param name="nombre">String con el nombre de la Empresa.</param>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPost]
        [Route("[action]/{nombre}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateEmpleado(string nombre)
        {
            return Ok();
        }

        // POST: api/empresa
        /// <summary>
        /// Crea una nueva Empresa en la BD, tabla EMPRESAS..
        /// </summary>
        /// <remarks>
        /// Este método crea una empresa enviando el Nombre de la empresa por URL
        /// </remarks>
        /// <param name="empleado">String con el nombre de la Empresa.</param>
        /// <param name="id">ID del empleado.</param>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPost]
        [Route("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateEmpleadoOtro(Empleado empleado, int id)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Empleado>> PerfilEmpleado()
        {
            //DEBEMOS BUSCAR EL CLAIM DEL EMPLEADO
            Claim claim = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado =
                claim.Value;
            Empleado empleado = JsonConvert.DeserializeObject<Empleado>
                (jsonEmpleado);
            return empleado;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Empleado>>> CompisCurro()
        {
            string jsonEmpleado = HttpContext.User.Claims
                .SingleOrDefault(z => z.Type == "UserData").Value;
            Empleado empleado =
                JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);
            List<Empleado> compis =
                await this.repo.GetCompisCurro(empleado.IdDepartamento);
            return compis;
        }
    }
}
