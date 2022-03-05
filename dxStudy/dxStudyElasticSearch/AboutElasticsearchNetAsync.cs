using Elasticsearch.Net;

namespace dxStudyElasticSearch;
public class AboutElasticsearchNetAsync
{
    public async void IndexDataAsync()
    {
        var personElastic = new Person { ID = 1, Name = "dingxu1", Address = "dingxu1" };
        using (var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/")))
        {
            var lowlevelClient = new ElasticLowLevelClient(settings);
            //var awaitResult = await lowlevelClient.IndexAsync<BytesResponse>("people", "1", PostData.Serializable(personElastic));
            //var bytes = awaitResult.Body;
            var indexAwait = await lowlevelClient.IndexAsync<StringResponse>("dingxu", "1", PostData.Serializable(personElastic));
            var indexSuccessful = indexAwait.Success;
            var bytes2 = indexAwait.Body;
        }
    }

    public async void BulkIndexDataAsync()
    {
        var peopleArray = new object[]
        {
            new { index = new{ _index="dingxu", _type="person", _id = "2" } },
            new {ID = 2, Name= "dingxu2", Address = "dingxu2" },
            new { index = new{ _index="dingxu", _type="person", _id = "3" } },
            new {ID = 3, Name= "dingxu3", Address = "dingxu3" },
            new { index = new{ _index="dingxu", _type="person", _id = "4" } },
            new {ID = 4, Name= "dingxu4", Address = "dingxu4" },
            new { index = new{ _index="dingxu", _type="person", _id = "5" } },
            new {ID = 5, Name= "dingxu5", Address = "dingxu5" },
            new { index = new{ _index="dingxu", _type="person", _id = "6" } },
            new {ID = 6, Name= "dingxu6", Address = "dingxu6" }
        };

        using (var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/")))
        {
            var lowlevelClient = new ElasticLowLevelClient(settings);
            var bulkAwait = await lowlevelClient.BulkAsync<StringResponse>(PostData.Serializable(peopleArray));
            var bulkSuccessful = bulkAwait.Success;
            var bytes2 = bulkAwait.Body;
        }
    }

    public async void SearchResponseAsync()
    {
        var personElastic = new Person { ID = 1, Name = "dingxu1", Address = "dingxu1" };
        using (var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/")))
        {
            var lowlevelClient = new ElasticLowLevelClient(settings);
            var searchResponseAwait = await lowlevelClient.SearchAsync<StringResponse>("dingxu", PostData.Serializable(new
            {
                from = 0,
                size = 10,
                query = new
                {
                    match = new
                    {
                        name = "dingxu1"
                    }
                }
            }));

            var searchSuccessful = searchResponseAwait.Success;
            var searchResponseBody = searchResponseAwait.Body;
        }
    }

    public async void DeleteResponseAsync()
    {
        using (var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/")))
        {
            var lowlevelClient = new ElasticLowLevelClient(settings);
            var deleteResponseAwait = await lowlevelClient.DeleteAsync<StringResponse>("dingxu", "1");
            var deleteSuccessful = deleteResponseAwait.Success;
            var deleteResponseBody = deleteResponseAwait.Body;
        }
    }
}
