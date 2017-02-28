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
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
        //Arrange, Act
        int result = Student.GetAll().Count;

        //Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
        //Arrange
        Student testStudent = new Student("Jasper", "07/24/2017");

        //Act
        testStudent.Save();
        Student savedStudent = Student.GetAll()[0];


        int result = savedStudent.GetId();
        int testId = testStudent.GetId();

        Assert.Equal(testId, result);
    }

    public void Dispose()
    {
      Student.DeleteAll();
    }
  }
}
