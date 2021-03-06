using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DAL;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string ConString = @"
   Server=127.0.0.1,1433;
   Database=apbd;
   User Id=sa;
   Password=yourStrong(!)Password
";
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        
        
        [HttpGet]
        public IActionResult GetStudents([FromServices] IDbService dbService)
        {
            var list = new List<Students>();
            
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select * from Student";
                con.Open();
                var dr = com.ExecuteReader();
                while(dr.Read())
                {
                    var st = new Students();
                    st.IndexNumer = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    
                    list.Add(st);
                }
            }

            return Ok(list);
        }
        
        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student where IndexNumber=@index";
                com.Parameters.AddWithValue("index", indexNumber);

                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Students();
                    st.IndexNumer = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    return Ok(st);
                }

            }

            return NotFound();
        }
        [HttpPost]
        public IActionResult CreateStudent(Students student)
        {
            student.IndexNumer = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok($"Student o id:{id} zostal usunięty");
        }

        [HttpPut("{id}")]
        public IActionResult AddStudentPhoto(int id)
        {
            return Ok($"Aktualizacja zdjęcia dla studenta o id={id} została zakończona");
        }
        
    }
}