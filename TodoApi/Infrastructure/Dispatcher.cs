using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Application;

namespace TodoApi.Infrastructure
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
                await handler.Handle(command);
            }
        }

        public async Task<TResponse> Query<TResponse>(IQuery<TResponse> query)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var queryType = query.GetType();
                var queryInterface = queryType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));
                var responseType = queryInterface.GetGenericArguments()[0];

                var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, responseType);

                var handler = scope.ServiceProvider.GetRequiredService(handlerType);

                return await (Task<TResponse>)((dynamic)handler).Handle((dynamic)query);
            }
        }
    }

    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }

    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<TResponse> Handle(TQuery query);
    }
}