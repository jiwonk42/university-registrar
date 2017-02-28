using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversityRegistrar
{
  public class UniversityRegistrarTest : IDisposable
  {
    public UniversityRegistrarTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_registrar;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
        //Arrange, Act
        int result = Student.GetAll().Count;

        //Assert
        Assert.Equal(0, result);
    }

    public void Dispose()
    {
      Student.DeleteAll();
    }
  }
}
