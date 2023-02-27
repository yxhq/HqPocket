using HqPocket.Mvvm;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HqPocket.Wpf.Mvvm;

public class ValidatingObservableObject : ObservableObject, INotifyDataErrorInfo
{
    [NonSerialized]
    private int _totalErrors;
    [NonSerialized]
    private readonly IDictionary<string, List<ValidationResult>> _errors = new Dictionary<string, List<ValidationResult>>();
    [NonSerialized]
    private static readonly PropertyChangedEventArgs _hasErrorsChangedEventArgs = new(nameof(HasErrors));

    [XmlIgnore]
    [JsonIgnore]
    public bool HasErrors => _totalErrors > 0;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    protected virtual bool SetAndValidate<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        var proertyChanged = SetValue(ref storage, value, propertyName);
        if (proertyChanged)
        {
            ValidateProperty(value, propertyName);
        }
        return proertyChanged;
    }

    protected virtual bool SetAndValidate<T>(ref T storage, T value, Action<T> onChanged, [CallerMemberName] string? propertyName = null)
    {
        var proertyChanged = SetValue(ref storage, value, onChanged, propertyName);
        if (proertyChanged)
        {
            ValidateProperty(value, propertyName);
        }
        return proertyChanged;
    }

    protected virtual bool SetAndValidate<T>(ref T storage, T value, Action<T> onChanging, Action<T> onChanged, [CallerMemberName] string? propertyName = null)
    {
        var proertyChanged = SetValue(ref storage, value, onChanging, onChanged, propertyName);
        if (proertyChanged)
        {
            ValidateProperty(value, propertyName);
        }
        return proertyChanged;
    }

    [Pure]
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return GetAllErrors();
        }

        if (_errors.TryGetValue(propertyName, out List<ValidationResult>? errors))
        {
            return errors;
        }

        return Array.Empty<ValidationResult>();
    }

    [Pure]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private IEnumerable GetAllErrors()
    {
        return _errors.Values.SelectMany(errors => errors);
    }

    private void ValidateProperty(object? value, string? propertyName)
    {
        if (propertyName is null)
        {
            throw new ArgumentNullException(nameof(propertyName), "The input property name cannot be null when validating a property");
        }

        if (!_errors.TryGetValue(propertyName, out List<ValidationResult>? propertyErrors))
        {
            propertyErrors = new List<ValidationResult>();
            _errors.Add(propertyName, propertyErrors);
        }

        bool errorsChanged = false;
        if (propertyErrors.Count > 0)
        {
            propertyErrors.Clear();
            errorsChanged = true;
        }

        bool isValid = Validator.TryValidateProperty(value, new ValidationContext(this) { MemberName = propertyName }, propertyErrors);

        if (isValid)
        {
            if (errorsChanged)
            {
                _totalErrors--;

                if (_totalErrors == 0)
                {
                    OnPropertyChanged(_hasErrorsChangedEventArgs);
                }
            }
        }
        else if (!errorsChanged)
        {
            _totalErrors++;

            if (_totalErrors == 1)
            {
                OnPropertyChanged(_hasErrorsChangedEventArgs);
            }
        }

        if (errorsChanged || !isValid)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}