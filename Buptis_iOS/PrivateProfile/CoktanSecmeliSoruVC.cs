using Buptis_iOS.Database;
using Buptis_iOS.Web_Service;
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using UIKit;
using static Buptis_iOS.ProfilSorulariBaseVC;

namespace Buptis_iOS
{
    public partial class CoktanSecmeliSoruVC : UIViewController
    {
        #region Tanimlamalar
        List<EngelleCustomRadioItem> Noktalar = new List<EngelleCustomRadioItem>();
        List<OptionsDTO> OptionsDTOs = new List<OptionsDTO>();
        public QuestionDTO GelenSoru;
        public List<UserAnswersDTO> userAnswer = new List<UserAnswersDTO>();
        bool Actinmi = false;
        UserAnswersDTO SendDataForApi;
        #endregion
        public CoktanSecmeliSoruVC(IntPtr handle) : base(handle)
        {
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.View.BackgroundColor = UIColor.Clear;
            SoruLabel.TextColor = UIColor.White;
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!Actinmi)
            {
                SetRadioButtons();
                Actinmi = true;
            }
        }
        void SetRadioButtons()
        {
            Noktalar = new List<EngelleCustomRadioItem>();
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("answers");
            if (Donus != null)
            {
                OptionsDTOs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OptionsDTO>>(Donus.ToString());
                OptionsDTOs = OptionsDTOs.FindAll(item => item.questionId == GelenSoru.id);
                SoruLabel.Text = GelenSoru.name;
                if (OptionsDTOs.Count > 0)
                {
                    for (int i = 0; i < OptionsDTOs.Count; i++)
                    {
                        var NoktaItem = EngelleCustomRadioItem.Create(OptionsDTOs[i].option, RadioBUtonlarinTasarimlariniDuzenle, false);
                        if (i == 0)
                        {
                            NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, 47f);
                        }
                        else
                        {
                            NoktaItem.Frame = new CoreGraphics.CGRect(0, Noktalar[i - 1].Frame.Bottom, UIKit.UIScreen.MainScreen.Bounds.Width, 47f);
                        }
                        var Durum = userAnswer.FindAll(item => item.id.ToString() ==  OptionsDTOs[i].id);
                        if (Durum.Count > 0)
                        {
                            NoktaItem.isSelect = true;
                        }
                        NoktaItem.Tag = i;
                        VerticalScroll.AddSubview(NoktaItem);
                        Noktalar.Add(NoktaItem);
                       
                    }
                    VerticalScroll.ContentSize = new CoreGraphics.CGSize(UIKit.UIScreen.MainScreen.Bounds.Width, Noktalar[Noktalar.Count - 1].Frame.Bottom);
                }
            }
        }
        public UserAnswersDTO GetSelectedAnswer()
        {
            SendDataForApi = null;
            for (int i = 0; i < Noktalar.Count; i++)
            {
                if (Noktalar[i].isSelect)
                {
                    SendDataForApi = new UserAnswersDTO()
                    {
                        id = OptionsDTOs[i].id.ToString(),
                        option = OptionsDTOs[i].option,
                        questionId = OptionsDTOs[i].questionId
                    };
                    break;
                }
            }
            return SendDataForApi;
        }
       

        void RadioBUtonlarinTasarimlariniDuzenle()
        {
            for (int i = 0; i < Noktalar.Count; i++)
            {
                Noktalar[i].UzaktanErisim(false);
            }
        }

        public class OptionsDTO
        {
            public string id { get; set; }
            public string option { get; set; }
            public int questionId { get; set; }
        }
    }
}