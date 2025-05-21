using Microsoft.AspNetCore.Mvc;
using DotnetApi.Dtos;
using DotnetApi.Models;
using DotnetApi.Data;
namespace DotNetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }
    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
        SELECT  [UserId]
        , [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
  FROM  TutorialAppSchema.Users;";
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
    }
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sql = @"
        SELECT  [UserId]
        , [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
  FROM  TutorialAppSchema.Users
  WHERE UserID = 
  " + userId.ToString();
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }

    [HttpGet("GetSalay/{userId}")]
    public IEnumerable<UserSalary> GetUserSalary(int userId)
    {
        string sql = @"
        SELECT  UserSalary.UserId
        , UserSalary.Salary
        , UserSalary.AverageSalary
  FROM  TutorialAppSchema.UserSalary
  WHERE UserID = 
  " + userId.ToString();
        return _dapper.LoadData<UserSalary>(sql);
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users
                SET  [FirstName] = '" + user.FirstName + @"'
                    , [LastName] = '" + user.LastName + @"'
                    , [Email] = '" + user.Email + @"'
                    , [Gender] = '" + user.Gender + @"'
                    , [Active] = ' " + user.Active + @" '
         WHERE UserId = " + user.UserId + @"
        ";
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to Update User");
    }


    [HttpPut("EditUsersalary")]
    public IActionResult EditUserSalary(UserSalary userSalaryforUpdate)
    {
        string sql = @"
        UPDATE TutorialAppSchema.UserSalary
                SET  Salary = '" + userSalaryforUpdate.Salary + @" '
         WHERE UserId = " + userSalaryforUpdate.UserId + @"
        ";
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to Update User");
    }


    [HttpPost]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users(
        [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
) VALUES ( '" +
      user.FirstName + @"'
                    , '" + user.LastName + @"'
                    , '" + user.Email + @"'
                    , '" + user.Gender + @"'
                    , ' " + user.Active + @" '
         )";
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to Add User");
    }

    [HttpPost("UserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalarytoAdd)
    {
        string sql = @"INSERT INTO TutorialAppSchema.UserSalary(
        UserId,
        Salary
) VALUES ( '" +
      userSalarytoAdd.UserId + @"'
        , '" + userSalarytoAdd.UserId + @" '
         )";
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to Add User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
        Delete FROM TutorialAppSchema.Users
            Where UserId = " + userId;
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to Delete User");
    }
    
    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = @"
        Delete FROM TutorialAppSchema.UserSalary
            Where UserId = " + userId;
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to Delete User Salary");
    }
}
