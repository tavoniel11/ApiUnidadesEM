using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Reflection;

namespace Apiunidades.Modelo
{
    public class DatosEventos
    {
        public List<ParametrosEventos> ObtenerEventos()
        {
            List<ParametrosEventos> Lista = new List<ParametrosEventos>();
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                Query = "select * from _ObtenerEventos()";

                Conexion.Open();
                NpgsqlCommand Comm = new NpgsqlCommand(Query, Conexion);

                NpgsqlDataReader reader = Comm.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ParametrosEventos dt = new ParametrosEventos();
                        dt.int_id = (int)reader["id"];
                        dt.var_nombre = (string)reader["nombre"];
                        dt.var_matricula = (string)reader["matricula"];
                        dt.var_comando = (string)reader["comando"];
                        dt.var_valor = (string)reader["valor"];
                        dt.dt_modificacion = Convert.ToDateTime(reader["fecha"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

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

        public Respuesta InsertarEvento(ParametrosPostEventos _Parametros)
        {
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (_Parametros.int_idunidad > 0 && _Parametros.int_comando >0)
                {
                    Conexion.Open();
                    Query = string.Format("select count(1) from public.tbl_unidades where int_id = '{0}'", _Parametros.int_idunidad);
                    NpgsqlCommand Comm = new NpgsqlCommand(Query, Conexion);
                    int idunidad = Convert.ToInt32(Comm.ExecuteScalar());

                    if (idunidad > 0)
                    {
                        Query = string.Format("select count(1) from public.tbl_comandos where int_id = '{0}'", _Parametros.int_comando);
                        NpgsqlCommand Comm0 = new NpgsqlCommand(Query, Conexion);
                        int idcomando = Convert.ToInt32(Comm0.ExecuteScalar());
                        if (idcomando > 0)
                        {
                            Query = "call sp_InsertarEvento (@idunidad, @idcomando)";

                            NpgsqlCommand Comm1 = new NpgsqlCommand(Query, Conexion);
                            Comm1.CommandType = CommandType.Text;
                            Comm1.Parameters.Add(new NpgsqlParameter("@idunidad", NpgsqlDbType.Integer)).Value = _Parametros.int_idunidad;
                            Comm1.Parameters.Add(new NpgsqlParameter("@idcomando", NpgsqlDbType.Integer)).Value = _Parametros.int_comando;
                            Comm1.ExecuteScalar();

                            Objeto.Datos = 1;
                            Objeto.Estado = 1;
                            Objeto.Mensaje = "Ok";
                            Conexion.Close();
                        }
                        else
                        {
                            Objeto.Datos = -2;
                            Objeto.Estado = -2;
                            Objeto.Mensaje = "Error: idcomando no existente";
                        }
                    }
                    else
                    {
                        Objeto.Datos = -3;
                        Objeto.Estado = -3;
                        Objeto.Mensaje = "Error: idunidad no existente";
                    }
                }
                else
                {
                    Objeto.Datos = -4;
                    Objeto.Estado = -4;
                    Objeto.Mensaje = "Error: Párametros no validos";
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

        public Respuesta EliminarEvento(ParametrosPostEventos _Parametros)
        {
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (_Parametros.int_id >0)
                {
                    Conexion.Open();
                    Query = string.Format("select count(1) from public.tbl_eventos where int_id = {0};", _Parametros.int_id);
                    NpgsqlCommand Comm = new NpgsqlCommand(Query, Conexion);
                    int idevento = Convert.ToInt32(Comm.ExecuteScalar());

                    if (idevento ==1)
                    {
                        Query = "call sp_EliminarEvento(@id)";

                        NpgsqlCommand Comm0 = new NpgsqlCommand(Query, Conexion);
                        Comm0.CommandType = CommandType.Text;
                        Comm0.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Integer)).Value = _Parametros.int_id;
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
                        Objeto.Mensaje = "Error: Idevento no existente";
                    }
                }
                else
                {
                    Objeto.Datos = -4;
                    Objeto.Estado = -4;
                    Objeto.Mensaje = "Error: Párametro no valido";
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

        public List<ParametrosEventos> BuscarEvento(ParametrosPostEventos _Parametros)
        {
            List<ParametrosEventos> Lista = new List<ParametrosEventos>();
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (!string.IsNullOrEmpty(_Parametros.var_busqueda))
                {
                    Query = "select * from _BuscarEvento (@busqueda);";

                    Conexion.Open();
                    NpgsqlCommand Comm1 = new NpgsqlCommand(Query, Conexion);
                    Comm1.CommandType = CommandType.Text;
                    Comm1.Parameters.Add(new NpgsqlParameter("@busqueda", NpgsqlDbType.Varchar)).Value = _Parametros.var_busqueda;
                    NpgsqlDataReader reader = Comm1.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ParametrosEventos dt = new ParametrosEventos();
                            dt.int_id = (int)reader["id"];
                            dt.var_nombre = (string)reader["nombre"];
                            dt.var_matricula = (string)reader["matricula"];
                            dt.var_comando = (string)reader["comando"];
                            dt.var_valor = (string)reader["valor"];
                            dt.dt_modificacion = Convert.ToDateTime(reader["fecha"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                            Lista.Add(dt);
                        }
                        Objeto.Estado = 1;
                        Objeto.Mensaje = "Ok";
                        Conexion.Close();
                    }
                }
                else
                {
                    Objeto.Estado = -1;
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

        public List<ParametrosEventos> EventosAntiguos()
        {
            List<ParametrosEventos> Lista = new List<ParametrosEventos>();
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                Query = "select * from _EventosAntiguos()";

                Conexion.Open();
                NpgsqlCommand Comm = new NpgsqlCommand(Query, Conexion);

                NpgsqlDataReader reader = Comm.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ParametrosEventos dt = new ParametrosEventos();
                        dt.int_id = (int)reader["id"];
                        dt.var_nombre = (string)reader["nombre"];
                        dt.var_matricula = (string)reader["matricula"];
                        dt.var_comando = (string)reader["comando"];
                        dt.var_valor = (string)reader["valor"];
                        dt.dt_modificacion = Convert.ToDateTime(reader["fecha"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                        Lista.Add(dt);
                    }
                    Objeto.Estado = 1;
                    Objeto.Mensaje = "Ok";
                    Conexion.Close();
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

        public List<ParametrosEventos> EventosRecientes()
        {
            List<ParametrosEventos> Lista = new List<ParametrosEventos>();
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                Query = "select * from _EventosRecientes()";

                Conexion.Open();
                NpgsqlCommand Comm = new NpgsqlCommand(Query, Conexion);

                NpgsqlDataReader reader = Comm.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ParametrosEventos dt = new ParametrosEventos();
                        dt.int_id = (int)reader["id"];
                        dt.var_nombre = (string)reader["nombre"];
                        dt.var_matricula = (string)reader["matricula"];
                        dt.var_comando = (string)reader["comando"];
                        dt.var_valor = (string)reader["valor"];
                        dt.dt_modificacion = Convert.ToDateTime(reader["fecha"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                        Lista.Add(dt);
                    }
                    Objeto.Estado = 1;
                    Objeto.Mensaje = "Ok";
                    Conexion.Close();
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

        public List<ParametrosEventos> EventosAntiguosxComando(ParametrosPostEventos _Parametros)
        {
            List<ParametrosEventos> Lista = new List<ParametrosEventos>();
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (!string.IsNullOrEmpty(_Parametros.var_comando))
                {
                    Query = "select * from _EventosAntiguosxComando(@comando)";

                    Conexion.Open();
                    NpgsqlCommand Comm1 = new NpgsqlCommand(Query, Conexion);
                    Comm1.CommandType = CommandType.Text;
                    Comm1.Parameters.Add(new NpgsqlParameter("@comando", NpgsqlDbType.Varchar)).Value = _Parametros.var_comando;
                    NpgsqlDataReader reader = Comm1.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ParametrosEventos dt = new ParametrosEventos();
                            dt.int_id = (int)reader["id"];
                            dt.var_nombre = (string)reader["nombre"];
                            dt.var_matricula = (string)reader["matricula"];
                            dt.var_comando = (string)reader["comando"];
                            dt.var_valor = (string)reader["valor"];
                            dt.dt_modificacion = Convert.ToDateTime(reader["fecha"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                            Lista.Add(dt);
                        }
                        Objeto.Estado = 1;
                        Objeto.Mensaje = "Ok";
                        Conexion.Close();
                    }
                }
                else
                {
                    Objeto.Estado = -1;
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

        public List<ParametrosEventos> EventosRecientesxComando(ParametrosPostEventos _Parametros)
        {
            List<ParametrosEventos> Lista = new List<ParametrosEventos>();
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (!string.IsNullOrEmpty(_Parametros.var_comando))
                {
                    Query = "select * from _EventosRecientesxComando(@comando)";

                    Conexion.Open();
                    NpgsqlCommand Comm1 = new NpgsqlCommand(Query, Conexion);
                    Comm1.CommandType = CommandType.Text;
                    Comm1.Parameters.Add(new NpgsqlParameter("@comando", NpgsqlDbType.Varchar)).Value = _Parametros.var_comando;
                    NpgsqlDataReader reader = Comm1.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ParametrosEventos dt = new ParametrosEventos();
                            dt.int_id = (int)reader["id"];
                            dt.var_nombre = (string)reader["nombre"];
                            dt.var_matricula = (string)reader["matricula"];
                            dt.var_comando = (string)reader["comando"];
                            dt.var_valor = (string)reader["valor"];
                            dt.dt_modificacion = Convert.ToDateTime(reader["fecha"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                            Lista.Add(dt);
                        }
                        Objeto.Estado = 1;
                        Objeto.Mensaje = "Ok";
                        Conexion.Close();
                    }
                }
                else
                {
                    Objeto.Estado = -1;
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

        public List<ParametrosEventos> EventosRangoxFecha(ParametrosPostEventos _Parametros)
        {
            List<ParametrosEventos> Lista = new List<ParametrosEventos>();
            string Query = string.Empty;
            NpgsqlConnection Conexion = Utilidades.ObtenerConexion();
            Respuesta Objeto = new Respuesta() { Estado = 0, Datos = 0, Mensaje = "Error de datos" };
            try
            {
                if (!string.IsNullOrEmpty(_Parametros.var_comando))
                {
                    Query = "select * from _EventosRangoxFecha(@fechaini,@fechafin,@comando)";

                    Conexion.Open();
                    NpgsqlCommand Comm1 = new NpgsqlCommand(Query, Conexion);
                    Comm1.CommandType = CommandType.Text;
                    Comm1.Parameters.Add(new NpgsqlParameter("@fechaini", NpgsqlDbType.Varchar)).Value = _Parametros.dt_fechaini;
                    Comm1.Parameters.Add(new NpgsqlParameter("@fechafin", NpgsqlDbType.Varchar)).Value = _Parametros.dt_fechafin;
                    Comm1.Parameters.Add(new NpgsqlParameter("@comando", NpgsqlDbType.Varchar)).Value = _Parametros.var_comando;
                    NpgsqlDataReader reader = Comm1.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ParametrosEventos dt = new ParametrosEventos();
                            dt.int_id = (int)reader["id"];
                            dt.var_nombre = (string)reader["nombre"];
                            dt.var_matricula = (string)reader["matricula"];
                            dt.var_comando = (string)reader["comando"];
                            dt.var_valor = (string)reader["valor"];
                            dt.dt_modificacion = Convert.ToDateTime(reader["fecha"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                            Lista.Add(dt);
                        }
                        Objeto.Estado = 1;
                        Objeto.Mensaje = "Ok";
                        Conexion.Close();
                    }
                }
                else
                {
                    Objeto.Estado = -1;
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
        public class ParametrosEventos 
        {
            public int int_id { get; set; }
            public string var_matricula { get; set; }
            public string var_nombre { get; set; }
            public string var_comando { get; set; }
            public string var_valor { get; set; }
            public string dt_modificacion { get; set; }
        }

        public class ParametrosPostEventos
        {
            public int? int_id { get; set; }
            public int? int_idunidad { get; set; }
            public int? int_comando { get; set; }
            public string? var_comando { get; set; }
            public string? dt_fechaini { get; set; }
            public string? dt_fechafin { get; set; }
            public string? var_busqueda { get; set; }

        }

        public class ParametroGeneral
        {
            public int IdUsuario { get; set; }
            public string Token { get; set; }
        }

    }
}
