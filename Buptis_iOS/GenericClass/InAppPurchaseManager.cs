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
            //#region Kredi
            //if (productId == KrediYukleBaseVC.Kredi_200_ID)
            //{
            //    //Satýn Alma Ýþlemi Baþarýlý Þimdi Ne yapacak
            //}
            //else if (productId == KrediYukleBaseVC.Kredi_500_ID)
            //{
            //    //Satýn Alma Ýþlemi Baþarýlý Þimdi Ne yapacak
            //}
            //else if (productId == KrediYukleBaseVC.Kredi_1000_ID)
            //{
            //    //Satýn Alma Ýþlemi Baþarýlý Þimdi Ne yapacak
            //}
            //else if (productId == KrediYukleBaseVC.Kredi_2000_ID)
            //{
            //    //Satýn Alma Ýþlemi Baþarýlý Þimdi Ne yapacak
            //}
            //#endregion
            //else
            //{
            //    Console.WriteLine("Shouldn't happen, there are only two products");
            //}

		}

		public override void RestoreTransaction (SKPaymentTransaction transaction)
		{
			throw new InvalidProgramException ("Can't restore transaction");
		}
	}
}
