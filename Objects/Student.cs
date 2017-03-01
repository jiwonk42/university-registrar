using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace UniversityRegistrar
{
    public class Student
    {
        private int _id;
        private string _name;
        private string _date;

        public Student(string Name, string Date, int Id = 0)
        {
            _id = Id;
            _name = Name;
            _date = Date;
        }

        public override bool Equals(System.Object otherStudent)
        {
          if (!(otherStudent is Student))
          {
            return false;
          }
          else
          {
            Student newStudent = (Student) otherStudent;
            bool idEquality = (this.GetId() == newStudent.GetId());
            bool nameEquality = (this.GetName() == newStudent.GetName());
            bool dateEquality = (this.GetDate() == newStudent.GetDate());
            return (idEquality && nameEquality && dateEquality);
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

        public string GetDate()
        {
            return _date;
        }

        public void SetDate(string newDate)
        {
            _date = newDate;
        }

        public static List<Student> GetAll()
        {
            List<Student> AllStudents = new List<Student>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                string studentDate = rdr.GetString(2);
                Student newStudent = new Student(studentName, studentDate, studentId);
                AllStudents.Add(newStudent);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return AllStudents;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO students (name, date) OUTPUT INSERTED.id VALUES (@StudentName, @StudentDate);", conn);

            cmd.Parameters.Add(new SqlParameter("@StudentName", this.GetName()));
            cmd.Parameters.Add(new SqlParameter("@StudentDate", this.GetDate()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

        }

        public static Student Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);

            cmd.Parameters.Add(new SqlParameter("@StudentId", id.ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();
            int foundStudentId = 0;
            string foundStudentName = null;
            string foundStudentDate = null;
            while(rdr.Read())
            {
                foundStudentId = rdr.GetInt32(0);
                foundStudentName = rdr.GetString(1);
                foundStudentDate = rdr.GetString(2);
            }
            Student foundStudent = new Student(foundStudentName, foundStudentDate, foundStudentId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundStudent;
        }

        public void AddCourse(Course newCourse)
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId);", conn);
          cmd.Parameters.Add(new SqlParameter("@StudentId", this.GetId()));
          cmd.Parameters.Add(new SqlParameter("@CourseId", newCourse.GetId()));

          cmd.ExecuteNonQuery();

          if (conn != null)
          {
            conn.Close();
          }
        }

        public List<Course> GetCourses()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN students_courses ON (students.id = students_courses.student_id) JOIN courses ON (students_courses.course_id = courses.id) WHERE students.id = @StudentId;", conn);

            cmd.Parameters.Add(new SqlParameter("@StudentId", this.GetId().ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Course> courses = new List<Course>{};

            while(rdr.Read())
            {
                int courseId = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                Course newCourse = new Course(courseName, courseId);
                courses.Add(newCourse);

            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return courses;

        //   SqlConnection conn = DB.Connection();
        //   conn.Open();
          //
        //   SqlCommand cmd = new SqlCommand("SELECT course_id FROM students_courses WHERE student_id = @StudentId;", conn);
          //
        //   cmd.Parameters.Add(new SqlParameter("@StudentId", this.GetId()));
          //
        //   SqlDataReader rdr = cmd.ExecuteReader();
          //
        //   List<int> courseIds = new List<int> {};
          //
        //   while (rdr.Read())
        //   {
        //     int courseId = rdr.GetInt32(0);
        //     courseIds.Add(courseId);
        //   }
        //   if (rdr != null)
        //   {
        //     rdr.Close();
        //   }
          //
        //   List<Course> courses = new List<Course> {};
          //
        //   foreach (int courseId in courseIds)
        //   {
        //     SqlCommand courseQuery = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId;", conn);
          //
        //     courseQuery.Parameters.Add(new SqlParameter("@CourseId", courseId));
          //
        //     SqlDataReader queryReader = courseQuery.ExecuteReader();
        //     while (queryReader.Read())
        //     {
        //       int thisCourseId = queryReader.GetInt32(0);
        //       string courseName = queryReader.GetString(1);
        //       Course foundCourse = new Course(courseName, thisCourseId);
        //       courses.Add(foundCourse);
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
        //   return courses;
        }

        public void Delete()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("DELETE FROM students WHERE id = @StudentId; DELETE FROM students_courses WHERE student_id = @StudentId;", conn);

          cmd.Parameters.Add(new SqlParameter("@StudentId", this.GetId()));

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
          SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
          cmd.ExecuteNonQuery();
          conn.Close();
        }
    }
}
