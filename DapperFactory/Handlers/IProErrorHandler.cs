using System;

namespace Chambersoft.DapperFactory.Handlers
{
    public interface IProErrorHandler
    {
        bool Succeeded(int hr);
        bool Failed(int hr);
        int ThrowOnFailure(int hr);
        int ThrowOnFailure(int hr, params int[] expectedHrFailure);
        bool IsRejectedRpcCall(int hr);
        bool IsCriticalException(Exception ex);
        int CallWithComConvention(Func<int> method, bool reportError = false);
        int CallWithComConvention(Action method, bool reportError = false);
    }
}