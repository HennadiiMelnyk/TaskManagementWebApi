using NSubstitute;
using TaskManagementWebApi.Application.DTO;
using TaskManagementWebApi.Application.Services;
using TaskManagementWebApi.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace TaskManagementWebApi.Tests;

public class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _service = new UserService(_userRepository);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddUser_WhenNameIsUnique()
    {
        // Arrange
        var dto = new UserCreateDto { Name = "John" };
        _userRepository.GetAllAsync().Returns(Array.Empty<User>());

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        await _userRepository.Received(1).AddAsync(Arg.Is<User>(u => u.Name == "John"));
        Assert.Equal("John", result.Name);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenNameAlreadyExists()
    {
        // Arrange
        var dto = new UserCreateDto { Name = "Existing" };
        _userRepository.GetAllAsync().Returns(new[]
        {
            new User { Id = 1, Name = "Existing" }
        });

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }
}