// using Xunit;
// using System.Collections.Generic;
// using System;
// using System.Data;
// using System.Data.SqlClient;
//
// namespace UniversityRegistrar
// {
//   public class CourseTest : IDisposable
//   {
//     public CourseTest()
//     {
//       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_registrar_test;Integrated Security=SSPI;";
//     }
//
//     [Fact]
//     public void Test_CoursesEmptyAtFirst()
//     {
//       //Arrange, Act
//       int result = Course.GetAll().Count;
//
//       //Assert
//       Assert.Equal(0, result);
//     }
//
//     [Fact]
//     public void Test_Equal_ReturnsTrueForSameName()
//     {
//       //Arrange, Act
//       Course firstCourse = new Course("Intro to CS");
//       Course secondCourse = new Course("Intro to CS");
//
//       //Assert
//       Assert.Equal(firstCourse, secondCourse);
//     }
//
//     [Fact]
//     public void Test_Save_SavesCourseToDatabase()
//     {
//       //Arrange
//       Course testCourse = new Course("Intro To CS");
//       testCourse.Save();
//
//       //Act
//       List<Course> result = Course.GetAll();
//       List<Course> testList = new List<Course>{testCourse};
//
//       //Assert
//       Assert.Equal(testList, result);
//     }
//
//     [Fact]
//     public void Test_Save_AssignsIdToCourseObject()
//     {
//       //Arrange
//       Course testCourse = new Course("Intro To CS");
//       testCourse.Save();
//
//       //Act
//       Course savedCourse = Course.GetAll()[0];
//
//       int result = savedCourse.GetId();
//       int testId = testCourse.GetId();
//
//       //Assert
//       Assert.Equal(testId, result);
//     }
//
//     [Fact]
//     public void Test_Find_FindsCourseInDatabase()
//     {
//       //Arrange
//       Course testCourse = new Course("Intro To CS");
//       testCourse.Save();
//
//       //Act
//       Course foundCourse = Course.Find(testCourse.GetId());
//
//       //Assert
//       Assert.Equal(testCourse, foundCourse);
//     }
//
//     public void Dispose()
//     {
//       Task.DeleteAll();
//       Course.DeleteAll();
//     }
//   }
// }
