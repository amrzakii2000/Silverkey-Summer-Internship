using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFirstapp.Services
{
    public interface ICounterService
    {
        int CurrentCount { get; }
        void IncrementCount();
    }

    public class CounterService : ICounterService
    {
        private int _currentCount;
        public int CurrentCount { get => _currentCount; }

        public void IncrementCount()
        {
            _currentCount++;
        }
    }
}
