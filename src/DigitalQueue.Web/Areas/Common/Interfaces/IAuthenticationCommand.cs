using MediatR;

namespace DigitalQueue.Web.Areas.Common.Interfaces;

public interface IAuthenticationCommand<out TResult> : IRequest<TResult>
{ }
