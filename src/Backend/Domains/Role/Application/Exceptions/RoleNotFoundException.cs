namespace Backend.Domains.Role.Application.Exceptions;

public class RoleNotFoundException(string key) : Exception($"Role with key '{key}' not found.");
