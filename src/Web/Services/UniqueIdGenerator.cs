using System;
using Be.Vlaanderen.Basisregisters.Generators.Guid;

namespace STKBC.Stats.Services;

public interface IIdGenerator
{
    string NewId();
    Guid NewGuid();
    DeterministicGuid NewDeterministicId(Guid namespaceId, string value);
}

public class UniqueIdGenerator : IIdGenerator
{
    public static readonly Guid NamespaceId = new Guid("0bd0b193-a448-4b22-b426-3eab22d4bb98");

    public string NewId() => Guid.NewGuid().ToString();
    public Guid NewGuid() => Guid.NewGuid();

    public DeterministicGuid NewDeterministicId(Guid namespaceId, string value)
    {
        return DeterministicGuid.New(namespaceId, value);
    }

    public DeterministicGuid NewDeterministicId(string value)
    {
        return DeterministicGuid.New(UniqueIdGenerator.NamespaceId, value);
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

    public string String() => this._namespaceId.ToString();

    public DeterministicGuid NewGuid(string value)
    {

        return DeterministicGuid.New(_namespaceId, value);
    }

    public static DeterministicGuid New(Guid namespaceId, string value)
    {
        return new DeterministicGuid(Deterministic.Create(namespaceId, value));
    }
}