using MediatR;
using System;

namespace MS.IConstruye.Application
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand { }

    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult> { }

    public interface ICommand : IRequest
    {
        Guid Id { get; }
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid Id { get; }
    }

    public interface IQuery<out TResult> : IRequest<TResult> { }

    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult> { }

    public class CommandBase : ICommand
    {
        public Guid Id { get; }

        public CommandBase() => Id = Guid.NewGuid();

        protected CommandBase(Guid id) => Id = id;
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        public Guid Id { get; }

        protected CommandBase() => Id = Guid.NewGuid();

        protected CommandBase(Guid id) => Id = id;
    }
}
