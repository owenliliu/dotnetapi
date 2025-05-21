using Microsoft.AspNetCore.Mvc;
using DotnetApi.Dtos;
using DotnetApi.Models;
using DotnetApi.Data;
using DotNetApi.Data;
using AutoMapper;
using DotNetAPI.Data;
namespace DotNetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;
    IUserRepository _userRepository;
    IMapper _mapper;
    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _entityFramework = new DataContextEF(config);
        _userRepository = userRepository;
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
        }));
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        User? user = _entityFramework.Users
        .Where(u => u.UserId == userId)
        .FirstOrDefault<User>();

        if (user != null)
        {
            return user;
        }
        throw new Exception("Failed to Get User");
    }

    [HttpGet("GetJobInfo/{userId}")]
    public UserJobInfo GetJobInfo(int userId)
    {
        UserJobInfo? user = _entityFramework.UserJobInfo
        .Where(u => u.UserId == userId)
        .FirstOrDefault<UserJobInfo>();

        if (user != null)
        {
            return user;
        }
        throw new Exception("Failed to Get User");
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        
        
        User? userDb = _entityFramework.Users
        .Where(u => u.UserId == user.UserId)
        .FirstOrDefault<User>();

        if (user != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
        }
        throw new Exception("Failed to Edit User");
    }

    [HttpPost]
    public IActionResult AddUser(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);

        
        userDb.Active = user.Active;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.Email = user.Email;
        userDb.Gender = user.Gender;
        _userRepository.AddEntity<User>(userDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
    
    throw new Exception("Failed to Add User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
       User? userDb = _entityFramework.Users
        .Where(u => u.UserId == userId)
        .FirstOrDefault<User>();

        if (userDb != null)
        {
            _entityFramework.Users.Remove(userDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
        }
        throw new Exception("Failed to Delete User");
    }
}
