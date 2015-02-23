using System;
using Microsoft.VisualStudio;

namespace Chambersoft.DapperFactory.Handlers
{
    public class ProErrorHandler : IProErrorHandler
    {
        public bool Succeeded(int hr)
        {
            return ErrorHandler.Succeeded(hr);
        }

        public bool Failed(int hr)
        {
            return ErrorHandler.Failed(hr);
        }

        public int ThrowOnFailure(int hr)
        {
            return ErrorHandler.ThrowOnFailure(hr);
        }

        public int ThrowOnFailure(int hr, params int[] expectedHrFailure)
        {
            return ErrorHandler.ThrowOnFailure(hr, expectedHrFailure);
        }

        public bool IsRejectedRpcCall(int hr)
        {
            return ErrorHandler.IsRejectedRpcCall(hr);
        }

        public bool IsCriticalException(Exception ex)
        {
            return ErrorHandler.IsCriticalException(ex);
        }

        public int CallWithComConvention(Func<int> method, bool reportError = false)
        {
            return ErrorHandler.CallWithCOMConvention(method, reportError);
        }

        public int CallWithComConvention(Action method, bool reportError = false)
        {
            return ErrorHandler.CallWithCOMConvention(method, reportError);
        }
    }
}
