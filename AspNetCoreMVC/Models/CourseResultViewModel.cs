namespace AspNetCoreMVC.Models
{
    public class CourseResultViewModel
    {
        public string CourseName { get; init; }
        public int TraineeId { get; init; }
        public string TraineeName { get; init; }
        public double Score { get; init; }
        public bool IsSucceeded { get; init; }

        public CourseResultViewModel(CourseResult result)
        {
            CourseName = result.Course.Name;
            TraineeId = result.TraineeId;
            TraineeName = result.Trainee.Name;
            Score = result.Score;
            IsSucceeded = Score >= result.Course.SuccessMark;
        }
    }
}
