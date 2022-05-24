using Colonial.Server.Models;
using Colonial.Shared;
using Colonial.Shared.SharedModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Colonial.Server.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string mainTable = " [account].[Users] ";

        public UserController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpPost]
        [Route("user/create")]
        public void Insert([FromBody] User u)
        {
            string QueryStr = @"INSERT INTO " + mainTable;
            QueryStr += @" (
                            [UserName]
                           ,[Email]
                           ,[Password]
                           ,[DateCreation]
                           ,[DateModification])
                     VALUES
                           (
                            '" + u.UserName + "', '" + 
                            u.Email + "','" +
                            u.Password + "'," +
                            "getdate(), getdate())";

            Exec(QueryStr);
        }

        [HttpPost]
        [Route("user/currentUser")]
        public User CurrentUser([FromBody] User u)
        {
            try
            {
                return SearchUsers(u).First();
            }
            catch (Exception)
            {
                return new User();
            }
            
        }

        [HttpPost]
        [Route("user/readUser")]
        public IEnumerable<User> ReadUser([FromBody] User u)
        {
            return SearchUsers(u);
        }

        [NonAction]
        public IEnumerable<User> SearchUsers(User u)
        {
            string QueryStr = "SELECT * FROM " + mainTable + " WHERE 1 = 1 ";
            QueryStr += (u.Id != 0 && u.Id != null) ? " AND [id] = '" + u.Id + "'" : "";
            QueryStr += (!string.IsNullOrEmpty(u.UserName)) ? " AND [Username] = '" + u.UserName + "'" : "";
            DataSet Data = (DataSet)Read(QueryStr);
            IEnumerable<User> Res = Data.Tables[0].AsEnumerable().Select(u => new User
            {
                Id = u.Field<int>("id"),
                UserName = u.Field<string>("UserName"),
                Email = u.Field<string>("Email"),
            }).ToList();
            return Res;
        }

        [HttpPut]
        [Route("user/update")]
        public void Update(User u)
        {
            string QueryStr = @"UPDATE " + mainTable  + " SET [UserName] = '" + u.UserName + "'";
            QueryStr += @" WHERE ID = '" + u.Id + "'";
            Exec(QueryStr);
        }

        [HttpDelete]
        [Route("user/delete/{id}")]
        public void Delete(int id)
        {
            string QueryStr = @"DELETE FROM " + mainTable  + " WHERE id = " + id;
            Exec(QueryStr);
        }
    }
}