using BuildingBlocks.Exceptions;

namespace Auth.Application.Exceptions;

public class UserNotFoundException(string name, string email) : NotFoundException(name, email)
{
    
}