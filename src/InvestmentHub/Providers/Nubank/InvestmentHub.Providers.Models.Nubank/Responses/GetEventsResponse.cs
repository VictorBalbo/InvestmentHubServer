using System.Collections.Generic;

namespace InvestmentHub.Providers.Models.Nubank.Responses
{
    public class GetEventsResponse : BaseResponse
    {
        public IEnumerable<Event> Events { get; set; }
    }
}
