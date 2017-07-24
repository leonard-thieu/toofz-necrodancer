using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

internal sealed class ValidationTemplate<T> : IDataErrorInfo, INotifyDataErrorInfo
    where T : INotifyPropertyChanged
{
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, IValidator> validators = new ConcurrentDictionary<RuntimeTypeHandle, IValidator>();

    private static IValidator GetValidator(Type modelType)
    {
        IValidator validator;

        if (!validators.TryGetValue(modelType.TypeHandle, out validator))
        {
            var typeName = string.Format("{0}.{1}Validator", modelType.Namespace, modelType.Name);
            var type = modelType.Assembly.GetType(typeName, true);
            validators[modelType.TypeHandle] = validator = (IValidator)Activator.CreateInstance(type);
        }

        return validator;
    }

    public ValidationTemplate(INotifyPropertyChanged target)
    {
        this.target = target;
        validator = GetValidator(target.GetType());
        validationResult = validator.Validate(target);
        target.PropertyChanged += Validate;
    }

    private readonly INotifyPropertyChanged target;
    private readonly IValidator validator;
    private ValidationResult validationResult;

    private void Validate(object sender, PropertyChangedEventArgs e)
    {
        validationResult = validator.Validate(target);
        foreach (var error in validationResult.Errors)
        {
            OnErrorsChanged(error.PropertyName);
        }
    }

    #region IDataErrorInfo Members

    public string this[string columnName]
    {
        get
        {
            var strings = validationResult.Errors.Where(x => x.PropertyName == columnName)
                                          .Select(x => x.ErrorMessage)
                                          .ToArray();
            return string.Join(Environment.NewLine, strings);
        }
    }

    public string Error
    {
        get
        {
            var strings = validationResult.Errors.Select(x => x.ErrorMessage)
                                          .ToArray();
            return string.Join(Environment.NewLine, strings);
        }
    }

    #endregion

    #region INotifyDataErrorInfo Members

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    private void OnErrorsChanged(string propertyName) => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

    public bool HasErrors => validationResult.Errors.Count > 0;

    public IEnumerable GetErrors(string propertyName)
    {
        return from e in validationResult.Errors
               where e.PropertyName == propertyName
               select e.ErrorMessage;
    }

    #endregion
}
