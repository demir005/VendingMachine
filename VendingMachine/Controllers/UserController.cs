﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VendingMachine.Models;
using VendingMachine.Models.Dto;
using VendingMachine.Services;

namespace VendingMachine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly ILogger<UserController> _logger;


        public UserController(IUserServices userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            try
            {
                var user = await _userService.GetUserById(Id);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                if (!result)
                    return NotFound();

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserModel model)
        {
            try
            {
                var result = await _userService.UpdateUser(id, model);
                if (!result)
                    return NotFound();

                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, message) = await _userService.Login(model);
                if (status == 0)
                    return BadRequest(message);
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, message) = await _userService.Registeration(model);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return CreatedAtAction(nameof(Register), model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost("deposit")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> DepositCoin([FromBody] DepositRequestDTO depositModel)
        {
            // Retrieve current user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"User ID: {userId}");
            Console.WriteLine($"ClaimTypes: {ClaimTypes.NameIdentifier}");


            // Validate coin value
            if (!IsValidCoin(depositModel.CoinValue))
                return BadRequest(new { error = "Invalid coin value" });

            // Perform deposit
            var depositResult = await _userService.DepositCoin(userId, depositModel.CoinValue);

            if (depositResult)
                return Ok(new { message = "Deposit successful" });
            else
                return BadRequest(new { error = "Failed to deposit. Please try again." });
        }

        [Authorize]
        [HttpPost("logout/all")]
        public async Task<IActionResult> LogoutAllSessions()
        {
            // Get the user ID from the authenticated user
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var success = await _userService.LogoutAllSessions(userId);

            if (!success)
            {
                return NotFound();
            }
            return Ok("All sessions invalidated successfully");
        }

        [HttpPost("reset")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> ResetDeposit()
        {
            // Retrieve current user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var resetResult = await _userService.ResetDeposit(userId);
            if (resetResult)
                return Ok(new { message = "Deposit reset successful" });
            else
                return BadRequest(new { error = "Failed to reset deposit. Please try again." });
        }
        private bool IsValidCoin(int coinValue)
        {
            // Validate against your allowed coin values (5, 10, 20, 50, 100)
            var validCoinValues = new List<int> { 5, 10, 20, 50, 100 };
            return validCoinValues.Contains(coinValue);
        }
    }
}
