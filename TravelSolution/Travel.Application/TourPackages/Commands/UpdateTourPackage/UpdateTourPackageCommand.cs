﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Travel.Application.Common.Exceptions;
using Travel.Application.Common.Interfaces;
using Travel.Domain.Entities;

namespace Travel.Application.TourPackages.Commands.UpdateTourPackage
{
    public class UpdateTourPackageCommand: IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateTourPackageCommandHandler : IRequestHandler<UpdateTourPackageCommand>
    {
        private IApplicationDbContext _context;

        public UpdateTourPackageCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTourPackageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TourPackages.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TourPackage), request.Id);
            }

            entity.Name = request.Name;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
