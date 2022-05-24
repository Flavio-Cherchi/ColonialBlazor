using Colonial.Shared.SharedModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Colonial.Server.Controllers
{
    
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpPost]
        [Route("testing/create")]
        public void Insert([FromBody] TestObject player)
        {

            string QueryStr = @"INSERT INTO [col].[t_a_player] ([t_name]
                                                               ,[i_age]
                                                               ,[i_level])
                                                         VALUES
                                                               ('" + player.TName + "'," 
                                                               + player.IAge + "," 
                                                               + player.ILevel + ")";
            Exec(QueryStr);
        }

        [HttpGet]
        [Route("testing/read/{id}")]
        public IEnumerable<TestObject> Read(int id)
        {
            string QueryStr = "SELECT * from col.t_a_player";
            QueryStr += (id != 0) ? " where i_id = " + id : "";
            DataSet Data = Read(QueryStr);
            IEnumerable<TestObject> Res = Data.Tables[0].AsEnumerable().Select(r => new TestObject
            {
                IId = r.Field<int>("i_id"),
                IAge = r.Field<int>("i_age"),
                ILevel = r.Field<int>("i_level"),
                TName = r.Field<string>("t_name")

            }).ToList();
            return Res;
        }

        [HttpPut]
        [Route("testing/update")]
        public void Update(TestObject player)
        {
            string QueryStr = @"UPDATE [col].[t_a_player] SET [t_name] = '" + player.TName + "',[i_age] = " + player.IAge + ",[i_level] = " + player.ILevel + " WHERE i_id = " + player.IId;
            Exec(QueryStr);
        }

        [HttpDelete]
        [Route("testing/delete/{id}")]
        public void Delete(int id)
        {
            string QueryStr = @"DELETE FROM [col].[t_a_player] WHERE i_id = " + id;
            Exec(QueryStr);
        }
    }
}