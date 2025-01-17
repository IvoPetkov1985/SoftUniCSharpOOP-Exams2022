namespace UniversityCompetition.Models.Subjects
{
    public class EconomicalSubject : Subject
    {
        private const double EconomicalSubjectRate = 1.0;

        public EconomicalSubject(int subjectId, string subjectName)
            : base(subjectId, subjectName, EconomicalSubjectRate)
        {
        }
    }
}
