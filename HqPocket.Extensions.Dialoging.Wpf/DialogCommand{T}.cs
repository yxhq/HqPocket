using System;

namespace HqPocket.Extensions.Dialoging;

public class DialogCommand<TViewModel> where TViewModel : IDialogViewModel
{
    /// <summary>
    /// 一个名称，若该DialogCommand需要执行TViewModel里的方法，则设置该名称，
    /// 并在TViewModel里实现public/private void/bool On{Name}()方法，则在按钮按下时将会执行On{Name}()方法，
    /// 其中bool返回值表示该按钮按下后是否可以调用关闭窗口，默认关闭，若不需要改变，该方法可返回void；
    /// 若不需要执行TViewModel里的方法，则不设置即可
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 按钮内容
    /// </summary>
    public object? Content { get; set; }

    /// <summary>
    /// 是否默认内容
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// 按钮按下后执行的操作
    /// </summary>
    public Action<TViewModel>? Execute { get; set; }

    /// <summary>
    /// 按钮是否可以执行
    /// </summary>
    public Func<TViewModel, bool>? CanExecute { get; set; }

}