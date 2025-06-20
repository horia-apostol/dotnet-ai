﻿namespace DotnetAi.Sdk.Exceptions;

/// <summary>
/// Exception thrown when an AI provider call fails.
/// </summary>
public class AiClientException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AiClientException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public AiClientException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AiClientException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public AiClientException(string message, Exception innerException)
        : base(message, innerException) { }
}
