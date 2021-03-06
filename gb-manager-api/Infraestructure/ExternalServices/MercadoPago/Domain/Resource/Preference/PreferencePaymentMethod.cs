namespace gb_manager.Infraestructure.ExternalServices.MercadoPago.Domain
{
    /// <summary>
    /// Payment method information from <see cref="Preference"/>
    /// </summary>
    public class PreferencePaymentMethod
    {
        /// <summary>
        /// Payment method ID.
        /// </summary>
        public string Id { get; set; }
    }
}