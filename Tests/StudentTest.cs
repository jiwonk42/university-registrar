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
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Student firstStudent = new Student("Matt Caswell", "04/01/2017");
      Student secondStudent = new Student("Matt Caswell", "04/01/2017");


      //Assert
      Assert.Equal(firstStudent, secondStudent);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Student testStudent = new Student("Jasper", "07/24/2017");

      //Act
      testStudent.Save();
      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      Assert.Equal(testList, result);
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

        //Assert
        Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindStudentInDatabase()
    {
        //Arrange
        Student testStudent = new Student("Jasper", "07/24/2017");
        testStudent.Save();

        //Act
        Student foundStudent = Student.Find(testStudent.GetId());

        //Assert
        Assert.Equal(testStudent, foundStudent);
    }

    [Fact]
    public void Test_AddCourse_AddsCourseToStudent()
    {
      //Arrange
      Student testStudent = new Student("Jasper", "07/24/2017");
      testStudent.Save();

      Course testCourse = new Course("Intro to CS");
      testCourse.Save();

      //Act
      testStudent.AddCourse(testCourse);

      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course>{testCourse};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetCourses_ReturnsAllStudentCourses()
    {
      //Arrange
      Student testStudent = new Student("Jasper", "07/24/2017");
      testStudent.Save();

      Course testCourse1 = new Course("Intro to CS");
      testCourse1.Save();

      Course testCourse2 = new Course("Accounting");
      testCourse2.Save();

      //Act
      testStudent.AddCourse(testCourse1);
      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course> {testCourse1};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Delete_DeletesStudentAssociationsFromDatabase()
    {
      //Arrange
      Course testCourse = new Course("Intro to CS");
      testCourse.Save();

      string testName = "Accounting";
      string testDate = "01/03/2017";
      Student testStudent = new Student(testName, testDate);
      testStudent.Save();

      //Act
      testStudent.AddCourse(testCourse);
      testStudent.Delete();

      List<Student> resultCourseStudents = testCourse.GetStudents();
      List<Student> testCourseStudents = new List<Student> {};

      //Assert
      Assert.Equal(testCourseStudents, resultCourseStudents);
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
