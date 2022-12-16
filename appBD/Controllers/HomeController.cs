using appBD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;

namespace appBD.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;

        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.users.ToListAsync());
        }

        //ПОЛУЧЕНИЕ ВСЕХ ПОЛЬЗОВАТЕЛЕЙ

        [HttpGet("~/api/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()//получение всех пользователей 
        {
            var _users = db.users.ToList();
            if (_users != null)
            {
                return Ok(_users);
            }
            return BadRequest();
        }

        [HttpGet("~/api/GetAllUsersSQL")]
        public async Task<IActionResult> GetAllUsersSQL()//получение всех пользователей через SQL запрос
        {
            var _users = db.users.FromSqlRaw("SELECT * FROM users;").ToList();

            if (_users != null)
            {
                return Ok(_users);
            }
            return BadRequest();
        }

        //ПОИСК ПОЛЬЗОВАТЕЛЯ ПО ID
        [HttpGet("~/api/GetUser_id")]
        public async Task<IActionResult> GetUser_id([FromBody] Id ID)//поиск пользователя по id 
        {
            User? user = await db.users.FirstOrDefaultAsync(u => u.id == ID.id);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpGet("~/api/GetUser_idSQL")]// Поиск пользователя по id с помощью SQL запроса 
        public async Task<IActionResult> GetUser_idSQL([FromBody] Id ID)//поиск пользователя по id 
        {
            List<User> user = db.users.FromSqlRaw($"SELECT * FROM users WHERE id = {ID.id};").ToList();
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        //ПОЛУЧЕНИЕ СПИСКА ПОЛЬЗОВАТЕЛЕЙ С ОПРЕДЕЛЁННЫМ УСЛОВИЕМ (У КОГО НЕТ ПОЧТЫ)
        [HttpGet("~/api/GetUserWithoutEmail")]// получение пользователей без почты 
        public async Task<IActionResult> GetUserWithoutEmail()
        {
            var _users = db.users.FromSqlRaw($"SELECT * FROM users WHERE email IS NULL;").ToList();
            if (_users != null)
            {
                return Ok(_users);
            }
            return BadRequest();
        }

        [HttpGet("~/api/GetUserWithoutEmailSQL")]// получение пользователей без почты с использованием SQL
        public async Task<IActionResult> GetUserWithoutEmaiSQL()
        {
            var _users = db.users.Where(u => u.email == null).ToList();
            if (_users != null)
            {
                return Ok(_users);
            }
            return BadRequest();
        }

        //ДОБАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯ
        [HttpPost("~/api/AddUserSQL")]//добавление пользователя с помощью SQL
        public async Task<IActionResult> PostAddUserSQL([FromBody] User? user)
        {
            if (user != null)
            {
                string _date = $"{user.date_of_birth.Value.Year}-{user.date_of_birth.Value.Month}-{user.date_of_birth.Value.Day}";
                db.Database.ExecuteSqlRaw($"INSERT INTO users (first_name, last_name, email, gender, date_of_birth) VALUES ('{user.first_name}', '{user.last_name}', '{user.email}', '{user.gender}', '{_date}');");
                db.SaveChanges();

                return Ok();
            }
            return BadRequest();
        }

        //УДАЛЕНИЕ ПОЛЬЗОВАТЕЛЯ ПО ID 
        [HttpDelete("~/api/DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody]Id ID)
        {
            if (ID != null)
            {
                var del_user = db.users.Where(u => u.id == ID.id);
                foreach(var u in del_user)
                {
                    db.users.Remove(u);
                }
                db.SaveChanges();

                return Ok();
            }
            return BadRequest();
        }

        //РЕДАКТИРОВАНИЕ ПОЛЬЗОВАТЕЛЯ ПО ID !!!ДОДЕЛАТЬ!!!
        [HttpPut("~/api/EditUser")]
        public async Task<IActionResult> PutEditUser([FromBody]Name_Id name_Id)
        {
            if (name_Id!= null)
            {
                var edit_user = db.users.Where(u => u.id == name_Id.id);
                foreach (var u in edit_user)
                {
                    u.first_name = name_Id.name;
                    db.SaveChanges();
                }

                return Ok(edit_user);
            }
            return BadRequest();
        }

    }
}