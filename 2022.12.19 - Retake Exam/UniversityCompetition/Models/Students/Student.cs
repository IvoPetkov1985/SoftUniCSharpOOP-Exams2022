using System;
using System.Collections.Generic;
using UniversityCompetition.Models.Contracts;

namespace UniversityCompetition.Models.Students
{
    public class Student : IStudent
    {
        private string firstName;
        private string lastName;
        private readonly List<int> coveredExams;

        public Student(int studentId, string firstName, string lastName)
        {
            Id = studentId;
            FirstName = firstName;
            LastName = lastName;
            coveredExams = new List<int>();
        }

        public int Id { get; private set; }

        public string FirstName
        {
            get => firstName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be null or whitespace!");
                }

                firstName = value;
            }
        }

        public string LastName
        {
            get => lastName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be null or whitespace!");
                }

                lastName = value;
            }
        }

        public IReadOnlyCollection<int> CoveredExams
            => coveredExams.AsReadOnly();

        public IUniversity University { get; private set; }

        public void CoverExam(ISubject subject)
        {
            coveredExams.Add(subject.Id);
        }

        public void JoinUniversity(IUniversity university)
        {
            University = university;
        }
    }
}
