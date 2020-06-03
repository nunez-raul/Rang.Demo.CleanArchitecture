using Rang.Demo.CleanArchitecture.Domain.Common;
using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Domain.Entity
{
    public abstract class BaseEntity<Tmodel>
        where Tmodel : BaseModel
    {
        //fields
        protected readonly Tmodel _model;

        //properties
        public bool IsValid { get => ValidateMe(); }
        public IDictionary<ModelValidationStatusCode, List<string>> ModelValidationErrors { get; protected set; }

        //constructors
        protected BaseEntity(Tmodel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            if (_model.Id == null || model.Id == Guid.Empty) { _model.Id = Guid.NewGuid(); }
            ModelValidationErrors = new Dictionary<ModelValidationStatusCode, List<string>>();
            InitializeMe();
        }

        //methods
        public abstract Tmodel GetModel();
        protected abstract bool ValidateMe();
        protected abstract void InitializeMe();
        protected void AddModelValidationError(ModelValidationStatusCode statusCode, string validationMessage)
        {
            if (ModelValidationErrors.ContainsKey(statusCode))
            {
                ModelValidationErrors[statusCode].Add(validationMessage);
            }
            else
            {
                var messageList = new List<string> { validationMessage };
                ModelValidationErrors.Add(statusCode, messageList);
            }
        }
    }
}
