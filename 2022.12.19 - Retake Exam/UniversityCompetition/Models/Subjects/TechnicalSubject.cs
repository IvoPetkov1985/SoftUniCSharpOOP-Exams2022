namespace UniversityCompetition.Models.Subjects
{
    public class TechnicalSubject : Subject
    {
        private const double TechnicalSubjectRate = 1.3;

        public TechnicalSubject(int subjectId, string subjectName)
            : base(subjectId, subjectName, TechnicalSubjectRate)
        {
        }
    }
}
