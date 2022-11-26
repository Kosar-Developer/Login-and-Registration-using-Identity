﻿using MediatR;
using sattec.Identity.Application.Common.Interfaces;
using sattec.Identity.Application.Common.Models;


namespace sattec.Identity.Application.Users.Commands.ConfirmMobile
{
    public record ConfirmMobileCommand : IRequest<Result>
    {
        public string Code { get; set; }
    }
    public class ConfirmMobileCommandHandler : IRequestHandler<ConfirmMobileCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;


        public ConfirmMobileCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService; 
        }

        public async Task<Result> Handle(ConfirmMobileCommand request, CancellationToken cancellationToken)
        {
            var result = _identityService.GetByMobileVerificationCode(request.Code);

            await _context.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
