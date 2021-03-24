using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace dxStudyOpenJson
{
    class Program
    {
        static void Main(string[] args)
        {
            var personDataOperation = new PersonDataOperation();
            var listPerson = personDataOperation.CreatePersonListData();
            string strJsonPersonsData = JsonConvert.SerializeObject(listPerson);

            string strConnectionStr = "server=.;database=dxStudy;uid=sa;pwd=dx6301659;";
            var sqlConnection = new SqlConnection(strConnectionStr);
            var sqlCommand = new SqlCommand("P_PERSON_INSERT_BY_OPENJSON", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var sqlParameter = new SqlParameter("@PersonJsonData", strJsonPersonsData);
            sqlCommand.Parameters.Add(sqlParameter);

            try
            {
                sqlConnection.Open();
                int intResult = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strErrMessage = ex.Message;
                Console.WriteLine(strErrMessage);
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            Console.WriteLine("Hello World!");
        }
    }

    public class Person
    {
        public string ID;
        public string Name;
        public int Age;
        public string CreatedBy;
        public DateTime CreatedTime;
    }

    public class PersonDataOperation
    {
        public List<Person> CreatePersonListData()
        {
            var listPerson = new List<Person>();
            var person1 = new Person();
            person1.ID = Guid.NewGuid().ToString();
            person1.Name = "dingxu11";
            person1.Age = 34;
            person1.CreatedBy = "丁旭";
            person1.CreatedTime = DateTime.Now;
            listPerson.Add(person1);

            var person2 = new Person();
            person2.ID = Guid.NewGuid().ToString();
            person2.Name = "dingxu12";
            person2.Age = 34;
            person2.CreatedBy = "丁旭";
            person2.CreatedTime = DateTime.Now;
            listPerson.Add(person2);

            var person3 = new Person();
            person3.ID = Guid.NewGuid().ToString();
            person3.Name = "dingxu13";
            person3.Age = 34;
            person3.CreatedBy = "丁旭";
            person3.CreatedTime = DateTime.Now;
            listPerson.Add(person3);

            return listPerson;
        }
    }
}
