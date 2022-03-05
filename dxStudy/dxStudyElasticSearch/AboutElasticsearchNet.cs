using Elasticsearch.Net;

namespace dxStudyElasticSearch
{
    public class AboutElasticsearchNet
    {
        public void IndexData()
        {
            var person = new Person { ID = 1, Name = "dingxu1", Address = "dingxu1" };
            using (var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/")))
            {
                var lowlevelClient = new ElasticLowLevelClient(settings);
                //var awaitResult = await lowlevelClient.IndexAsync<BytesResponse>("people", "1", PostData.Serializable(personElastic));
                //var bytes = awaitResult.Body;
                var response = lowlevelClient.Index<StringResponse>("dingxu", "1", PostData.Serializable(person));
                var indexSuccessful = response.Success;
                var indexSuccessful2 = response.SuccessOrKnownError;
                var indexResponseBody = response.Body;
            }
        }

        //Bulk Index        
        public void BulkIndexData()
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
                var response = lowlevelClient.Bulk<StringResponse>(PostData.Serializable(peopleArray));
                var bulkSuccessful = response.Success;
                var bulkResponseBody = response.Body;
            }
        }

        //Search
        public void SearchResponse()
        {
            var personElastic = new Person { ID = 1, Name = "dingxu1", Address = "dingxu1" };
            using (var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/")))
            {
                var lowlevelClient = new ElasticLowLevelClient(settings);
                var response = lowlevelClient.Search<StringResponse>("dingxu", PostData.Serializable(new
                {
                    from = 0,
                    size = 10,
                    query = new
                    {
                        match = new
                        {
                            Name = "dingxu1"
                        }
                    }
                }));

                var searchSuccessful = response.Success;
                var searchSuccessful2 = response.SuccessOrKnownError;
                var searchResponseBody = response.Body;
            }
        }

        //Delete
        public void DeleteResponse()
        {
            using (var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/")))
            {
                var lowlevelClient = new ElasticLowLevelClient(settings);
                var response = lowlevelClient.Delete<StringResponse>("dingxu", "1");
                var deleteSuccessful = response.Success;
                var deleteSuccessful2 = response.SuccessOrKnownError;
                var deleteResponseBody = response.Body;
            }
        }

        //Update
        public void UpdateResponse()
        {
            var personElastic = new Person { ID = 1, Name = "dingxu1", Address = "dingxuTest1" };
            using (var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/")))
            {
                var lowlevelClient = new ElasticLowLevelClient(settings);
                var response = lowlevelClient.Update<StringResponse>("dingxu", "1", PostData.Serializable(personElastic));
                var updateSuccessful = response.Success;
                var updateSuccessful2 = response.SuccessOrKnownError;
                var updateResponseBody = response.Body;
            }
        }
    }
}
