using System;
using Castle.MicroKernel;

namespace nu.Specs.AMC
{
    public interface IMockingStrategy
    {
        object Create(CreationContext context, Type type);
    }
}