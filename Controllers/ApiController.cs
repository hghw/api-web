using api_web.Controllers.Helpers;
using api_web.Module.MainPage.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Dapper.FastCrud;
using System.Collections.Generic;

namespace api_web.Controllers
{
    [ApiController]
    [Route("api/location")]
    public class ApiController : ControllerBase
    {

        private readonly ILogger<ApiController> _logger;

        private readonly EF_DataContext _context;
        public ApiController(EF_DataContext context)
        {
            _context = context;
        }

        [HttpGet("list")]
        public RestData List()
        {
            try
            {
                var list = _context.Locations.ToList();
                return new RestData(status: "OK", data: list);
            }
            catch (Exception e)
            {
                return new RestData(status: e.Message, data: null);
            }

        }
        [HttpGet("getItem")]
        public RestData GetItem(int id)
        {
            try
            {
                Location response = new Location();
                var row = _context.Locations.Where(d => d.id.Equals(id)).FirstOrDefault();

                if (row == null) return new RestData(status: "ERROR", data: null);

                return new RestData(status: "OK", data: row);
            }
            catch (Exception e)
            {
                return new RestData(status: e.Message, data: null);
            }
        }
        [HttpPost("add")]
        public RestData SaveOrder(Location item)
        {
            try
            {
                Location dbTable = new Location();
                if (item.id > 0)
                {
                    //PUT
                    dbTable = _context.Locations.Where(d => d.id.Equals(item.id)).FirstOrDefault();
                    if (dbTable != null)
                    {
                        dbTable.lat = item.lat;
                        dbTable.lng = item.lng;
                        dbTable.user_id = item.user_id;
                    }
                }
                else
                {
                    //POST
                    dbTable.lat = item.lat;
                    dbTable.lng = item.lng;
                    dbTable.user_id = item.user_id;
                    _context.Locations.Add(dbTable);
                }
                _context.SaveChanges();
                return new RestData(status: "OK", data: null);
            }
            catch (Exception e)
            {
                return new RestData(status: e.Message, data: null);
            }


        }
    }
}