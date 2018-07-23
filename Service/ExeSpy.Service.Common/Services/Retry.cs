using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExeSpy.Service.Common.Services
{
    public static class Retry
    {
        public static T Linear<T>(Func<T> action, int retryCount = 3, double retryIntervalInMilliseconds = 500)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
                        Task.Delay(TimeSpan.FromMilliseconds(retryIntervalInMilliseconds));
                    }

                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }

        public static T Exponential<T>(Func<T> action, int retryCount = 3, double minimumBackoffInMilliseconds = 500, double maximumBackoffInMilliseconds = 4000, double incrementMultiplier = 2)
        {
            var exceptions = new List<Exception>();
            double retryInterval = minimumBackoffInMilliseconds;

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
                        Task.Delay(TimeSpan.FromMilliseconds(retryInterval));
                        var calculatedIncrement = retryInterval *= incrementMultiplier;
                        retryInterval = Math.Min(minimumBackoffInMilliseconds + calculatedIncrement, maximumBackoffInMilliseconds);
                    }

                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}
