using System;

namespace gb_manager.Domain.Models
{
    public class Installment
    {
        public int? Id { get; set; }
        public int? ContractId { get; set; }
        public Guid? RecordId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? Receipt { get; set; }
        public string Type { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual Preference Preference { get; set; }
    }

    public enum InstallmentType
    {
        Boleto = 'B',
        Dinheiro = 'D',
        Cartao = 'C',
        Pix = 'P',
        Transferencia = 'T'
    }
}