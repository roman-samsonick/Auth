using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authenticate {
  public class UserDto {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class LoginDto {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class SetBlockDto {
    public string[] Users { get; set; }
    public bool Blocked { get; set; }
  }
  
  public class DeleteDto {
    public string[] Users { get; set; }
  }

  public class CreateTokenDto {
    public string Token { get; set; }
    public User User { get; set; }
  }

  [Authorize]
  [Route("user")]
  public class AuthenticationController : Controller {
    private readonly SignInManager<User> _signInManager;
    private readonly ApplicationContext _applicationContext;
    private readonly JwtService _jwtService;

    public AuthenticationController(
      SignInManager<User> signInManager,
      ApplicationContext applicationContext,
      JwtService jwtService
    ) {
      _signInManager = signInManager;
      _applicationContext = applicationContext;
      _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto) => Ok(
      await _signInManager.UserManager.CreateAsync(
        new User {
          Email = userDto.Email,
          EmailConfirmed = true,
          Blocked = false,
          Created = DateTime.Now,
          Name = userDto.Name,
          UserName = userDto.Email,
        },
        userDto.Password
      )
    );

    
    [AllowAnonymous]
    [HttpPost("create-token")]
    public async Task<IActionResult> CreateToken([FromBody]LoginDto userDto) {
      var result = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password, true, false);

      if (result.Succeeded) {
        var user = await _signInManager.UserManager.FindByEmailAsync(userDto.Email);

        user.LastLogin = DateTime.Now;

        await _signInManager.UserManager.UpdateAsync(user);

        return Ok(new CreateTokenDto {
          Token = _jwtService.CreateJwt(user.Id),
          User = user
        });
      }

      return Unauthorized();
    }
    

    [HttpPost("set-block")]
    public async Task<IActionResult> SetBlock([FromBody]SetBlockDto setBlockDto) {
      var users = await _applicationContext.Users.Where(user => setBlockDto.Users.Contains(user.Id))
        .ToListAsync();
      
      users.ForEach(user => {
        user.Blocked = setBlockDto.Blocked;
      });
      
      _applicationContext.Users.UpdateRange(users);

      await _applicationContext.SaveChangesAsync();

      return Ok();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteUsers([FromBody] DeleteDto deleteDto) {
      var users = await _applicationContext.Users.Where(user => deleteDto.Users.Contains(user.Id))
        .ToListAsync();
      
      _applicationContext.Users.RemoveRange(users);

      await _applicationContext.SaveChangesAsync();

      return Ok();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers() => Ok(
      await _applicationContext.Users.ToListAsync()
    );
  }
}