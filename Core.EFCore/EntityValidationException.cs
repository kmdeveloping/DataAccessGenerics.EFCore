using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.EFCore;

public class EntityValidationException : ValidationException
{
    public EntityValidationException()
    {
    }

    public EntityValidationException(ValidationResult validationResult, ValidationAttribute validatingAttribute, object value) : base(validationResult, validatingAttribute, value)
    {
    }

    public EntityValidationException(string message) : base(message)
    {
    }

    public EntityValidationException(string errorMessage, ValidationAttribute validatingAttribute, object value) : base(errorMessage, validatingAttribute, value)
    {
    }

    public EntityValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected EntityValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EntityValidationException(string message, List<ValidationResult> entityValidationErrors) : base(message)
    {
        EntityValidationErrors = entityValidationErrors;
    }

    public List<ValidationResult> EntityValidationErrors { get; private set; }
}