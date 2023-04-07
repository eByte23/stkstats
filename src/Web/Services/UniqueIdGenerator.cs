using System;
using Be.Vlaanderen.Basisregisters.Generators.Guid;

namespace STKBC.Stats.Services;

public interface IIdGenerator
{
    string NewId();
    Guid NewGuid();
    DeterministicGuid NewDeterministicGuid(Guid namespaceId, string value);
}

public class UniqueIdGenerator : IIdGenerator
{
    public string NewId() => Guid.NewGuid().ToString();
    public Guid NewGuid() => Guid.NewGuid();

    public DeterministicGuid NewDeterministicGuid(Guid namespaceId, string value)
    {
        return DeterministicGuid.New(namespaceId, value);
    }
}


public class DeterministicGuid
{
    private readonly Guid _namespaceId;

    public DeterministicGuid(Guid namespaceId)
    {
        this._namespaceId = namespaceId;
    }

    public Guid Id => this._namespaceId;

    public DeterministicGuid NewGuid(string value)
    {

        return DeterministicGuid.New(_namespaceId, value);
    }

    public static DeterministicGuid New(Guid namespaceId, string value)
    {
        return new DeterministicGuid(Deterministic.Create(namespaceId, value));
    }
}