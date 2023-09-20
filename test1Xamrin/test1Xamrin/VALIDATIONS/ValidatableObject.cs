using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xUtilityPCL //20180510
{
    public class ValidatableObject<T> : ExtendedBindableObject, IValidity
    {
        private readonly List<IValidationRule<T>> _validations;
        private List<string> _errors;
        private T _value;
        private bool _isValid;

        public List<IValidationRule<T>> Validations => _validations;

        public List<string> Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
                RaisePropertyChanged(() => Errors);
            }
        }

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        public bool IsInError
        {
            get
            {
                return !_isValid;
            }
        }

        public bool IsValid
        {
            get
            {
                RaisePropertyChanged(() => IsInError);
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
                RaisePropertyChanged(() => IsInError);
            }
        }

        public ValidatableObject()
        {
            _isValid = true;
            _errors = new List<string>();
            _validations = new List<IValidationRule<T>>();
        }

        public bool Validate(bool daBottoneSalva)
        {
            Errors.Clear();

            IEnumerable<string> errors;

            if (daBottoneSalva == false) //20180515 introdotto IF
                errors = _validations.Where(x => x.SoloGlobale == false).ToList().Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);
            else

                errors = _validations.Where(v => !v.Check(Value))
                    .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return this.IsValid;
        }
    }

}
