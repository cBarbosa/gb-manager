using System;

namespace gb_manager.Domain.Models
{
    public class Grade
    {
        public int? Id { get; set; }
        public int WeekDay { get; set; }
        public string Start { get; set; }
        public string Finish { get; set; }
        public virtual string WeekName => WeekDay switch
        {
            1 => "domingo",
            2 => "segunda-feira",
            3 => "terça-feira",
            4 => "quarta-feira",
            5 => "quinta-feira",
            6 => "sexta-feira",
            7 => "sábado",
            _ => "indefinido",
        };
    }
}