using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace test1Xamrin
{
    public partial class MenuItem
    {
        public string MyImageSourceFileName { get; set; }
        public object imgLogoSource
        {
            get
            {
                if (string.IsNullOrEmpty(MyImageSourceFileName) == false && MyImageSourceFileName.ToLower().StartsWith("http"))
                    return MyImageSourceFileName;
                else
                {
                    return ImageSource.FromResource("ChatBot.Resources.Logo.png");

                }

                //return ImageSource.FromResource(MyImageSourceFileName);
            }
        }

        /*
        public string MyImgBtnUnoSourceFileName { get; set; }
        public object imgBtnUnoSource
        {
            get
            {
                if (string.IsNullOrEmpty(MyImgBtnUnoSourceFileName) == false && MyImgBtnUnoSourceFileName.ToLower().StartsWith("http"))
                    return MyImgBtnUnoSourceFileName;
                else
                    return ImageSource.FromResource("ChatBot.Resources.PencilIcon.png");
               
            }
        }

        public string MyImgBtnDueSourceFileName { get; set; }
        public object imgBtnDueSource
        {
            get
            {
                if (string.IsNullOrEmpty(MyImgBtnDueSourceFileName) == false && MyImgBtnDueSourceFileName.ToLower().StartsWith("http"))
                    return MyImgBtnDueSourceFileName;
                else
                    return ImageSource.FromResource("ChatBot.Resources.CheckIcon.png");
              
            }
        }

        public string MyImgBtnTreSourceFileName { get; set; }
        public object imgBtnTreSource
        {
            get
            {
                if (string.IsNullOrEmpty(MyImgBtnTreSourceFileName) == false && MyImgBtnTreSourceFileName.ToLower().StartsWith("http"))
                    return MyImgBtnTreSourceFileName;
                else
                    return ImageSource.FromResource("ChatBot.Resources.CloseIcon.png");
             
            }
        }
*/

        public ICommand cmdbtnRegistrazione { set; get; }

        public ICommand cmdbtnRichiestaFattura { set; get; }

        public ICommand cmdbtnElencoRichieste { set; get; }

        public ICommand cmdbtnChatBot { set; get; } //20190122

        public ICommand cmdbtnChiudi { set; get; }
    }
}
