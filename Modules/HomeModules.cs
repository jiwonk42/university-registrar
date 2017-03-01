using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;


namespace UniversityRegistrar
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                List<Course> AllCourses = Course.GetAll();
                return View["index.cshtml", AllCourses];
            };
            // Get["/students"] = _ =>
            // {
            //     List<Student> AllStudents = Student.GetAll();
            //     return View["students.cshtml", AllStudents];
            // };    
        }


    }
}
