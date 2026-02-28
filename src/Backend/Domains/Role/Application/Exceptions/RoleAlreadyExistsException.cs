namespace Backend.Domains.Role.Application.Exceptions;

public class RoleAlreadyExistsException(string key) : Exception($"A role with the key '{key}' already exists.");
