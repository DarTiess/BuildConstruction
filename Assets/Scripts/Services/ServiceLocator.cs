using System;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, IService> _servicesMap = new();
        public T Get<T>() => (T) _servicesMap[typeof(T)];
        public void Reg<T>(T service) where T : IService => _servicesMap[typeof(T)] = service;
    }
}