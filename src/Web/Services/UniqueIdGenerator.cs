using System;

namespace STKBC.Stats.Services;

public class UniqueIdGenerator
{
    public string NewId() => Guid.NewGuid().ToString();
    public Guid NewGuid() => Guid.NewGuid();
}