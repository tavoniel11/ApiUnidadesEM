using Microsoft.AspNetCore.Mvc;
using Apiunidades.Modelo;
using System.Reflection;
using static Apiunidades.Modelo.DatosUnidades;

namespace Apiunidades.Controllers
{
    [Route("mc/[controller]")]
    [ApiController]

    public class ControlUnidades : ControllerBase
    {

        #region "--------------+Get+-----------------"
        [HttpGet]
        public List<string[]> Get()
        {
            List<string[]> Lista = new List<string[]>();
            try
            {
                Lista.Add(new string[] { $"Modulo {Utilidades.NombreServicio} .Net7" });
            }
            catch { }
            return Lista;
        }
        #endregion+
     

        [HttpGet]
        [Route("ObtenerUnidades")]
        public List<ParametrosUnidades> ObtenerUnidades()
        {
            List<ParametrosUnidades> unidades = new List<ParametrosUnidades>();
            try
            {
                unidades = new DatosUnidades().ObtenerUnidades();
            }
            catch (Exception ObjExcepcion)
            {
                Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod());

            }
            return unidades;

        }

        [HttpPost]
        [Route("InsertarUnidad")]
        public Respuesta InsertarUnidad([FromBody] ParametrosPostUnidades _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            DatosUnidades Datos = new DatosUnidades();
            try
            {
                if (_Parametros != null)
                {
                    Objeto = Datos.InsertarUnidad(_Parametros);
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
        [Route("EliminarUnidad")]
        public Respuesta EliminarUnidad([FromBody] ParametrosPostUnidades _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            DatosUnidades Datos = new DatosUnidades();
            try
            {
                if (_Parametros != null)
                {
                    Objeto = Datos.EliminarUnidad(_Parametros);
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

        [HttpPut]
        [Route("ModificarUnidad")]
        public Respuesta ModificarUnidad([FromBody] ParametrosPostUnidades _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            DatosUnidades Datos = new DatosUnidades();
            try
            {
                if (_Parametros != null)
                {
                    Objeto = Datos.ModificarUnidad(_Parametros);
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

        [HttpPost("BuscarUnidad")]
        public List<ParametrosUnidades> BuscarUnidad([FromBody] ParametrosPostUnidades _Parametros)
        {
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            List<ParametrosUnidades> unidades = new List<ParametrosUnidades>();
            try
            {
                if (_Parametros != null)
                {
                    unidades = new DatosUnidades().BuscarUnidad(_Parametros);
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
