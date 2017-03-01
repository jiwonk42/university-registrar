using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace UniversityRegistrar
{
  public class Course
  {
    private int _id;
    private string _name;

    public Course(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherCourse)
    {
        if (!(otherCourse is Course))
        {
          return false;
        }
        else
        {
          Course newCourse = (Course) otherCourse;
          bool idEquality = this.GetId() == newCourse.GetId();
          bool nameEquality = this.GetName() == newCourse.GetName();
          return (idEquality && nameEquality);
        }
    }

    public override int GetHashCode()
    {
        return this.GetName().GetHashCode();
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int CourseId = rdr.GetInt32(0);
        string CourseName = rdr.GetString(1);
        Course newCourse = new Course(CourseName, CourseId);
        allCourses.Add(newCourse);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCourses;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (name) OUTPUT INSERTED.id VALUES (@CourseName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CourseName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Course Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId;", conn);

      cmd.Parameters.Add(new SqlParameter("@CourseId", id.ToString()));

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCourseId = 0;
      string foundCourseName = null;

      while(rdr.Read())
      {
        foundCourseId = rdr.GetInt32(0);
        foundCourseName = rdr.GetString(1);
      }
      Course foundCourse = new Course(foundCourseName, foundCourseId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCourse;
    }

    public void AddStudent(Student newStudent)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (course_id, Student_id) VALUES (@CourseId, @StudentId);", conn);
      cmd.Parameters.Add(new SqlParameter("@CourseId", this.GetId()));
      cmd.Parameters.Add(new SqlParameter("@StudentId", newStudent.GetId()));

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Student> GetStudents()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses JOIN students_courses ON (courses.id = students_courses.course_id) JOIN students ON (students_courses.student_id = students.id) WHERE courses.id = @CourseId;", conn);
    //   SqlCommand cmd = new SqlCommand("SELECT student_id FROM students_courses WHERE course_id = @CourseId;", conn);
      cmd.Parameters.Add(new SqlParameter("@CourseId", this.GetId().ToString()));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Student> students = new List<Student>{};

      while(rdr.Read())
      {
          int studentId = rdr.GetInt32(0);
          string studentName = rdr.GetString(1);
          string studentDate = rdr.GetString(2);
          Student newStudent = new Student(studentName, studentDate, studentId);
          students.Add(newStudent);
      }

      if (rdr != null)
      {
          rdr.Close();
      }
      if (conn != null)
      {
          conn.Close();
      }
      return students;
    }

    //   List<int> studentIds = new List<int> {};
    //   while(rdr.Read())
    //   {
    //     int studentId = rdr.GetInt32(0);
    //     studentIds.Add(studentId);
    //   }
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   List<Student> students = new List<Student> {};
    //   foreach (int studentId in studentIds)
    //   {
    //     SqlCommand studentQuery = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
      //
    //     studentQuery.Parameters.Add(new SqlParameter("@StudentId", studentId));
      //
    //     SqlDataReader queryReader = studentQuery.ExecuteReader();
    //     while(queryReader.Read())
    //     {
    //           int thisStudentId = queryReader.GetInt32(0);
    //           string studentName = queryReader.GetString(1);
    //           string studentDate = queryReader.GetString(2);
    //           Student foundStudent = new Student(studentName, studentDate, thisStudentId);
    //           students.Add(foundStudent);
    //     }
    //     if (queryReader != null)
    //     {
    //       queryReader.Close();
    //     }
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return students;
    // }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM courses WHERE id = @CourseId; DELETE FROM students_courses WHERE course_id = @CourseId;", conn);

      cmd.Parameters.Add(new SqlParameter("@CourseId", this.GetId()));

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
