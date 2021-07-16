namespace gb_manager.Infraestructure.ExternalServices.MercadoPago.Domain
{
    /// <summary>
    /// Personal identification.
    /// </summary>
    public class Identification
    {
        /// <summary>
        /// Identification type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Identification number.
        /// </summary>
        public string Number { get; set; }
    }
}