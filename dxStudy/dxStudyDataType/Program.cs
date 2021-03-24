using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace dxStudyDataType
{
    class Program
    {
        static void Main(string[] args)
        {
            var personDataOperation = new PersonDataOperation();
            var listPerson = personDataOperation.CreatePersonListData();
            var personDataTable = personDataOperation.CreatePersonDataTable(listPerson);

            string strConnectionStr = "server=.;database=dxStudy;uid=sa;pwd=dx6301659;";
            var sqlConnection = new SqlConnection(strConnectionStr);
            var sqlCommand = new SqlCommand("P_PERSON_INSERT_BY_DATATYPE", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var sqlParameter = new SqlParameter("@PersonData", SqlDbType.Structured);
            sqlParameter.Value = personDataTable;
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
        private DataTable CreatePersonTableStructure()
        {
            var dtResult = new DataTable("T_PERSON");
            var dcID = new DataColumn("ID");
            var dcName = new DataColumn("NAME");
            var dcAge = new DataColumn("AGE");
            var dcCreatedBy = new DataColumn("CREATED_BY");
            var dcCreatedTime = new DataColumn("CREATED_TIME");
            dtResult.Columns.Add(dcID);
            dtResult.Columns.Add(dcName);
            dtResult.Columns.Add(dcAge);
            dtResult.Columns.Add(dcCreatedBy);
            dtResult.Columns.Add(dcCreatedTime);
            return dtResult;
        }

        public DataTable CreatePersonDataTable(List<Person> listPerson)
        {
            if (listPerson == null || listPerson.Count == 0)
                return null;

            var dtResult = CreatePersonTableStructure();
            foreach (var item in listPerson)
            {
                if (item == null)
                    continue;

                var dr = dtResult.NewRow();
                dr["ID"] = item.ID;
                dr["NAME"] = item.Name;
                dr["AGE"] = item.Age;
                dr["CREATED_BY"] = item.CreatedBy;
                dr["CREATED_TIME"] = item.CreatedTime;
                dtResult.Rows.Add(dr);
            }

            return dtResult;
        }

        public List<Person> CreatePersonListData()
        {
            var listPerson = new List<Person>();
            var person1 = new Person();
            person1.ID = Guid.NewGuid().ToString();
            person1.Name = "dingxu1";
            person1.Age = 34;
            person1.CreatedBy = "丁旭";
            person1.CreatedTime = DateTime.Now;
            listPerson.Add(person1);

            var person2 = new Person();
            person2.ID = Guid.NewGuid().ToString();
            person2.Name = "dingxu2";
            person2.Age = 34;
            person2.CreatedBy = "丁旭";
            person2.CreatedTime = DateTime.Now;
            listPerson.Add(person2);

            var person3 = new Person();
            person3.ID = Guid.NewGuid().ToString();
            person3.Name = "dingxu3";
            person3.Age = 34;
            person3.CreatedBy = "丁旭";
            person3.CreatedTime = DateTime.Now;
            listPerson.Add(person3);

            return listPerson;
        }
    }
}
