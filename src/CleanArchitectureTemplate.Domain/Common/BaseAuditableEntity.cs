using System;

namespace CleanArchitectureTemplate.Domain.Common;

public abstract class BaseAuditableEntity : Entity
{
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}