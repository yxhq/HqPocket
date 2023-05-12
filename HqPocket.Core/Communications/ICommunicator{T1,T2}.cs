using System;

namespace HqPocket.Communications
{
    public interface ICommunicator<TProtocol, TData> : ICommunicator<TProtocol>
        where TProtocol : IProtocol
        where TData : class
    {
        /// <summary>
        /// 数据创建成功后
        /// </summary>
        event EventHandler<TData>? DataCreated;
        /// <summary>
        /// 创建数据所使用的委托
        /// </summary>
        Func<TProtocol, TData>? DataCreator { get; set; }
    }
}
