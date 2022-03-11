using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestFullPM012022.Models;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace RestFullPM012022.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmpleController(IConfiguration configuracion, IWebHostEnvironment env)
        {
            _configuration = configuracion;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            String consulta = @"select id, nombre, apellidos, edad from empleados";
            string StringConexion = _configuration.GetConnectionString("EmpleCon");
            MySqlDataReader lector;
            DataTable tabla = new DataTable();

            using (MySqlConnection conexion = new MySqlConnection(StringConexion))
            {
                conexion.Open();

                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    lector = comando.ExecuteReader();
                    tabla.Load(lector);
                    lector.Close();
                    conexion.Close();
                }
            }

            return new JsonResult(tabla);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            String consulta = @"select id, nombre, apellidos, edad from empleados 
                               where id = @id";
            string StringConexion = _configuration.GetConnectionString("EmpleCon");
            MySqlDataReader lector;
            DataTable tabla = new DataTable();

            using (MySqlConnection conexion = new MySqlConnection(StringConexion))
            {
                conexion.Open();

                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    lector = comando.ExecuteReader();
                    tabla.Load(lector);
                    lector.Close();
                    conexion.Close();
                }
            }

            return new JsonResult(tabla);
        }

        [HttpPost]
        public JsonResult Post(Empleado emple)
        {
            String consulta = @"insert into empleados(nombre, apellidos, edad) 
                                values (@nombre, @apellidos, @edad)";

            string StringConexion = _configuration.GetConnectionString("EmpleCon");
            MySqlDataReader lector;
            DataTable tabla = new DataTable();

            using (MySqlConnection conexion = new MySqlConnection(StringConexion))
            {
                conexion.Open();

                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", emple.nombre);
                    comando.Parameters.AddWithValue("@apellidos", emple.apellidos);
                    comando.Parameters.AddWithValue("@edad", emple.edad);


                    lector = comando.ExecuteReader();
                    tabla.Load(lector);
                    lector.Close();
                    conexion.Close();
                }
            }

            return new JsonResult("Agregado con exito");
        }

    }
}
