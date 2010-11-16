using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kurejito.Gateways
{
    public class GoogleCheckoutGateway : Gateway
    {
        public override GatewayResult Purchase(Money money, CreditCard creditCard, Options options)
        {
            throw new NotImplementedException();
        }

        public override GatewayResult Authorise(Money money, CreditCard creditCard, Options options)
        {
            throw new NotImplementedException();
        }

        public override GatewayResult Capture(Money money, Identification identification, Options options)
        {
            throw new NotImplementedException();
        }

        public override GatewayResult Void(Money money, Identification identification, Options options)
        {
            throw new NotImplementedException();
        }

        public override GatewayResult Credit(Money money, Identification identification, Options options)
        {
            throw new NotImplementedException();
        }
    }
}
