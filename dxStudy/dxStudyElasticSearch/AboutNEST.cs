using Nest;

namespace dxStudyElasticSearch
{
    public class AboutNEST
    {
        //Add or Update Index
        public void IndexData()
        {
            var listPerson = Utility.CreateData();
            var client = Utility.CreateElasticClient();

            BulkResponse response;
            try
            {
                response = client.IndexMany(listPerson, "dingxu");
                //response = client.BulkAll(listPerson, bulkAllDesc => bulkAllDesc.Index("dingxu"));
            }
            catch
            {
                throw;
            }

            bool blnResult = response.IsValid;
        }

        public void GetIndex()
        {
            var client = Utility.CreateElasticClient();

            try
            {
                var response = client.Get<Person>(1, getDesc => getDesc.Index("dingxu"));
            }
            catch
            {
                throw;
            }
        }

        public void SearchIndex()
        {
            var client = Utility.CreateElasticClient();

            try
            {
                var response = client.Search<Person>(searchDesc => searchDesc.Index("dingxu"));
            }
            catch
            {
                throw;
            }
        }

        public void SearchIndex2()
        {
            var listPerson = Utility.CreateData();
            var client = Utility.CreateElasticClient();

            try
            {
                var response = client.Search<Person>(searchDesc => searchDesc.Index("dingxu").Query(entity => entity.Term(t => t.Name, "dingxu2")));
            }
            catch
            {
                throw;
            }
        }

        public void SearchIndex3()
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
                var response = client.Search<Person>(request);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateIndex()
        {
            var person = new Person { ID = 4, Name = "dingxuTest4", Address = "dingxuTest4" };
            var client = Utility.CreateElasticClient();

            try
            {
                var response = client.Update<Person>("4", updateDesc => updateDesc.Index("dingxu").Source(false).Doc(person));
            }
            catch
            {
                throw;
            }
        }

        public void DeleteIndex()
        {
            var person1 = new Person { ID = 1 };
            var person2 = new Person { ID = 8 };
            var listDeletedPerson = new List<Person> { person1, person2 };
            var client = Utility.CreateElasticClient();

            try
            {
                //var response = client.Delete<Person>("5", deleteDesc => deleteDesc.Index("dingxu"));
                var response = client.DeleteMany(listDeletedPerson, "dingxu");
            }
            catch
            {
                throw;
            }
        }

        public void DeleteAllIndex()
        {
            var deleteByQueryRequest = new DeleteByQueryRequest("dingxu");
            deleteByQueryRequest.QueryOnQueryString = "*";
            var client = Utility.CreateElasticClient();

            try
            {
                //var response = client.DeleteByQuery<Person>(delQueryDesc => delQueryDesc.Index("dingxu").Query(queryDesc => queryDesc.QueryString(q => q.Query("*")))); ;
                var response2 = client.DeleteByQuery(deleteByQueryRequest);
            }
            catch
            {
                throw;
            }
        }
    }
}
