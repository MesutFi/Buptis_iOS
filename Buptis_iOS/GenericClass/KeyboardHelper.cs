using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;

namespace Buptis_iOS.GenericClass
{
    class KeyboardHelper
    {

        #region Tanımlamalar
        private NSObject _didShowNotificationObserver;
        private NSObject _willHideNotificationObserver;
        private UIView activeTextFieldView;
        private nfloat amountToScroll = 0.0f;
        private nfloat alreadyScrolledAmount = 0.0f;
        private nfloat bottomOfTheActiveTextField = 0.0f;
        private nfloat offsetBetweenKeybordAndTextField = 10.0f;
        private bool isMoveRequired = false;
        #endregion

        private UIView TextViewHaznr;
        private UIViewController BaseController;
        public KeyboardHelper(UIView uIViewaznesi, UIViewController BaseControllerr)
        {
            TextViewHaznr = uIViewaznesi;
            BaseController = BaseControllerr;
            MoveUIViewUpHandling();
        }


        private void KeyBoardDidShow(NSNotification notification)
        {
            // get the keyboard size
            CoreGraphics.CGRect notificationBounds = UIKeyboard.BoundsFromNotification(notification);

            // Find what opened the keyboard
            foreach (UIView view in TextViewHaznr.Subviews)
            {
                if (view.IsFirstResponder)
                    activeTextFieldView = view;
            }

            // Bottom of the controller = initial position + height + offset
            bottomOfTheActiveTextField = (activeTextFieldView.Frame.Y + activeTextFieldView.Frame.Height + offsetBetweenKeybordAndTextField);

            // Calculate how far we need to scroll
            amountToScroll = (notificationBounds.Height - (TextViewHaznr.Frame.Size.Height - bottomOfTheActiveTextField));

            // Perform the scrolling
            if (amountToScroll > 0)
            {
                bottomOfTheActiveTextField -= alreadyScrolledAmount;
                amountToScroll = (notificationBounds.Height - (TextViewHaznr.Frame.Size.Height - bottomOfTheActiveTextField));
                alreadyScrolledAmount += amountToScroll;
                isMoveRequired = true;
                ScrollTheView(isMoveRequired);
            }
            else
            {
                isMoveRequired = false;
            }

        }
        private void MoveUIViewUpHandling()
        {
            #region Move UI View Up Handling
            // Keyboard popup
            _didShowNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.DidShowNotification, KeyBoardDidShow, BaseController);

            // Keyboard Down
            _willHideNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.WillHideNotification, KeyBoardWillHide, BaseController);
            #endregion
        }
        public void KeyboardViewWillAppear()
        {
            _didShowNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyBoardDidShow);

            _willHideNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardWillHide);
        }
        public void KeyboardViewWillDisappear()
        {
            if (_didShowNotificationObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_didShowNotificationObserver);
            }

            if (_willHideNotificationObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_willHideNotificationObserver);
            }
        }
        private void KeyBoardWillHide(NSNotification notification)
        {
            bool wasViewMoved = !isMoveRequired;
            if (isMoveRequired) { ScrollTheView(wasViewMoved); }
        }

        private void ScrollTheView(bool move)
        {

            // scroll the view up or down
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);

            CoreGraphics.CGRect frame = TextViewHaznr.Frame;

            if (move)
            {
                frame.Y -= amountToScroll;
            }
            else
            {
                frame.Y += alreadyScrolledAmount;
                amountToScroll = 0;
                alreadyScrolledAmount = 0;
            }

            TextViewHaznr.Frame = frame;
            UIView.CommitAnimations();
        }
    }
}