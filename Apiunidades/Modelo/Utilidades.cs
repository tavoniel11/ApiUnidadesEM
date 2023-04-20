using Newtonsoft.Json.Linq;
using Npgsql;
using System.IO;
using System.Reflection;
using System.Text;

namespace Apiunidades.Modelo
{
    public class Utilidades
    {
        public static string RutaConfiguracion = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        public static string NombreServicio = "Api Unidades";
        #region Variables
        public static string _Sv;
        public static string _Pt;
        public static string _Us;
        public static string _Ps;
        public static string _Db;
        public static string[] encabezados;
        public static string UrlPat;
        #endregion

        public static NpgsqlConnection ObtenerConexion()
        {
            NpgsqlConnection Conexion = null;
            try
            {
                ObtenerSettings();
                string _conecction = _Sv + _Pt + _Us + _Ps + _Db;
                Conexion = new NpgsqlConnection(_conecction);

                if (Conexion == null)
                {
                    throw new ArgumentNullException(paramName: MethodBase.GetCurrentMethod().Name, message: $"Conexion Nula");
                }

            }
            catch
            {
                throw;
            }
            return Conexion;
        }

        #region "------------+Logs+-------------"

        public static void RegistrarError(string _NombreServicio, Exception _ObjException, MethodBase _ObjMethodBase, string _NombreArchivo = "", string _Query = "", NpgsqlConnection _Conexion = null, bool _Enviar = true)
        {



            try
            {
                string CadenaConexion = string.Empty;
                if (_Conexion != null)
                    CadenaConexion = _Conexion.ConnectionString;

                if (_Enviar)
                {

                    _NombreArchivo = GetNameFile("Error");



                    string Archivo = string.Empty;
                    try
                    {
                        LogErrorCsv(_ObjException, _ObjMethodBase, _NombreArchivo, _Query, CadenaConexion);
                    }
                    catch { }


                }
            }
            catch { }
        }
        public static string CrearDirectorio()
        {
            try
            {
                ObtenerSettings();
                if (!Directory.Exists(UrlPat))
                {
                    Directory.CreateDirectory(UrlPat + "LogTxt/");
                    Directory.CreateDirectory(UrlPat + "LogCsv/");
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);
            }
            return UrlPat;
        }
        public static string GetNameFile(string _TipoLog)
        {
            string Nombre = "";
            string FechaArchivo = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day;

            if (_TipoLog == "Error")
                Nombre = $"Log_{_TipoLog}-" + FechaArchivo;

            if (_TipoLog == "Datos")
                Nombre = $"Log_{_TipoLog}-" + FechaArchivo;

            if (_TipoLog == "Servicio")
                Nombre = $"Log_{_TipoLog}-" + FechaArchivo;

            return Nombre;
        }
        #endregion

        #region ARCHIVO TXT

        public void LogError(string _LogEstado, bool _Activo)
        {

            string ruta = GetNameFile("Error");

            if (_Activo is true)
            {
                string cadena = "-";
                string Pat = (CrearDirectorio() + "LogTxt/");
                using (StreamWriter sw = new StreamWriter(Pat + ruta + ".txt", true))
                {
                    cadena += DateTime.Now + _LogEstado + Environment.NewLine;

                    sw.WriteLine(cadena);
                    sw.Close();
                }
                _Activo = false;
            }
        }
        public void LogDatos(string _LogEstado, bool _Activo)
        {

            string ruta = GetNameFile("Datos");
            if (_Activo is true)
            {
                string cadena = "-";
                string Pat = (CrearDirectorio() + "LogTxt/");
                using (StreamWriter sw = new StreamWriter(Pat + ruta + ".txt", true))
                {
                    cadena += DateTime.Now + _LogEstado + Environment.NewLine;

                    sw.WriteLine(cadena);
                    sw.Close();
                }
                _Activo = false;
            }

        }
        #endregion

        #region ARCHIVO EXCEL

        public static void LogErrorCsv(Exception _ObjExcepcion, MethodBase _ObjMetodoBase, string _NombreArchivo, string _Query = "", string _CadenaConexion = "")
        {

            string rutaCsv = (CrearDirectorio() + "LogCsv/" + _NombreArchivo + ".csv");

            encabezados = new string[4] { "Fecha", "Clase", "Funcion", "Error" };

            string funcion = _ObjMetodoBase.Name;
            string clase = _ObjMetodoBase.ReflectedType.Name;
            string coma = ",";
            string fecha = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss");

            string texto = string.Empty;
            if (!string.IsNullOrEmpty(_ObjExcepcion.Message))
            {
                texto = _ObjExcepcion.Message.Replace("\r", "").Replace("\n", "");
            }

            using (StreamWriter streamWriter = new StreamWriter(rutaCsv, append: true))
            {
                streamWriter.WriteLine(string.Join(coma, encabezados));
                string[] Datos = { fecha, clase, funcion, "\"" + _ObjExcepcion + "\"" };
                streamWriter.WriteLine(string.Join(coma, Datos));
                streamWriter.ToString();
                streamWriter.Close();

            }

        }
        public void LogDatosCsv(Exception _ObjExcepcion, MethodBase _ObjMetodoBase, string _NombreArchivo, string _Query = "", string _CadenaConexion = "")
        {
            string ruta = GetNameFile("Datos");
            string rutaCsv = (CrearDirectorio() + "LogCsv/" + ruta + ".csv");

            encabezados = new string[4] { "Fecha", "Clase", "Funcion", "Error" };

            string funcion = _ObjMetodoBase.Name;
            string clase = _ObjMetodoBase.ReflectedType.Name;
            string coma = ",";
            string fecha = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss");

            string texto = string.Empty;
            if (!string.IsNullOrEmpty(_ObjExcepcion.Message))
            {
                texto = _ObjExcepcion.Message.Replace("\r", "").Replace("\n", "");
            }

            using (StreamWriter streamWriter = new StreamWriter(rutaCsv, append: true))
            {
                streamWriter.WriteLine(string.Join(coma, encabezados));
                string[] Datos = { fecha, clase, funcion, "\"" + _ObjExcepcion + "\"" };
                streamWriter.WriteLine(string.Join(coma, Datos));
                streamWriter.ToString();
                streamWriter.Close();

            }
        }

        #endregion
        public static void ObtenerSettings()
        {
            if (File.Exists(RutaConfiguracion))
            {
                var DatosJson = File.ReadAllText(RutaConfiguracion);
                JObject obj = JObject.Parse(DatosJson);

                IEnumerable<JProperty> enumerable = obj.Properties();

                foreach (JProperty _item in enumerable)
                {
                    if (_item.Name == "AppSettings")
                    {
                        JObject _obj = JObject.Parse(_item.Value.ToString());
                        IEnumerable<JProperty> enumerable2 = _obj.Properties();
                        foreach (JProperty item in enumerable2)
                        {
                            if (item.Name == "PAT")
                            {
                                UrlPat = item.Value.ToString();
                            }
                            if (item.Name == "SV")
                            {
                                _Sv = item.Value.ToString();
                            }
                            if (item.Name == "PT")
                            {
                                _Pt = item.Value.ToString();
                            }
                            if (item.Name == "US")
                            {
                                _Us = item.Value.ToString();
                            }
                            if (item.Name == "PS")
                            {
                                _Ps = item.Value.ToString();
                            }
                            if (item.Name == "DB")
                            {
                                _Db = item.Value.ToString();
                            }
                        }
                    }

                }
            }
        }
    }
}
