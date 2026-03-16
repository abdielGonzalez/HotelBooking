using FluentValidation;

namespace Hotel.Booking.Application.Common.Validators.Catalogs
{
    public static class CatalogsValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string?> IsIdInCatalog<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            ICatalogService cardDbService,
            CatalogsEnum catalogToValidate,
            string? additionalCondition = null,
            object? additionalParams = null)
        {
            return ruleBuilder.SetAsyncValidator(new CatalogStringPropertyValidator<T>(
                cardDbService,
                catalogToValidate,
                useCode: false,
                additionalCondition: additionalCondition,
                additionalParams: additionalParams));
        }

        public static IRuleBuilderOptions<T, string?> IsCodeInCatalog<T>(
          this IRuleBuilder<T, string?> ruleBuilder,
          ICatalogService cardDbService,
          CatalogsEnum catalogToValidate,
          string? additionalCondition = null,
          object? additionalParams = null)
        {
            return ruleBuilder.SetAsyncValidator(new CatalogStringPropertyValidator<T>(
                cardDbService,
                catalogToValidate,
                useCode: true,
                additionalCondition: additionalCondition,
                additionalParams: additionalParams));
        }

        public static IRuleBuilderOptions<T, int> IsIdInCatalog<T>(
           this IRuleBuilder<T, int> ruleBuilder,
           ICatalogService cardDbService,
           CatalogsEnum catalogToValidate,
           string? additionalCondition = null,
           object? additionalParams = null)
        {
            return ruleBuilder.SetAsyncValidator(new CatalogIntValidator<T>(cardDbService, catalogToValidate, additionalCondition, additionalParams));
        }

        public static IRuleBuilderOptions<T, int?> IsIdInCatalog<T>(
           this IRuleBuilder<T, int?> ruleBuilder,
           ICatalogService cardDbService,
           CatalogsEnum catalogToValidate,
           string? additionalCondition = null,
           object? additionalParams = null)
        {
            return ruleBuilder.SetAsyncValidator(new CatalogIntValidator<T>(cardDbService, catalogToValidate, additionalCondition, additionalParams));
        }


    }
}
