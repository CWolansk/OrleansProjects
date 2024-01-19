using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Project1.Grains;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlShortenerController
    {
        private readonly IGrainFactory _grains;

        public UrlShortenerController(IGrainFactory grains) 
        {
            _grains = grains;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IResult> Get(string url)
        {

            // Create a unique, short ID
            var shortenedRouteSegment = Guid.NewGuid().GetHashCode().ToString("X");

            // Create and persist a grain with the shortened ID and full URL
            var shortenerGrain = _grains.GetGrain<IUrlShortenerGrain>(shortenedRouteSegment);

            await shortenerGrain.SetUrl(url);

            return Results.Ok(await shortenerGrain.GetUrl());
        }

    }
}
