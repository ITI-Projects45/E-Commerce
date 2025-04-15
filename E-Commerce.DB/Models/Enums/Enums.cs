namespace E_Commerce.Models.Enums
{
    public class Enums
    {

        public enum OrderStatus {
            pending=1,
                shipped=2,
                delivered=4,
                cancelled=8

        }
        public enum PaymentMethod {
            card=1,
            paypal=2,
            cod=4
        }
    }
}
