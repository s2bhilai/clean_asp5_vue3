﻿using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Travel.Application.Common.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private Stopwatch _timer;
        private ILogger<TRequest> _logger;

        public PerformanceBehavior(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            if (elapsedMilliseconds <= 500) return response;

            var requestName = typeof(TRequest).Name;
            _logger.LogWarning("Travel Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                                     requestName, elapsedMilliseconds, request);

            return response;
        }
    }
}
