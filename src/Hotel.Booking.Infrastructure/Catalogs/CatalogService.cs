using Hotel.Booking.Application.Common.Interfaces;
using Hotel.Booking.Application.Common.Validators.Catalogs;
using Dapper;
using System.Data.Common;

namespace Hotel.Booking.Infrastructure.Catalogs
{
    internal sealed class CatalogService : ICatalogService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CatalogService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> ExistsAsync<TIdType>(CatalogsEnum catalog, TIdType id, bool useCode = false)
        {
            using DbConnection cnn = await _connectionFactory.OpenConnectionAsync();
            string tableName = GetTableName(catalog);

            if (string.IsNullOrEmpty(tableName))
            {
                return false;
            }

            //string idColumn = GetIdColumnName(catalog);

            string column = useCode ? GetCodeColumnName(catalog) : GetIdColumnName(catalog);

            string query = !useCode
                ? $"SELECT COUNT(1) FROM {tableName} WHERE CAST({column} AS NVARCHAR(MAX)) = CAST(@Id AS NVARCHAR(MAX))"
                : $"SELECT COUNT(1) FROM {tableName} WHERE UPPER({column}) = UPPER(@Id)";
            var parameter = new { Id = id };


            int count = await cnn.QuerySingleAsync<int>(query, parameter);
            return count > 0;
        }

        private string GetIdColumnName(CatalogsEnum catalog)
        {
            return catalog switch
            {
                CatalogsEnum.AddressTypes => "AddressTypeId",
                CatalogsEnum.ContactTypes => "ContactTypeId",
                CatalogsEnum.Countries => "CountryId",
                CatalogsEnum.CustomerAssociationTypes => "CustomerAssociationTypeId",
                CatalogsEnum.Departments => "DepartmentId",
                CatalogsEnum.DocumentTypes => "DocumentTypeId",
                CatalogsEnum.EconomicActivities => "EconomicActivityId",
                CatalogsEnum.FIncomeTypes => "FIncomeTypeId",
                CatalogsEnum.Genders => "GenderId",
                CatalogsEnum.LegalFigures => "LegalFigureId",
                CatalogsEnum.MaritalStatuses => "MaritalStatusId",
                CatalogsEnum.MigratoryStatuses => "MigratoryStatusId",
                CatalogsEnum.Municipalities => "MunicipalityId",
                CatalogsEnum.SocietyTypes => "SocietyTypeId",
                CatalogsEnum.ProcessingCT => "PCTId",
                CatalogsEnum.DeliveryHours => "DeliveryHourId",
                CatalogsEnum.DeliveryModes => "DeliveryModeId",
                CatalogsEnum.Branches => "BranchId",
                CatalogsEnum.CardColors => "CardColorId",
                _ => "Id"
            };
        }

        private string GetCodeColumnName(CatalogsEnum catalog)
        {
            return catalog switch
            {
                CatalogsEnum.AddressTypes => "AddressTypeCode",
                CatalogsEnum.ContactTypes => "ContactTypeCode",
                CatalogsEnum.Countries => "Alpha3Code",
                CatalogsEnum.CustomerAssociationTypes => "CustomerATypeCode",
                CatalogsEnum.Departments => "DepartmentCode",
                CatalogsEnum.DocumentTypes => "DocumentTypeCode",
                CatalogsEnum.EconomicActivities => "EconomicActivityCode",
                CatalogsEnum.FIncomeTypes => "FIncomeTypeCode",
                CatalogsEnum.Genders => "GenderCode",
                CatalogsEnum.LegalFigures => "LegalFigureCode",
                CatalogsEnum.MaritalStatuses => "MaritalStatusCode",
                CatalogsEnum.MigratoryStatuses => "MigratoryStatusCode",
                CatalogsEnum.Municipalities => "MunicipalityCode",
                CatalogsEnum.SocietyTypes => "SocietyTypeCode",
                CatalogsEnum.DeliveryHours => "DeliveryHourCode",
                CatalogsEnum.DeliveryModes => "DeliveryModeCode",
                CatalogsEnum.Branches => "BranchCode",
                CatalogsEnum.CardColors => "CardColorCode",
                _ => "Id"
            };
        }

        private string GetTableName(CatalogsEnum catalog)
        {
            return catalog switch
            {
                CatalogsEnum.AddressTypes => "AddressTypes",
                CatalogsEnum.ContactTypes => "ContactTypes",
                CatalogsEnum.Countries => "Countries",
                CatalogsEnum.CustomerAssociationTypes => "CustomerAssociationTypes",
                CatalogsEnum.Departments => "Departments",
                CatalogsEnum.DocumentTypes => "DocumentTypes",
                CatalogsEnum.EconomicActivities => "EconomicActivities",
                CatalogsEnum.FIncomeTypes => "FIncomeTypes",
                CatalogsEnum.Genders => "Genders",
                CatalogsEnum.LegalFigures => "LegalFigures",
                CatalogsEnum.MaritalStatuses => "MaritalStatuses",
                CatalogsEnum.MigratoryStatuses => "MigratoryStatuses",
                CatalogsEnum.Municipalities => "Municipalities",
                CatalogsEnum.ProcessingCT => "ProcessingCT",
                CatalogsEnum.DeliveryHours => "DeliveryHours",
                CatalogsEnum.DeliveryModes => "DeliveryModes",
                CatalogsEnum.Branches => "Branches",
                CatalogsEnum.CardColors => "CardColors",
                CatalogsEnum.SocietyTypes => "SocietyTypes",
                _ => string.Empty
            };
        }

    }
}
