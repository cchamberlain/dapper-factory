// Guids.cs
// MUST match guids.h
using System;

namespace Chambersoft.DapperFactory
{
    static class GuidList
    {
        public const string guidDapperFactoryPkgString = "cd7aec75-b502-4460-afae-e89ccf446ef1";
        public const string guidDapperFactoryCmdSetString = "5332557a-784d-4712-ad61-13d0a266bfb2";
        //public const string guidDapperFactoryInterfaceCmdSetString = "DFEDFA1F-E1B2-4B65-8487-AEC6897BAC99";

        public static readonly Guid guidDapperFactoryCmdSet = new Guid(guidDapperFactoryCmdSetString);
        //public static readonly Guid guidDapperFactoryInterfaceCmdSet = new Guid(guidDapperFactoryInterfaceCmdSetString);
    };
}