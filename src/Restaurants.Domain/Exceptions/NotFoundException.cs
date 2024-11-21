

namespace Restaurants.Domain.Exceptions;

public class NotFoundException : Exception
{
    public string ResourceType { get; }
    public string ResourceIdentifier { get; }

    public NotFoundException(string resourceType, string resourceIdentifier)
        : base($"{resourceType} with id: {resourceIdentifier} does not exist")
    {
        ResourceType = resourceType;
        ResourceIdentifier = resourceIdentifier;
    }

    public override string ToString()
    {
        return $"{ResourceType} with id: {ResourceIdentifier} was not found.";
    }
}
