namespace Ordering.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
        
    }

    public NotFoundException(string name, object key) : base($"Order with key {key} not found")
    {
        
    }
}