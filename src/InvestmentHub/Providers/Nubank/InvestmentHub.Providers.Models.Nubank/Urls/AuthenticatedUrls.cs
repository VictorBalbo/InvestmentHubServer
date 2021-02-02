namespace InvestmentHub.Providers.Models.Nubank.Urls
{
    public class AuthenticatedUrls
    {
        public AuthenticatedUrlsContainer Events { get; set; }
        public AuthenticatedUrlsContainer Ghostflame { get; set; }
    }

    public class AuthenticatedUrlsContainer
    {
        public string Href { get; set; }

        public override string ToString()
        {
            return Href;
        }

        public static implicit operator string(AuthenticatedUrlsContainer value) => value?.ToString();
        public static implicit operator AuthenticatedUrlsContainer(string value) => new AuthenticatedUrlsContainer
        {
            Href = value
        };
    }
}
