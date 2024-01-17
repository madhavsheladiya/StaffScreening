using System.ComponentModel.DataAnnotations;

namespace StaffScreening.Models
{
    public class ScreeningQuestionnaire
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Assuming you have some user identifier
        [Required(ErrorMessage = "Answering this question is required.")]
        public bool? Question1 { get; set; } // Symptoms question 1
        [Required(ErrorMessage = "Answering this question is required.")]
        public bool? Question2 { get; set; } // Symptoms question 2
        [Required(ErrorMessage = "Answering this question is required.")]
        public bool? Question3 { get; set; } // Symptoms question 3
        public DateTime DateCompleted { get; set; } // Timestamp of completion
        public bool PassedScreening { get; set; }
    }

}
