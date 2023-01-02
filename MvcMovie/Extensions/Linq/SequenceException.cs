#region usings

using System.Runtime.Serialization;

#endregion

namespace MvcMovie.Extensions.Linq;

/// <summary>
///     The exception that is thrown for a sequence that fails a condition.
/// </summary>
[Serializable]
public class SequenceException : Exception
{
    private const string _defaultMessage = "Error in sequence.";

    /// <summary>
    ///     Initializes a new instance of the <see cref="SequenceException" /> class.
    /// </summary>
    public SequenceException() :
        this(string.Empty)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SequenceException" /> class
    ///     with a given error message.
    /// </summary>
    /// <param name="message">A message that describes the error.</param>
    public SequenceException(string message) :
        this(message, null)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SequenceException" /> class
    ///     with a given error message and a reference to the inner exception
    ///     that is the cause of the exception.
    /// </summary>
    /// <param name="message">A message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public SequenceException(string message, Exception? innerException) :
        base(string.IsNullOrEmpty(message) ? _defaultMessage : message, innerException)
    {
    }


    /// <summary>
    ///     Initializes a new instance of the <see cref="SequenceException" /> class
    ///     with serialized data.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected SequenceException(SerializationInfo info, StreamingContext context) :
        base(info, context)
    {
    }
}