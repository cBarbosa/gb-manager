namespace gb_manager.Infraestructure.ExternalServices.MercadoPago.Domain
{
    /// <summary>
    /// Shipping address.
    /// </summary>
    public class PreferenceReceiverAddress
    {
        /// <summary>
        /// Country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Floor.
        /// </summary>
        public string Floor { get; set; }

        /// <summary>
        /// Apartment.
        /// </summary>
        public string Apartment { get; set; }
    }
}