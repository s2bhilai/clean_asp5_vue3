using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Application.Common.Interfaces;

namespace Travel.Application.TourLists.Commands.UpdateTourList
{
    public class UpdateTourListCommandValidator: AbstractValidator<UpdateTourListCommand>
    {
        private IApplicationDbContext _context;

        public UpdateTourListCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.City)
        }
    }
}
