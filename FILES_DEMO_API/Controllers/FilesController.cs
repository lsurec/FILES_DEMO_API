using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading;

namespace FILES_DEMO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {


        //api para subir archvios, el api soporta mas de un archivo a la vez
        [HttpPost]
        //para el limite de tamaño de los archivos.
        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        //Archivos sin limite de peso
        [DisableRequestSizeLimit]

        public async Task<IActionResult> PostFiles(
            [FromHeader] string urlCarpeta
            )
        {

            // Obtener la ruta del directorio del proyecto
            //.Parent busca la dreccion un nivel arriba, validar que las subcarpetas sean correctas
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

            //Url donde está alojado el proyecto
            string url = ObtenerRutaBase(urlCarpeta);

            //validar si se encontró la url base
            if (string.IsNullOrEmpty(url))
            {
                return NotFound("Url invalida (save)");
            } 

            //Files del body del ai¿pi
            var files = Request.Form.Files;

            //verificar que se hayan seleccionado archivos
            if (files == null || files.Count == 0)
            {   
                //si no hay archivos
                return NotFound("No hay archivos para procesar");
            }

            try
            {
                //recorrer los archivos seleccionados
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //Nuevo nombre para el archvio
                        string newName = GenerarNuevoNombre(file.FileName);

                        //armar la url de la imagen 
                        var filePath = projectDirectory + url + newName;

                        //abirir stream para la escritura del archivo 
                        using var stream = new FileStream(filePath, FileMode.Create);

                        //guardar archivo 
                        await file.CopyToAsync(stream);

                        //TODO: Uso de prodedimiento almacenado para guardar nombre antiguo y nombre nuevo del archivo
                        //file.FileName; Nombre del  archivo
                        //newName; nuevo Nombre del archivo 
                    }
                    
                }

                //"Archivos cargados exitosamente."
                return Ok("Archivo Procesado exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        //Generar nuevo nombre para los archvios
        private static string GenerarNuevoNombre(string archivo)
        {
            string[] array = archivo.Split('.'); //Separar el nombre del archivo segun los puntos (.)
            string extension = array[^1]; //usar el ultimo indice (extensión del archivo)
            string uuid = Guid.NewGuid().ToString("N"); //generar aeatoriamente id nuevo
            string timestamp = DateTime.Now.ToString("yyyyMMddHHss"); // Obtiene la marca de tiempo actual
            string nuevoArchivo = $"{uuid}_{timestamp}.{extension}"; //nombre final del archivo 
            return nuevoArchivo; //retornar nombre
        }

        static string ObtenerRutaBase(string url)
        {
            try
            {

                // Eliminar la parte inicial http: //192.168.0.23:4050 o dominio
                Uri uri = new(url);

                string rutaSinHost = uri.PathAndQuery; // Esto incluye el path y la query string

                // Encontrar la última barra '/'
                int ultimaBarraIndex = rutaSinHost.LastIndexOf('/');

                // Extraer la ruta base
                string rutaBase = rutaSinHost[..(ultimaBarraIndex + 1)]; // +1 para incluir la barra


                return rutaBase;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
