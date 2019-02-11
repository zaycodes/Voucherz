using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VoucherServiceBL.Service
{
    public static class ExtensionMethods
    {
        public static Task<TBase> ToDerived<TBase>(this Task<TBase> t) {
            return t;
        }

        public static async Task<TDerived> ToDerived<TBase, TDerived>(
                this Task<TBase> task, Func<TBase, Task<TDerived>> continuation) {
            return await continuation(await task);
        }

        public static async Task<TDerived> ToDerived<TDerived>(
                this Task antecedent, Func<TDerived> continuation) {
                    await antecedent;
                    return continuation();
        }

        public static async Task<TSbclass> GetSubclass<TBaseCls, TSbclass>( 
                this Task<TBaseCls> baseTask, Func<TBaseCls, Task<TSbclass>> continuation
        ) 
        {
            return await continuation(await baseTask);
        }

        public static async Task<TBaseCls> ToBase<TSubCls, TBaseCls>( 
                this Task<TSubCls> subClassTask, Func<TSubCls, Task<TBaseCls>> continuation) 
        {
            return await continuation(await subClassTask);
        }

        public static async Task<TNewResult> Then<TResult, TNewResult>( 
                        this Task<TResult> task, Func<TResult, Task<TNewResult>> continuation) 
        { 
            return await continuation(await task); 
        }
    }
}