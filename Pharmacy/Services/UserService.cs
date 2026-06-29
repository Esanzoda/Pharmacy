using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IUserService : IBaseService<UserRequest, UserResponse>
{
    Task<UserResponse> GetByEmailAsync(string email);
    Task<List<UserResponse>> GetByRoleAsync(Role role, int page, int pageSize);
    Task<List<UserResponse>> GetByNameAsync(string name, int page, int pageSize);
}

public class UserService : BaseService<User, UserRequest, UserResponse>, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IMapper mapper)
        : base(userRepository, mapper)
    {
        _userRepository = userRepository;
    }

    public override async Task<UserResponse> CreateAsync(UserRequest request)
    {
        var exists = await _userRepository.EmailExistsAsync(request.Email);
        if (exists)
            throw new ResourseIsAlredyExsistExeption("Email already exists");

        var user = Mapper.Map<User>(request);
        user.PasswordHash = (request.Password);
        user.CreateAt = DateTime.UtcNow;

        await _userRepository.CreateAsync(user);
        return Mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> GetByEmailAsync (string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            throw new ResourseNotFoundExeption("User not found");

        return Mapper.Map<UserResponse>(user);
    }

    public async Task<List<UserResponse>> GetByRoleAsync(Role role, int page, int pageSize)
    {
        var users = await _userRepository.GetByRoleAsync(role, page, pageSize);
        if (!users.Any())
            throw new ResourseNotFoundExeption("Users not found");

        return Mapper.Map<List<UserResponse>>(users);
    }

    public async Task<List<UserResponse>> GetByNameAsync(string name, int page, int pageSize)
    {
        var users = await _userRepository.GetByNameAsync(name, page, pageSize);
        if (!users.Any())
            throw new ResourseNotFoundExeption("Users not found");

        return Mapper.Map<List<UserResponse>>(users);
    }
}