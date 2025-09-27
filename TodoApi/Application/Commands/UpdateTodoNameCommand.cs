using System;
using TodoApi.Application;

namespace TodoApi.Application.Commands
{
    public class UpdateTodoNameCommand : ICommand
    {
        public Guid Id { get; }
        public string NewName { get; }

        public UpdateTodoNameCommand(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }
}