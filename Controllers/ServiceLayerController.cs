using B1SLayer;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SL_Connector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceLayerController : ControllerBase
    {
        SLConnection _serviceLayer;

        public ServiceLayerController(SLConnection serviceLayer)
        {
            _serviceLayer = serviceLayer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string controller, int id)
        {
            return Ok(await _serviceLayer.Request($"{controller}{(id > 0 ? $"({id})" : "")}").GetAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(string controller, [FromBody] object value)
        { 
            try 
	        {	        
		        JObject json = JObject.Parse(JsonConvert.SerializeObject(JObject.Parse(value.ToString() ?? "")));

                return Ok(await _serviceLayer.Request($"{controller}").PostAsync<IDictionary<string, dynamic>>(json));
            }
	        catch (Exception ex)
	        {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string controller, int id, [FromBody] object value)
        {
            try
            {
                JObject json = JObject.Parse(JsonConvert.SerializeObject(JObject.Parse(value.ToString() ?? "")));

                await _serviceLayer.Request($"{controller}({id})").PatchAsync(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string controller, int id, [FromBody] object value)
        {
            try
            {
                JObject json = JObject.Parse(JsonConvert.SerializeObject(JObject.Parse(value.ToString() ?? "")));

                await _serviceLayer.Request($"{controller}({id})").PutAsync(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }
    }
}
