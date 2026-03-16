using Bancatlan.BusinessCC.Application.Features.BusinessCC.CreateCreditCards;
using Bancatlan.BusinessCC.Application.Features.BusinessCC.CreateCreditCards.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Bancatlan.BusinessCC.Api.Swagger.Examples
{
    public class CreateCreditCardsExamples : IMultipleExamplesProvider<CreateCreditCardsCommand>
    {
        public IEnumerable<SwaggerExample<CreateCreditCardsCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Create a new primary cardholder card with additional cardholders",
                new CreateCreditCardsCommand(
                    Data: new List<DatumDto>
                    {
                        new DatumDto(
                            CustomerLegal: new MainBusinessCreditCardDto(
                                CustomerID: 41481,
                                CardId: null,
                                AccountNum: "0004513540019908171",
                                CardNum: "0004513540019908171",
                                AccountLimit: 10000,
                                PCTId: "A01",
                                EmbossedName: "Farmacia San Rey",
                                Day: new DateTime(2026, 2, 14),
                                DeliveryHourId: 0,
                                DeliveryModeId: 1,
                                BranchId: 1,
                                OwnerId: "r.hernandez",
                                Cards: new List<CardDto>
                                {
                                    new CardDto(
                                        CustomerID: 41482,
                                        CardColorId: 1,
                                        CardLimit: 2000,
                                        EmbossedName: "Juan Perez",
                                        IsNotificationTran: true,
                                        IsNotificationAdvertising: true,
                                        CardNum: "0004513540019908172",
                                        PhoneContactId: 127329,
                                        MailContactId: 127330,
                                        IsCardReplace: true,
                                        RelatedCreditCardId: 0
                                    )
                                }
                            )
                        )
                    }
                )
            );

            yield return SwaggerExample.Create(
                "Add extras to existing card",
                new CreateCreditCardsCommand(
                    Data: new List<DatumDto>
                    {
                        new DatumDto(
                            CustomerLegal: new MainBusinessCreditCardDto(
                                CustomerID: 41481,
                                CardId: 7602,
                                AccountNum: null,
                                CardNum: null,
                                AccountLimit: 10000,
                                PCTId: null,
                                EmbossedName: "Farmacia San Rey",
                                Day: new DateTime(2026, 2, 14),
                                DeliveryHourId: 0,
                                DeliveryModeId: 1,
                                BranchId: 1,
                                OwnerId: "r.hernandez",
                                Cards: new List<CardDto>
                                {
                                    new CardDto(
                                        CustomerID: 41482,
                                        CardColorId: 1,
                                        CardLimit: 2000,
                                        EmbossedName: "Juan Perez",
                                        IsNotificationTran: true,
                                        IsNotificationAdvertising: true,
                                        CardNum: "0004513540019908173",
                                        PhoneContactId: 127329,
                                        MailContactId: 127330,
                                        IsCardReplace: true,
                                        RelatedCreditCardId: 0
                                    )
                                }
                            )
                        )
                    }
                )
            );

            yield return SwaggerExample.Create(
                "Create a new primary cardholder card",
                new CreateCreditCardsCommand(
                    Data: new List<DatumDto>
                    {
                        new DatumDto(
                            CustomerLegal: new MainBusinessCreditCardDto(
                                CustomerID: 41481,
                                CardId: null,
                                AccountNum: "0004513540019908171",
                                CardNum: "0004513540019908171",
                                AccountLimit: 10000,
                                PCTId: "A01",
                                EmbossedName: "Farmacia San Rey",
                                Day: new DateTime(2026, 2, 14),
                                DeliveryHourId: 0,
                                DeliveryModeId: 1,
                                BranchId: 1,
                                OwnerId: "r.hernandez",
                                Cards: null
                            )
                        )
                    }
                )
            );
        }
    }
}