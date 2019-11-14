using System;
using System.Collections.Generic;
using System.Linq;

using UIKit;
using Foundation;
using StoreKit;

//using SharedCode;

namespace Buptis_iOS.GenericClass
{
	public class InAppPurchaseManager : PurchaseManager
	{
		protected override void CompleteTransaction (string productId)
		{

		}

		public override void RestoreTransaction (SKPaymentTransaction transaction)
		{
			//throw new InvalidProgramException ("Can't restore transaction");
		}
	}
}
