// ----------------------------------------------------------------------------
// Copyright (C) 2003-2005 Microsoft Corporation, All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.ServiceModel.Samples
{
    /// <summary>
    /// A generic base class for IAsyncResult implementations
    /// that wraps a ManualResetEvent.
    /// </summary>
    abstract class AsyncResult : IAsyncResult
    {
        AsyncCallback asyncCallback;
        object stateObject;
        bool hasCompletedSynchronously;
        bool endCalled;
        Exception exceptionInfo;
        bool isCompleted;
        ManualResetEvent manualResetEvent;
        object thisLock;

        protected AsyncResult(AsyncCallback callback, object state)
        {
            asyncCallback = callback;
            stateObject = state;
            thisLock = new object();
        }

        public object AsyncState
        {
            get
            {
                return stateObject;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (manualResetEvent != null)
                {
                    return manualResetEvent;
                }

                lock (ThisLock)
                {
                    if (manualResetEvent == null)
                    {
                        manualResetEvent = new ManualResetEvent(isCompleted);
                    }
                }

                return manualResetEvent;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return hasCompletedSynchronously;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
        }

        object ThisLock
        {
            get
            {
                return thisLock;
            }
        }

        // Call this version of complete when your asynchronous operation is complete.  This will update the state
        // of the operation and notify the callback.
        protected void Complete(bool completedSynchronously)
        {
            if (isCompleted)
            {
                // It is a bug to call Complete twice.
                throw new InvalidOperationException("Cannot call Complete twice");
            }

            hasCompletedSynchronously = completedSynchronously;

            if (completedSynchronously)
            {
                // If we completedSynchronously, then there is no chance that the manualResetEvent was created so
                // we do not need to worry about a race condition.
                Debug.Assert(manualResetEvent == null, "No ManualResetEvent should be created for a synchronous AsyncResult.");
                isCompleted = true;
            }
            else
            {
                lock (ThisLock)
                {
                    isCompleted = true;

                    if (manualResetEvent != null)
                    {
                        manualResetEvent.Set();
                    }
                }
            }

            // If the callback throws, there is a bug in the callback implementation
            if (asyncCallback != null)
            {
                asyncCallback(this);
            }
        }

        // Call this version of complete if you raise an exception during processing.  In addition to notifying
        // the callback, it will capture the exception and store it to be thrown during AsyncResult.End.
        protected void Complete(bool completedSynchronously, Exception exception)
        {
            exceptionInfo = exception;
            Complete(completedSynchronously);
        }

        // End should be called when the End function for the asynchronous operation is complete.  It
        // ensures the asynchronous operation is complete, and does some common validation.
        protected static TAsyncResult End<TAsyncResult>(IAsyncResult result)
            where TAsyncResult : AsyncResult
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            
            TAsyncResult asyncResult = result as TAsyncResult;

            if (asyncResult == null)
            {
                throw new ArgumentException("Invalid async result.", "result");
            }

            if (asyncResult.endCalled)
            {
                throw new InvalidOperationException("Async object already ended.");
            }

            asyncResult.endCalled = true;

            if (!asyncResult.isCompleted)
            {
                asyncResult.AsyncWaitHandle.WaitOne();
            }

            if (asyncResult.manualResetEvent != null)
            {
                asyncResult.manualResetEvent.Close();
            }

            if (asyncResult.exceptionInfo != null)
            {
                throw asyncResult.exceptionInfo;
            }

            return asyncResult;
        }
    }

    //An AsyncResult that completes as soon as it is instantiated.
    class CompletedAsyncResult : AsyncResult
    {
        public CompletedAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            Complete(true);
        }

        public static void End(IAsyncResult result)
        {
            AsyncResult.End<CompletedAsyncResult>(result);
        }
    }

    //A strongly typed AsyncResult
    abstract class TypedAsyncResult<T> : AsyncResult
    {
        T genericData;

        protected TypedAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
        }

        public T Data
        {
            get { return genericData; }
        }

        protected void Complete(T data, bool completedSynchronously)
        {
            genericData = data;
            Complete(completedSynchronously);
        }

        public static T End(IAsyncResult result)
        {
            TypedAsyncResult<T> typedResult = AsyncResult.End<TypedAsyncResult<T>>(result);
            return typedResult.Data;
        }
    }

    //A strongly typed AsyncResult that completes as soon as it is instantiated.
    class TypedCompletedAsyncResult<T> : TypedAsyncResult<T>
    {
        public TypedCompletedAsyncResult(T data, AsyncCallback callback, object state)
            : base(callback, state)
        {
            Complete(data, true);
        }

        public new static T End(IAsyncResult result)
        {
            TypedCompletedAsyncResult<T> completedResult = result as TypedCompletedAsyncResult<T>;
            if (completedResult == null)
            {
                throw new ArgumentException("Invalid async result.", "result");
            }

            return TypedAsyncResult<T>.End(completedResult);
        }
    }
}
