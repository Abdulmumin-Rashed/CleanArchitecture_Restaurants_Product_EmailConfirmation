

namespace Restaurants.Domain.Exceptions;

public class ForbidException(string resultMessage) : Exception($"{resultMessage} access is denied")
{
}
