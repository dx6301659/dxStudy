using Elasticsearch.Net;
using Nest;

namespace dxStudyElasticSearch
{
    public static class Utility
    {
        public static ElasticClient CreateElasticClient()
        {
            try
            {
                var uri = new Uri("http://localhost:9200");
                // can add more node: uri1, uri2, uri3.....
                var nodes = new Uri[] { uri };
                var pool = new StaticConnectionPool(nodes);
                var settings = new ConnectionSettings(pool);
                return new ElasticClient(settings);
            }
            catch
            {
                throw;
            }
        }

        public static IEnumerable<Person> CreateData()
        {
            var person1 = new Person { ID = 1, Name = "dingxu1", Address = "dingxu1" };
            var person2 = new Person { ID = 2, Name = "dingxu2", Address = "dingxu2" };
            var person3 = new Person { ID = 3, Name = "dingxu3", Address = "dingxu3" };
            var person4 = new Person { ID = 4, Name = "dingxu4", Address = "dingxu4" };
            var person5 = new Person { ID = 5, Name = "dingxuTest5", Address = "dingxu5" };
            var person6 = new Person { ID = 6, Name = "dingxu6", Address = "dingxu6" };
            var person7 = new Person { ID = 7, Name = "dingxu7", Address = "dingxutest7" };
            return new List<Person> { person1, person2, person3, person4, person5, person6, person7 };
        }
    }
}
