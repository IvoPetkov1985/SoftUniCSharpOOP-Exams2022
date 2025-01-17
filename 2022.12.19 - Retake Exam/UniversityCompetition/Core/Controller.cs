using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityCompetition.Core.Contracts;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Models.Students;
using UniversityCompetition.Models.Subjects;
using UniversityCompetition.Models.Universities;
using UniversityCompetition.Repositories;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Core
{
    public class Controller : IController
    {
        private readonly IRepository<ISubject> subjects;
        private readonly IRepository<IStudent> students;
        private readonly IRepository<IUniversity> universities;

        public Controller()
        {
            subjects = new SubjectRepository();
            students = new StudentRepository();
            universities = new UniversityRepository();
        }

        public string AddStudent(string firstName, string lastName)
        {
            string studentName = $"{firstName} {lastName}";
            IStudent student = students.FindByName(studentName);

            if (student != null)
            {
                return $"{firstName} {lastName} is already added in the repository.";
            }

            int studentId = students.Models.Count + 1;
            student = new Student(studentId, firstName, lastName);
            students.AddModel(student);
            return $"Student {firstName} {lastName} is added to the {students.GetType().Name}!";
        }

        public string AddSubject(string subjectName, string subjectType)
        {
            if (subjectType != nameof(TechnicalSubject) &&
                subjectType != nameof(EconomicalSubject) &&
                subjectType != nameof(HumanitySubject))
            {
                return $"Subject type {subjectType} is not available in the application!";
            }

            ISubject subject = subjects.FindByName(subjectName);

            if (subject != null)
            {
                return $"{subjectName} is already added in the repository.";
            }

            int subjectId = subjects.Models.Count + 1;

            if (subjectType == nameof(TechnicalSubject))
            {
                subject = new TechnicalSubject(subjectId, subjectName);
            }
            else if (subjectType == nameof(EconomicalSubject))
            {
                subject = new EconomicalSubject(subjectId, subjectName);
            }
            else
            {
                subject = new HumanitySubject(subjectId, subjectName);
            }

            subjects.AddModel(subject);
            return $"{subjectType} {subjectName} is created and added to the {subjects.GetType().Name}!";
        }

        public string AddUniversity(string universityName, string category, int capacity, List<string> requiredSubjects)
        {
            IUniversity university = universities.FindByName(universityName);

            if (university != null)
            {
                return $"{universityName} is already added in the repository.";
            }

            List<int> subjectIds = new();

            foreach (string subjectName in requiredSubjects)
            {
                ISubject subject = subjects.FindByName(subjectName);
                subjectIds.Add(subject.Id);
            }

            int universityId = universities.Models.Count + 1;
            university = new University(universityId, universityName, category, capacity, subjectIds);
            universities.AddModel(university);
            return $"{universityName} university is created and added to the {universities.GetType().Name}!";
        }

        public string ApplyToUniversity(string studentName, string universityName)
        {
            IStudent student = students.FindByName(studentName);

            string[] names = studentName.Split(" ");
            string firstName = names[0];
            string lastName = names[1];

            if (student == null)
            {
                return $"{firstName} {lastName} is not registered in the application!";
            }

            IUniversity university = universities.FindByName(universityName);

            if (university == null)
            {
                return $"{universityName} is not registered in the application!";
            }

            foreach (int sub in university.RequiredSubjects)
            {
                if (student.CoveredExams.Contains(sub) == false)
                {
                    return $"{studentName} has not covered all the required exams for {universityName} university!";
                }
            }

            if (student.University == university)
            {
                return $"{firstName} {lastName} has already joined {universityName}.";
            }

            student.JoinUniversity(university);
            return $"{firstName} {lastName} joined {universityName} university!";
        }

        public string TakeExam(int studentId, int subjectId)
        {
            IStudent student = students.FindById(studentId);

            if (student == null)
            {
                return "Invalid student ID!";
            }

            ISubject subject = subjects.FindById(subjectId);

            if (subject == null)
            {
                return "Invalid subject ID!";
            }

            if (student.CoveredExams.Contains(subjectId))
            {
                return $"{student.FirstName} {student.LastName} has already covered exam of {subject.Name}.";
            }

            student.CoverExam(subject);
            return $"{student.FirstName} {student.LastName} covered {subject.Name} exam!";
        }

        public string UniversityReport(int universityId)
        {
            IUniversity university = universities.FindById(universityId);

            StringBuilder builder = new();
            builder.AppendLine($"*** {university.Name} ***");
            builder.AppendLine($"Profile: {university.Category}");

            int studentsCount = students.Models.Where(s => s.University == university).Count();

            builder.AppendLine($"Students admitted: {studentsCount}");
            builder.AppendLine($"University vacancy: {university.Capacity - studentsCount}");

            return builder.ToString().TrimEnd();
        }
    }
}
