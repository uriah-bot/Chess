using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chess.ViewModel
{
    public class ValidatableViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        public bool HasErrors => _propertyErrors.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            return _propertyErrors.GetValueOrDefault(propertyName, null);

            // _propertyErrors[propertyName] bad because may not exist yet
        }

        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors)); // Uses the method from ViewModelBase!
        }

        protected void AddError(string errorMessage, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) 
                return;

            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new List<string>());
            }

            var errors = _propertyErrors[propertyName];

            if (errors.Contains(errorMessage))
            {
                return;
            }
            
            _propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        protected void ClearErrors([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
                return;

            if (_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
