using Backend.Persistence.Sql.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Utils.EntityFramework.Infrastructure.Handlers.Event;

namespace Backend.Application.Handlers.Event;

public class CoreMigrationHandler(ILogger<CoreMigrationHandler> logger, IDbContextFactory<CoreDbContext> factory, IMediator mediator) : BaseMigrationHandler<CoreMigrationHandler, CoreDbContext>(logger, factory, mediator);
