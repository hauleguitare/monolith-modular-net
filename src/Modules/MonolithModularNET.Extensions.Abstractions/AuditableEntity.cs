namespace MonolithModularNET.Extensions.Abstractions;

public class AuditableEntity: DefaultAuditableEntity
{
    
}

public class DefaultAuditableEntity : AuditableEntity<string>
{
    
}


public abstract class AuditableEntity<TId>: BaseEntity<TId>, IAuditableEntity
{
    public virtual DateTimeOffset? CreatedAt { get; set; }
    public virtual DateTimeOffset? ModifiedAt { get; set; }
    public virtual string? CreatedBy { get; set; }
    public virtual string? ModifiedBy { get; set; }
    
    public virtual IAuditableEntity AddCreatedBy(string createdBy)
    {
        CreatedBy = createdBy;

        return this;
    }

    public virtual IAuditableEntity UpdateCreatedAt()
    {
        CreatedAt = DateTimeOffset.Now;

        return this;
    }

    public virtual IAuditableEntity AddModifiedBy(string modifiedBy)
    {
        ModifiedBy = modifiedBy;

        return this;
    }

    public virtual IAuditableEntity UpdateModifiedAt()
    {
        ModifiedAt = DateTimeOffset.Now;
        return this;
    }
}

public interface IAuditableEntity
{
    /// <summary>
    /// Timestamp for created time entities.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; set; }
    
    /// <summary>
    /// Timestamp for modified time entities.
    /// </summary>
    public DateTimeOffset? ModifiedAt { get; set; }
    
    /// <summary>
    /// Identity ID for created time entities.
    /// </summary>
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// Identity ID for modified time entities.
    /// </summary>
    public string? ModifiedBy { get; set; }

    public IAuditableEntity AddCreatedBy(string createdBy);

    public IAuditableEntity UpdateCreatedAt();

    public IAuditableEntity AddModifiedBy(string modifiedBy);

    public IAuditableEntity UpdateModifiedAt();
}
