namespace AspNetCoreMVC.Models
{
    public class CourseResult
    {
        public int CourseId { get; set; }
        public int TraineeId { get; set; }

        public double Score { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Trainee Trainee { get; set; } = null!;
    }
}