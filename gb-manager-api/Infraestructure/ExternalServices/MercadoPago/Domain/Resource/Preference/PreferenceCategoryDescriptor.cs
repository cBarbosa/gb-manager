namespace gb_manager.Infraestructure.ExternalServices.MercadoPago.Domain
{
    /// <summary>
    /// Item information related to the category.
    /// </summary>
    public class PreferenceCategoryDescriptor
    {
        /// <summary>
        /// Passenger information.
        /// </summary>
        public PreferencePassenger Passenger { get; set; }

        /// <summary>
        /// Flight information.
        /// </summary>
        public PreferenceRoute Route { get; set; }
    }
}