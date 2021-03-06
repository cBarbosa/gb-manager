using System.Collections.Generic;

namespace gb_manager.Infraestructure.ExternalServices.MercadoPago.Domain
{
    /// <summary>
    /// Shipments information from <see cref="Preference"/>.
    /// </summary>
    public class PreferenceShipments
    {
        /// <summary>
        /// Shipment mode.
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// The payer have the option to pick up the shipment in your store (mode:me2 only).
        /// </summary>
        public bool? LocalPickup { get; set; }

        /// <summary>
        /// Dimensions of the shipment in cm x cm x cm, gr (mode:me2 only).
        /// </summary>
        public string Dimensions { get; set; }

        /// <summary>
        /// Select default shipping method in checkout (mode:me2 only).
        /// </summary>
        public string DefaultShippingMethod { get; set; }

        /// <summary>
        /// Offer a shipping method as free shipping (mode:me2 only).
        /// </summary>
        public IList<PreferenceFreeMethod> FreeMethods { get; set; }

        /// <summary>
        /// Shipment cost (mode:custom only).
        /// </summary>
        public decimal? Cost { get; set; }

        /// <summary>
        /// Free shipping for mode:custom.
        /// </summary>
        public bool? FreeShipping { get; set; }

        /// <summary>
        /// Shipping address.
        /// </summary>
        public PreferenceReceiverAddress ReceiverAddress { get; set; }

        /// <summary>
        /// If use express shipment.
        /// </summary>
        public bool? ExpressShipment { get; set; }
    }
}