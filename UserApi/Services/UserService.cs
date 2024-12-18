using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Entities;
using UserApi.Models.Users;

namespace UserApi.Services;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid id, bool isIncludeDatas = false);
    Task<RegisterResponse> RegisterAsync(Models.Users.RegisterRequest request);
    Task<Guid> UpdateAsync(UserDto userDto);
    Task DeleteAsync(Guid id);
}
public class UserService : IUserService
{
    private readonly UserDbContext _context;
    private readonly PasswordHasher<string> _passwordHasher;
    public UserService(UserDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<string>();
    }


    public async Task<RegisterResponse> RegisterAsync(Models.Users.RegisterRequest request)
    {
        var response = new RegisterResponse();
        User user = await _context.Users.FirstOrDefaultAsync(x => x.Pin == request.Pin);

        if (user is not null)
        {
            response.Description = "you are already registered!";
            return response;
        }

        user = new User
        {
            Pin = request.Pin,
            Fio = request.Fio,
            Password = _passwordHasher.HashPassword(null, request.Password)
        };
       
        
        if (request.Parent is not null)
            await UpdateOrAddUserParent(request.Parent, user);

        if (request.Children.Count > 0)
            await UpdateOrAddUserChildren(request.Children, user);

        if (request.Roles.Count > 0)
            await UpdateOrAddRoles(request.Roles, user);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        response = new RegisterResponse
        {
            Pin = user.Pin,
            Fio = user.Fio,
            Description = "Registration success"
        };

        return response;
    }
    public async Task<UserDto> GetByIdAsync(Guid id, bool isIncludeDatas = false)
    {
        var user =await _context.Users.Include(x => x.UserParent)
                                       .Include(x => x.UserChildren)
                                       .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                                       .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            throw new Exception("user not found ,id=" + id);

        var dto = new UserDto
        {
            Pin = user.Pin,
            Fio = user.Fio,
        };
        if (isIncludeDatas)
        {
            dto.Parent = new UserParentDto
            {
                PinFather = user.UserParent.PinFather,
                PinMother = user.UserParent.PinMother,
            };
            dto.Children = user.UserChildren.Select(x => new UserChildDto
            {
                Id = x.Id,
                Pin = x.Pin,
                Fio = x.Fio
            }).ToList();
            dto.Roles = user.UserRoles.Select(x => new RoleDto
            {
                Id = x.RoleId,
                RoleName = x.Role.Name,
            }).ToList();
        }
        return dto;
    }
    public async Task<Guid> UpdateAsync(UserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Pin == userDto.Pin);
        if (user == null) return Guid.Empty;

        user.UpdateDate = DateTime.UtcNow;
        user.Pin = userDto.Pin;
        user.Fio = userDto.Fio;

        if (userDto.Roles.Count > 0)
            await UpdateOrAddRoles(userDto.Roles, user);
        if (userDto.Children.Count > 0)
            await UpdateOrAddUserChildren(userDto.Children, user);
        if (userDto.Parent is not null)
            await UpdateOrAddUserParent(userDto.Parent, user);

        var userId = (_context.Users.Update(user)).Entity.Id;
        await _context.SaveChangesAsync();

        return userId;
    }
    public async Task DeleteAsync(Guid id)
    {
        var user = (await _context.Users.FirstOrDefaultAsync(x => x.Id == id))
            ?? throw new Exception("user not found ,id= " + id);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    private async Task UpdateOrAddRoles(List<RoleDto> roleDtos, User user)
    {
        var roles = new List<UserRole>();
        foreach (var roleDto in roleDtos)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleDto.Id);

            if (role is not null)
            {
                role.Name = roleDto.RoleName;
                role.UpdateDate = DateTime.UtcNow;
                _context.Roles.Update(role);
            }
            else
            {
                roles.Add(new UserRole
                {
                    Role = new Role
                    {
                        Name = roleDto.RoleName,
                    }
                });
                user.UserRoles = roles;
            }
        }
    }

    private async Task UpdateOrAddUserParent(UserParentDto userParentDto, User user)
    {
        var newUserParent = new UserParent
        {
            PinFather = userParentDto.PinFather,
            PinMother = userParentDto.PinMother,
        };
        var userParent = await _context.UserParents.FirstOrDefaultAsync(x => x.UserId == user.Id);
        if (userParent is not null)
        {
            _context.UserParents.Remove(userParent);
        }

        user.UserParent = newUserParent;
    }

    private async Task UpdateOrAddUserChildren(List<UserChildDto> userChildrenDtos, User user)
    {
        var children = new List<UserChild>();
        foreach (var childDto in userChildrenDtos)
        {
            var newUserChild = new UserChild
            {
                Fio = childDto.Fio,
                Pin = childDto.Pin,
            };

            var userChild = await _context.UserChildren.FirstOrDefaultAsync(x => x.Id == childDto.Id);
            if (userChild is not null)
            {
                userChild.Pin = newUserChild.Pin;
                userChild.Fio = newUserChild.Fio;
                userChild.UpdateDate = DateTime.UtcNow;
                _context.UserChildren.Update(userChild);
            }
            else
            {
                children.Add(newUserChild);
                user.UserChildren = children;
            }
        }

    }


}
