using Nest;

namespace dxStudyElasticSearch
{
    public class AboutNESTAsync
    {
        //Add and Update Index
        public async void IndexDataAsync()
        {
            var listPerson = Utility.CreateData();
            var client = Utility.CreateElasticClient();

            BulkResponse response;
            try
            {
                response = await client.IndexManyAsync(listPerson, "dingxu");
                //response = await client.BulkAllAsync(listPerson, bulkAllDesc => bulkAllDesc.Index("dingxu"));
                bool blnResult = response.IsValid;
            }
            catch
            {
                throw;
            }
        }

        public async void GetIndex()
        {
            var client = Utility.CreateElasticClient();

            try
            {
                var response = await client.GetAsync<Person>(1, getDesc => getDesc.Index("dingxu"));
            }
            catch
            {
                throw;
            }
        }

        public async void SearchIndex()
        {
            var client = Utility.CreateElasticClient();

            try
            {
                var response = await client.SearchAsync<Person>(searchDesc => searchDesc.Index("dingxu"));
            }
            catch
            {
                throw;
            }
        }

        public async void SearchIndex2()
        {
            var listPerson = Utility.CreateData();
            var client = Utility.CreateElasticClient();

            try
            {
                var response = await client.SearchAsync<Person>(searchDesc => searchDesc.Index("dingxu").Query(entity => entity.Term(t => t.Name, "dingxu2")));
            }
            catch
            {
                throw;
            }
        }

        public async void SearchIndex3()
        {
            var request = new SearchRequest("dingxu")
            {
                From = 0,
                Size = 10,
                Query = new TermQuery { Field = "Name", Value = "dingxu3" } ||
                        new MatchQuery { Field = "Address", Query = "dingxu3" }
            };

            var client = Utility.CreateElasticClient();

            try
            {
                var response = await client.SearchAsync<Person>(request);
            }
            catch
            {
                throw;
            }
        }

        public async void UpdateIndex()
        {
            var person = new Person { ID = 4, Name = "dingxuTest4", Address = "dingxuTest4" };
            var client = Utility.CreateElasticClient();

            try
            {
                var response = await client.UpdateAsync<Person>("4", updateDesc => updateDesc.Index("dingxu").Source(false).Doc(person));
            }
            catch
            {
                throw;
            }
        }

        public async void DeleteIndex()
        {
            var person1 = new Person { ID = 1 };
            var person2 = new Person { ID = 8 };
            var listDeletedPerson = new List<Person> { person1, person2 };
            var client = Utility.CreateElasticClient();

            try
            {
                //var response = await client.Delete<Person>("5", deleteDesc => deleteDesc.Index("dingxu"));
                var response = await client.DeleteManyAsync(listDeletedPerson, "dingxu");
            }
            catch
            {
                throw;
            }
        }

        public async void DeleteAllIndex()
        {
            var deleteByQueryRequest = new DeleteByQueryRequest("dingxu");
            deleteByQueryRequest.QueryOnQueryString = "*";
            var client = Utility.CreateElasticClient();

            try
            {
                //var response = await client.DeleteByQuery<Person>(delQueryDesc => delQueryDesc.Index("dingxu").Query(queryDesc => queryDesc.QueryString(q => q.Query("*")))); ;
                var response2 =await client.DeleteByQueryAsync(deleteByQueryRequest);
            }
            catch
            {
                throw;
            }
        }
    }
}
