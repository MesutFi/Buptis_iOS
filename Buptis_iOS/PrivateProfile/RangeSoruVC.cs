using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using static Buptis_iOS.ProfilSorulariBaseVC;

namespace Buptis_iOS
{
    public partial class RangeSoruVC : UIViewController
    {
        public QuestionDTO GelenSoru;
        UserAnswersDTO ApiyeGidecekCevap = null;
        public List<UserAnswersDTO> userAnswer = new List<UserAnswersDTO>();
        
        bool Actinmi = false;
        public RangeSoruVC (IntPtr handle) : base (handle)
        {
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (!Actinmi)
            {
                this.View.BackgroundColor = UIColor.Clear;
                Hazne2.BackgroundColor = UIColor.Clear;
                SoruLabel.TextColor = UIColor.White;
                SliderControll.SetThumbImage(UIImage.FromBundle("Images/sliderthumpimg.png"), UIControlState.Normal & UIControlState.Highlighted);
                SliderControll.Highlighted = false;
                SliderControll.MaxValue = 255;
                SliderControll.MinValue = 0;
                CountLabel.Text = "0";
                Actinmi = true;
                CevapYansit();
            }

        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            SoruLabel.Text = GelenSoru.name;
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SliderControll.ValueChanged += SliderControll_ValueChanged;
        }

        private void SliderControll_ValueChanged(object sender, EventArgs e)
        {
            CountLabel.Text = Math.Round(SliderControll.Value, 0).ToString();
        }

        public UserAnswersDTO GetSelectedAnswer()
        {
            if (CountLabel.Text == "0")
            {
                ApiyeGidecekCevap = null;
            }
            else
            {
                var durum = userAnswer.FindAll(item => item.questionId == GelenSoru.id);
                if (durum.Count > 0)
                {
                    ApiyeGidecekCevap = new UserAnswersDTO()
                    {
                        questionId = GelenSoru.id,
                        option = CountLabel.Text,
                        id = durum[0].id
                    };
                }
                else
                {
                    ApiyeGidecekCevap = new UserAnswersDTO()
                    {
                        questionId = GelenSoru.id,
                        option = CountLabel.Text,
                    };
                }
            }
            return ApiyeGidecekCevap;
        }

        void CevapYansit()
        {
            var durum = userAnswer.FindAll(item => item.questionId == GelenSoru.id);
            if (durum.Count > 0)
            {

                SliderControll.SetValue((int)(Convert.ToInt32(durum[durum.Count - 1].option) / 2.5f),false);
                CountLabel.Text = durum[durum.Count - 1].option;
            }

        }
    }
}