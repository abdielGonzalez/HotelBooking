using Bancatlan.BusinessCC.Application.Features.LegalCustomers.CreateLegalCustomer.Dtos;
using Bancatlan.BusinessCC.Application.Features.LegalCustomers.CreateLegalCustomer;
using Swashbuckle.AspNetCore.Filters;

namespace Bancatlan.BusinessCC.Api.Swagger.Examples
{
    public class CreateLegalCustomerCommandExample
            : IMultipleExamplesProvider<CreateLegalCustomerCommand>
    {
        public IEnumerable<SwaggerExample<CreateLegalCustomerCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "New Customer Legal",
                new CreateLegalCustomerCommand
            (
                new LegalCustomerDto(
                    0, // LegalCustomerId
                    "202406011", // ClientId
                    10, // EconomicActivityId
                    222, // NationalityId
                    "Farmacia San Rey S.A. de C.V.", // LcFullName
                    "San Rey", // LcShortName
                    new DateOnly(2010, 5, 12), // DateOfIncorporation
                    1, // LegalFigureId
                    1, // SocietyTypeId
                    new List<IdentityDocumentDto>
                    {
                            new IdentityDocumentDto(
                                "ID-001",                // TCECreatorIdentityDocumentID
                                "0614010220001609",            // Document
                                new DateTime(2070, 12, 31), // ExpirationDate
                                2,                       // IDTypeId
                                new DateTime(2010, 5, 12), // IssuedDate
                                113                      // MunicipalityId
                            )
                    },
                    new List<ContactDto>
                    {
                            new ContactDto("CON003", 503, 2, "78903458", false, false), // TCECreatorContactID,AreaCode, ContactTypeId, Contact, NotificationTran, NotificationAdvertising
                            new ContactDto("CON004", null, 1, "farmaciasanrey@gmail.com", false, false)
                    },
                    new List<AddressDto>
                    {
                            new AddressDto(
                                "ADD001", // TCECreatorAddressID
                                2,         // AddressTypeId
                                110,           //MunicipalityId
                                "Residencial San Francisco",
                                "Avenida Central",
                                "Casa #12",
                                "Cerca del monumento X",
                                "13.692940",
                                "-89.218191",
                                true,
                                "Residencia Principal"
                            )
                    },
                    new List<NaturalCustomerDto>
                    {
                            new NaturalCustomerDto(
                                "NAT001", // TCECreatorNaturalCustomerID 
                                222,
                                2,
                                1,
                                1,
                                new DateOnly(1990, 1, 15),
                                "Roberto",
                                "Carlos",
                                "Gomez",
                                "Bolaños",
                                "",
                                null,
                                1,
                                new List<IdentityDocumentDto>
                                {
                                    new IdentityDocumentDto(
                                        "ID-002", // TCECreatorIdentityDocumentID
                                        "023456339",
                                        new DateTime(2028, 6, 30),
                                        1,
                                        new DateTime(2015, 3, 20),
                                        113
                                    )
                                },
                                new List<ContactDto>
                                {
                                    new ContactDto("CON005", 503, 5, "71234544", true, false),
                                    new ContactDto("CON006", null, 6, "roberto.gomez@gmail.com", false, true)
                                },
                                new List<AddressDto>
                                {
                                    new AddressDto(
                                        "ADD002", // TCECreatorAddressID
                                        1,         // AddressTypeId
                                        110,           //MunicipalityId
                                        "Residencial San Francisco",
                                        "Avenida Central",
                                        "Apartamento 5B",
                                        "Frente al parque",
                                        "13.692950",
                                        "-89.218200",
                                        false,
                                        "Casa de Juan"
                                    )
                                }
                            )
                    }
                ),
                "usuarioAPI"
            )
            );

            yield return SwaggerExample.Create(
                "LegalCustomerId and NaturalCustomers",
                new CreateLegalCustomerCommand
            (
                new LegalCustomerDto(
                    41568, // LegalCustomerId
                    null, // ClientId
                    null, // EconomicActivityId
                    null, // NationalityId
                    null, // LcFullName
                    null, // LcShortName
                    null, // DateOfIncorporation
                    null, // LegalFigureId
                    null, // SocietyTypeId
                    null, // IdentityDocuments
                    null, // Contacts
                    null, // Addresses
                    new List<NaturalCustomerDto>
                    {
                            new NaturalCustomerDto(
                                "NAT001", // TCECreatorNaturalCustomerID 
                                222,
                                2,
                                1,
                                1,
                                new DateOnly(1990, 1, 15),
                                "Mario",
                                "Carlos",
                                "Gomez",
                                "Bolaños",
                                "",
                                null,
                                1,
                                new List<IdentityDocumentDto>
                                {
                                    new IdentityDocumentDto(
                                        "ID-002", // TCECreatorIdentityDocumentID
                                        "023456339",
                                        new DateTime(2030, 6, 30),
                                        1,
                                        new DateTime(2025, 3, 20),
                                        113
                                    )
                                },
                                new List<ContactDto>
                                {
                                    new ContactDto("CON005", 503, 5, "71234644", true, false),
                                    new ContactDto("CON006", null, 6, "mario.gomez@gmail.com", false, true)
                                },
                                new List<AddressDto>
                                {
                                    new AddressDto(
                                        "ADD002", // TCECreatorAddressID
                                        1,         // AddressTypeId
                                        110,           //MunicipalityId
                                        "Residencial Luz ",
                                        "Avenida Central",
                                        "Apartamento 5B",
                                        "Frente al parque",
                                        "13.692950",
                                        "-89.218200",
                                        false,
                                        "Casa de Juan"
                                    )
                                }
                            )
                    }
                ),
                "usuarioAPI"
            )
                );
        }
    }
}
