namespace OpenLatino.Core.Domain.Internationalization
{
    public interface ITranslation
    {
        int LanguageId { get; set; }
        int EntityId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
