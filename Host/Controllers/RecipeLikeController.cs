using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Host.Controllers
{

    [Authorize]
    public class RecipeLikeController : Controller
    {
        private readonly IRecipeLikeService _recipeLikeService;

        public RecipeLikeController(
            IRecipeLikeService recipeLikeService)
        {
            _recipeLikeService = recipeLikeService;
        }

        [HttpPost]
        public async Task<IActionResult> Like([FromBody] RecipeLikeRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var response = await _recipeLikeService.LikeAsync( userId,request);

            if (!response.Status)
            {
                return BadRequest(new
                {
                    status = false,
                    message = response.Message
                });
            }

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Unlike([FromBody] RecipeLikeRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var response = await _recipeLikeService.UnlikeAsync(userId,request);

            if (!response.Status)
            {
                return BadRequest(new
                {
                    status = false,
                    message = response.Message
                });
            }

            return Ok(response);
        }
    }
}
