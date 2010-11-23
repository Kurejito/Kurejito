﻿namespace Kurejito.Gateways.SagePay.VspDirect {

	///<summary>
	///</summary>
	public enum GatewayMode {
		/// <summary>Specifies that transactions should be sent to the SagePay simulator service.</summary>
		Simulator,
		/// <summary>Specifies that transactions should be sent to the SagePay TEST service.</summary>
		Test,
		/// <summary>Specifies that transactions should be sent to the SagePay LIVE service.</summary>
		Live
	}
}
