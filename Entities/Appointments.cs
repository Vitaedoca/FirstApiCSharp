    namespace API.Entities;

    public class Appointments
    {
        public Guid Id { get; set; }
        
        public Guid ClientId { get; set; }
        public User Client { get; set; }
        
        public Guid ProfessionalId { get; set; }
        public User Professional { get; set; }
        
        public Guid ServiceId { get; set; } 
        public Service Service { get; set; }
        
        public DateTime Date { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = "Agendado";
    }