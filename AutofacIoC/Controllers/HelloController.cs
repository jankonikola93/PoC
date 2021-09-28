using System.ComponentModel.DataAnnotations;
using Autofac.Core.Registration;
using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Mvc;

namespace AutofacIoC
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        IIndex<string, IService> services;
        public HelloController(IIndex<string, IService> services)
        {
            this.services = services;
        }

        [HttpGet]
        public ActionResult<string> Hello([Required] string serviceName)
        {
            try
            {
                 var service = services[serviceName];
                 if (service == null)
                 {
                     return BadRequest("Service not found");
                 }

                 return Ok(service.SayHello());
            }
            catch (ComponentNotRegisteredException){
                return BadRequest("Component not registered");
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }
}