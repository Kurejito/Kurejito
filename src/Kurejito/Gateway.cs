namespace Kurejito
{
    public abstract class Gateway
    {
        public abstract GatewayResult Purchase(Money money, CreditCard creditCard, Options options = null);
        public abstract GatewayResult Authorise(Money money, CreditCard creditCard, Options options = null);
        public abstract GatewayResult Capture(Money money, Identification identification, Options options = null);
        public abstract GatewayResult Void(Money money, Identification identification, Options options = null);
        public abstract GatewayResult Credit(Money money, Identification identification, Options options = null);
    }

    public class GatewayResult
    {
    }
}