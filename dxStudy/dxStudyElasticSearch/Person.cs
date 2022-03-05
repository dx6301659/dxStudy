using Nest;

namespace dxStudyElasticSearch
{
    [ElasticsearchType(IdProperty = nameof(ID))]
    public record Person
    {
        [Flattened(Name = "ID", Ignore = true)]
        public int ID { get; init; }

        [Flattened(Name = "Name")]
        public string? Name { get; init; }

        [Flattened(Name = "Address")]
        public string? Address { get; init; }
    }
}
