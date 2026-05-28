namespace Application.Modules.Courses.Models;

/// <summary>
/// Represents a response with data.
/// </summary>
/// <typeparam name="T">The response data type.</typeparam>
/// <param name="Data">The response data.</param>
public abstract record Response<T>(T Data);
