using System;
using TodoApi.Application;

namespace TodoApi.Application.Commands
{
    public class CreateTodoCommand : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }

        public CreateTodoCommand(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}