using FluentValidation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Booking.Application.Common.Validators.Catalogs
{
    internal abstract class CatalogValidator<T, TValue> : AsyncPropertyValidator<T, TValue>
    {
        private readonly ICatalogService _catalogService;
        private readonly CatalogsEnum _catalogToValidate;
        private readonly bool _useCode;
        private readonly string? _additionalCondition;
        private readonly object? _additionalParams;

        protected CatalogValidator(
            ICatalogService catalogService,
            CatalogsEnum catalogToValidate,
            bool useCode = false,
            string? additionalCondition = null,
            object? additionalParams = null)
        {
            _catalogService = catalogService;
            _catalogToValidate = catalogToValidate;
            _useCode = useCode;
            _additionalCondition = additionalCondition;
            _additionalParams = additionalParams;
        }

        public override string Name => "InvalidCatalogValidator";

        public override async Task<bool> IsValidAsync(ValidationContext<T> context, TValue value, CancellationToken cancellation)
        {
            if (value is null || value is string str && string.IsNullOrWhiteSpace(str))
            {
                return true;
            }

            context.MessageFormatter.AppendArgument("CatalogName", _catalogToValidate.ToString());
            bool exists = await _catalogService.ExistsAsync(_catalogToValidate, value, _useCode);
            return exists;
        }
        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "'{PropertyValue}' is not valid code for the catalog '{CatalogName}'. Please use a valid value";
        }
    }


    internal sealed class CatalogStringPropertyValidator<T> : CatalogValidator<T, string?>
    {
        public CatalogStringPropertyValidator(
            ICatalogService cardDbService, CatalogsEnum catalogToValidate,
            bool useCode = false,
            string? additionalCondition = null,
            object? additionalParams = null) : base(cardDbService, catalogToValidate, useCode, additionalCondition, additionalParams)
        {
        }
    }

    internal sealed class CatalogIntValidator<T> : CatalogValidator<T, int>
    {
        public CatalogIntValidator(
            ICatalogService cardDbService,
            CatalogsEnum catalogToValidate,
            string? additionalCondition = null,
            object? additionalParams = null) : base(cardDbService, catalogToValidate, false, additionalCondition, additionalParams)
        {
        }
    }
}
