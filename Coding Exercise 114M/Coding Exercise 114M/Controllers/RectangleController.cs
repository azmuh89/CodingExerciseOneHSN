using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Coding_Exercise_114M.Controllers
{
    [ApiController]
    public class RectangleController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly string _connectionString;

        public RectangleController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Database");
        }

        [HttpPost]
        [Route("api/rectangle/search")]
        public IActionResult SearchCoordinates(List<int[]> coordinates)
        {
            List<string> matches = new List<string>();

            using (var cnn = new SqlConnection(_connectionString))
            {
                foreach (int[] coordinate in coordinates)
                {
                    int x = coordinate[0];
                    int y = coordinate[1];

                    string sql = $@"Select  Name
                        From    dbo.Rectangle r
                                Inner Join dbo.RectangleCoordinates rc on rc.RectangleId = r.Id
                        Where   {x} >= rc.X
                                And {x} <= rc.X + r.Width
                                And {y} >= rc.Y
                                And {y} <= rc.Y + r.Height";

                    var result = cnn.Query<string>(sql);

                    if (result.Count() > 0)
                        matches.Add($"Coordinate {x}, {y} falls into rectangles: {string.Join(",", result)}");
                    else
                        matches.Add($"Coordinate {x}, {y} does not fall into any rectangles.");
                }
            }    

            return Ok(matches);
        }
    }
}
