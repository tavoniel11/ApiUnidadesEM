using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Reflection;

namespace Apiunidades.Modelo
{
    public class DatosUnidades
    {
        public List<ParametrosUnidades> ObtenerUnidades()
        {
            List<ParametrosUnidades> Lista = new List<ParametrosUnidades>();
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                Query = "SELECT int_id, var_matricula, var_nombre, bol_enuso, dt_modificacion, dt_registro FROM public.tbl_unidades WHERE bol_enuso=true; ";

                Conexion.Open();
                NpgsqlCommand Comm = new NpgsqlCommand(Query, Conexion);

                NpgsqlDataReader reader = Comm.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ParametrosUnidades dt = new ParametrosUnidades();
                        dt.int_id = (int)reader["int_id"];
                        dt.var_matricula = (string)reader["var_matricula"];
                        dt.var_nombre = (string)reader["var_nombre"];
                        dt.bol_enuso = (bool)reader["bol_enuso"];
                        dt.dt_modificacion = Convert.ToDateTime(reader["dt_modificacion"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        dt.dt_registro = Convert.ToDateTime(reader["dt_registro"]).ToString("yyyy-MM-dd HH:mm:ss.fff");



                        Lista.Add(dt);
                    }
                    Objeto.Estado = 1;
                    Objeto.Mensaje = "Ok";
                    Conexion.Close();
                }
            }
            catch(Exception ObjExcepcion)
            {
                Objeto.Datos = -1;
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general";

                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod(), Query);
            }
            finally
            {
                if (Conexion.State != ConnectionState.Closed)
                    Conexion.Close();
            }

            return Lista;
        }

        public Respuesta InsertarUnidad(ParametrosPostUnidades _Parametros)
        {
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (!string.IsNullOrEmpty(_Parametros.var_matricula))
                {
                    Conexion.Open();
                    Query = string.Format("select count(1) from public.tbl_unidades where trim(lower(var_matricula)) = '{0}'", _Parametros.var_matricula.Trim().ToLower());

                    NpgsqlCommand Comm = new NpgsqlCommand(Query, Conexion);
                    int Name = Convert.ToInt32(Comm.ExecuteScalar());

                    if (Name == 0)
                    {
                        Query = "call sp_InsertarUnidad(@matricula, @nombre)";

                        NpgsqlCommand Comm0 = new NpgsqlCommand(Query, Conexion);
                        Comm0.CommandType = CommandType.Text;
                        Comm0.Parameters.Add(new NpgsqlParameter("@matricula", NpgsqlDbType.Varchar)).Value = _Parametros.var_matricula;
                        Comm0.Parameters.Add(new NpgsqlParameter("@nombre", NpgsqlDbType.Varchar)).Value = _Parametros.var_nombre;
                        Comm0.ExecuteScalar();

                        Objeto.Datos = 1;
                        Objeto.Estado = 1;
                        Objeto.Mensaje = "Ok";
                        Conexion.Close();
                    }
                    else
                    {
                        Objeto.Datos = -3;
                        Objeto.Estado = -3;
                        Objeto.Mensaje = "Error: matrícula existente";
                    }
                }
                else
                {
                    Objeto.Datos = -4;
                    Objeto.Estado = -4;
                    Objeto.Mensaje = "Error: Párametros vacios";
                }
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Datos = -1;
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general";

                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod(), Query);
            }
            finally
            {
                if (Conexion.State != ConnectionState.Closed)
                    Conexion.Close();
            }

            return Objeto;
        }

        public Respuesta EliminarUnidad(ParametrosPostUnidades _Parametros)
        {
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (_Parametros.int_id is not null)
                {
                    Conexion.Open();
                    Query = string.Format("select count(1) from public.tbl_unidades where int_id = {0};", _Parametros.int_id);
                    NpgsqlCommand Comm = new NpgsqlCommand(Query, Conexion);
                    int tabla = Convert.ToInt32(Comm.ExecuteScalar());

                    if (tabla != 0)
                    {
                        Query = "call sp_EliminarUnidad(@idunidad)";

                        NpgsqlCommand Comm0 = new NpgsqlCommand(Query, Conexion);
                        Comm0.CommandType = CommandType.Text;
                        Comm0.Parameters.Add(new NpgsqlParameter("@idunidad", NpgsqlDbType.Integer)).Value = _Parametros.int_id;
                        Comm0.ExecuteScalar();

                        Objeto.Datos = _Parametros.int_id;
                        Objeto.Estado = 1;
                        Objeto.Mensaje = "Ok";
                        Conexion.Close();
                    }
                    else
                    {
                        Objeto.Datos = -3;
                        Objeto.Estado = -3;
                        Objeto.Mensaje = "Error: Id no existente";
                    }
                }
                else
                {
                    Objeto.Datos = -4;
                    Objeto.Estado = -4;
                    Objeto.Mensaje = "Error: Párametro vacios";
                }
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Datos = -1;
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general";

                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod(), Query);
            }
            finally
            {
                if (Conexion.State != ConnectionState.Closed)
                    Conexion.Close();
            }
            return Objeto;
        }

        public Respuesta ModificarUnidad(ParametrosPostUnidades _Parametros)
        {
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (_Parametros.int_id > 0)
                {
                    if(_Parametros.var_matricula is not null || _Parametros.var_nombre is not null)
                    {
                        Conexion.Open();
                        Query = string.Format("select count(1) from public.tbl_unidades where trim(lower(var_matricula)) = '{0}'", _Parametros.var_matricula.ToLower().Trim());
                        NpgsqlCommand Comm = new NpgsqlCommand(Query, Conexion);
                        int existencia = Convert.ToInt32(Comm.ExecuteScalar());
                        if (existencia == 0)
                        {
                            Query = string.Format("select count(1) from public.tbl_unidades where int_id = {0};", _Parametros.int_id);
                            NpgsqlCommand Comm0 = new NpgsqlCommand(Query, Conexion);
                            existencia = Convert.ToInt32(Comm0.ExecuteScalar());

                            if (existencia != 0)
                            {
                                Query = "call sp_ActualizarUnidad (@id,@matricula,@nombre)";

                                NpgsqlCommand Comm1 = new NpgsqlCommand(Query, Conexion);
                                Comm1.CommandType = CommandType.Text;
                                Comm1.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Integer)).Value = _Parametros.int_id;
                                Comm1.Parameters.Add(new NpgsqlParameter("@matricula", NpgsqlDbType.Varchar)).Value = _Parametros.var_matricula;
                                Comm1.Parameters.Add(new NpgsqlParameter("@nombre", NpgsqlDbType.Varchar)).Value = _Parametros.var_nombre;
                                Comm1.ExecuteScalar();

                                Objeto.Datos = _Parametros.int_id;
                                Objeto.Estado = 1;
                                Objeto.Mensaje = "Ok";
                                Conexion.Close();
                            }
                            else
                            {
                                Objeto.Datos = -2;
                                Objeto.Estado = -2;
                                Objeto.Mensaje = "Error: id no existente";
                            }
                        }
                        else
                        {
                            Objeto.Datos = -3;
                            Objeto.Estado = -3;
                            Objeto.Mensaje = "Error: matricula existente";
                        }
                    }
                    else
                    {
                        Objeto.Datos = -3;
                        Objeto.Estado = -3;
                        Objeto.Mensaje = "Error: Valores vacios o nulos";
                    }

                }
                else
                {
                    Objeto.Datos = -4;
                    Objeto.Estado = -4;
                    Objeto.Mensaje = "Error: Id no valido";
                }
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Datos = -1;
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general";

                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod(), Query);
            }
            finally
            {
                if (Conexion.State != ConnectionState.Closed)
                    Conexion.Close();
            }
            return Objeto;
        }

        public List<ParametrosUnidades> BuscarUnidad(ParametrosPostUnidades _Parametros)
        {
            List<ParametrosUnidades> Lista = new List<ParametrosUnidades>();
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (!string.IsNullOrEmpty(_Parametros.var_busqueda))
                {
                    Query = "select * from _BuscarUnidad (@busqueda)";

                    Conexion.Open();
                    NpgsqlCommand Comm1 = new NpgsqlCommand(Query, Conexion);
                    Comm1.CommandType = CommandType.Text;
                    Comm1.Parameters.Add(new NpgsqlParameter("@busqueda", NpgsqlDbType.Varchar)).Value = _Parametros.var_busqueda;
                    NpgsqlDataReader reader = Comm1.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ParametrosUnidades dt = new ParametrosUnidades();
                            dt.int_id = (int)reader["id"];
                            dt.var_matricula = (string)reader["matricula"];
                            dt.var_nombre = (string)reader["nombre"];
                            dt.bol_enuso = (bool)reader["enuso"];
                            dt.dt_modificacion = Convert.ToDateTime(reader["modificacion"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            dt.dt_registro = Convert.ToDateTime(reader["registro"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                            Lista.Add(dt);
                        }
                        Objeto.Estado = 1;
                        Objeto.Mensaje = "Ok";
                        Conexion.Close();
                    }
                    Objeto.Estado = -1;
                    Objeto.Mensaje = "Unidad no encontrada";
                }
                else
                {
                    Objeto.Estado = -2;
                    Objeto.Mensaje = "Error: busqueda vacio o nula";
                }
                    
            }
            catch (Exception ObjExcepcion)
            {
                Objeto.Datos = -1;
                Objeto.Estado = -1;
                Objeto.Mensaje = "Error general";

                Utilidades.RegistrarError(Utilidades.NombreServicio, ObjExcepcion, MethodBase.GetCurrentMethod(), Query);
            }
            finally
            {
                if (Conexion.State != ConnectionState.Closed)
                    Conexion.Close();
            }

            return Lista;
        }

        public class ParametrosUnidades 
        {
            public int int_id { get; set; }
            public string var_matricula { get; set; }
            public string var_nombre { get; set; }
            public bool bol_enuso { get; set; }
            public string dt_modificacion { get; set; }    
            public string dt_registro { get; set; }
        }

        public class ParametrosPostUnidades
        {
            public int? int_id { get; set; }
            public string? var_matricula { get; set; }
            public string? var_nombre { get; set; }
            public string? var_busqueda { get; set; }

        }

        public class ParametroGeneral
        {
            public int IdUsuario { get; set; }
            public string Token { get; set; }
        }

    }
}
