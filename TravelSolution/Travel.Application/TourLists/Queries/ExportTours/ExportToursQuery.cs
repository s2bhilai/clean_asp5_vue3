using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Travel.Application.Common.Interfaces;

namespace Travel.Application.TourLists.Queries.ExportTours
{
    public class ExportToursQuery : IRequest<ExportToursVm>
    {
        public int ListId { get; set; }
    }

    public class ExportToursQueryHandler : IRequestHandler<ExportToursQuery, ExportToursVm>
    {
        private IApplicationDbContext _context;
        private IMapper _mapper;
        private ICsvFileBuilder _fileBuilder;

        public ExportToursQueryHandler(IApplicationDbContext applicationDbContext ,
            IMapper mapper, ICsvFileBuilder fileBuilder)
        {
            _context = applicationDbContext;
            _mapper = mapper;
            _fileBuilder = fileBuilder;
        }

        public async Task<ExportToursVm> Handle(ExportToursQuery request, CancellationToken cancellationToken)
        {
            var vm = new ExportToursVm();

            var records = await _context.TourPackages
                .Where(t => t.ListId == request.ListId)
                .ProjectTo<TourPackageFileRecord>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            vm.Content = _fileBuilder.BuildTourPackagesFile(records);
            vm.ContentType = "text/csv";
            vm.FileName = "TourPackages.csv";

            return await Task.FromResult(vm);
        }
    }
}
