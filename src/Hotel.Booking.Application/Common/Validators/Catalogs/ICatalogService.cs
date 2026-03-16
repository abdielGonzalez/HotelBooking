namespace Hotel.Booking.Application.Common.Validators.Catalogs
{
    public interface ICatalogService
    {
        Task<bool> ExistsAsync<TIdType>(CatalogsEnum catalog, TIdType id, bool useCode = false);

    }
}
