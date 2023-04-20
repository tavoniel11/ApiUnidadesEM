using Apiunidades.Modelo;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using static Apiunidades.Modelo.DatosEventos;
namespace Apiunidades.Controllers
{
    [Route("mc/[controller]")]
    [ApiController]

    public class ControlEventos : ControllerBase
    {
        [HttpGet]
        [Route("ObtenerEventos")]
        public List<ParametrosEventos> ObtenerEventos()
        {
            List<ParametrosEventos> unidades = new List<ParametrosEventos>();
            try
            {
                unidades = new DatosEventos().ObtenerEventos();
            }
            catch (Exception ObjExcepcion)
            {
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return unidades;

        }

        [HttpPost]
        [Route("InsertarEvento")]
        public Respuesta InsertarEvento([FromBody] ParametrosPostEventos _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            DatosEventos Datos = new DatosEventos();
            try
            {
                if (_Parametros != null)
                {
                    Objeto = Datos.InsertarEvento(_Parametros);
                }
                else
                {
                    Objeto.Estado = -1001;
                    Objeto.Mensaje = "Error de parametros: los parametros son requeridos";
                }
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general Control";
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return Objeto;
        }


        [HttpDelete]
        [Route("EliminarEvento")]
        public Respuesta EliminarEvento([FromBody] ParametrosPostEventos _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            DatosEventos Datos = new DatosEventos();
            try
            {
                if (_Parametros != null)
                {
                    Objeto = Datos.EliminarEvento(_Parametros);
                }
                else
                {
                    Objeto.Estado = -1001;
                    Objeto.Mensaje = "Error de parametros: los parametros son requeridos";
                }
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general Control";
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return Objeto;
        }

        [HttpPost("BuscarEvento")]
        public List<ParametrosEventos> BuscarEvento([FromBody] ParametrosPostEventos _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            List<ParametrosEventos> unidades = new List<ParametrosEventos>();
            try
            {
                if (_Parametros != null)
                {
                    unidades = new DatosEventos().BuscarEvento(_Parametros);
                }
                else
                {
                    Objeto.Estado = -1001;
                    Objeto.Mensaje = "Error de parametros: los parametros son requeridos";
                }
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general Control";
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return unidades;
        }

        [HttpGet("EventosAntiguos")]
        public List<ParametrosEventos> EventosAntiguos()
        {
            List<ParametrosEventos> unidades = new List<ParametrosEventos>();
            try
            {
                unidades = new DatosEventos().EventosAntiguos();
            }
            catch (Exception ObjExcepcion)
            {
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return unidades;

        }

        [HttpPost("EventosAntiguosxComando")]
        public List<ParametrosEventos> EventosAntiguosxComando([FromBody] ParametrosPostEventos _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            List<ParametrosEventos> unidades = new List<ParametrosEventos>();
            try
            {
                if (_Parametros != null)
                {
                    unidades = new DatosEventos().EventosAntiguosxComando(_Parametros);
                }
                else
                {
                    Objeto.Estado = -1001;
                    Objeto.Mensaje = "Error de parametros: los parametros son requeridos";
                }
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general Control";
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return unidades;
        }

        [HttpGet("EventosRecientes")]
        public List<ParametrosEventos> EventosRecientes()
        {
            List<ParametrosEventos> unidades = new List<ParametrosEventos>();
            try
            {
                unidades = new DatosEventos().EventosRecientes();
            }
            catch (Exception ObjExcepcion)
            {
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return unidades;

        }

        [HttpPost("EventosRecientesxComando")]
        public List<ParametrosEventos> EventosRecientesxComando([FromBody] ParametrosPostEventos _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            List<ParametrosEventos> unidades = new List<ParametrosEventos>();
            try
            {
                if (_Parametros != null)
                {
                    unidades = new DatosEventos().EventosRecientesxComando(_Parametros);
                }
                else
                {
                    Objeto.Estado = -1001;
                    Objeto.Mensaje = "Error de parametros: los parametros son requeridos";
                }
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general Control";
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return unidades;
        }

        [HttpPost("EventosRangoxFecha")]
        public List<ParametrosEventos> EventosRangoxFecha([FromBody] ParametrosPostEventos _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            List<ParametrosEventos> unidades = new List<ParametrosEventos>();
            try
            {
                if (_Parametros != null)
                {
                    unidades = new DatosEventos().EventosRangoxFecha(_Parametros);
                }
                else
                {
                    Objeto.Estado = -1001;
                    Objeto.Mensaje = "Error de parametros: los parametros son requeridos";
                }
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general Control";
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return unidades;
        }


    }
    
    
    
}
