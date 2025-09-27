using System.Threading.Tasks;

namespace TodoApi.Application
{
    public interface IDispatcher
    {
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResponse> Query<TResponse>(IQuery<TResponse> query);
    }
}