using System.Reflection;
using Autofac;
using Backend.Domains.Role.Application.Mapping;
using Backend.Domains.Role.Application.Services;
using Backend.Domains.Role.Infrastructure.Roles;
using Module = Autofac.Module;

namespace Backend.Domains.Role.Application.DI;

public class RoleModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(x => x.IsAssignableTo<IRole>())
            .AsImplementedInterfaces();

        builder.RegisterType<RoleService>().As<IRoleService>();
        builder.RegisterType<RoleMapper>().As<IRoleMapper>();
    }
}
