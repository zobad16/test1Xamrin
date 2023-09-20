using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.ConversationalUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xUtilityPCL;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SabicomChatView : ContentView
    {

        private SabicomRepeatBOTService botService { get; set; }
        private Author botAuthor { get; set; }
        public ChatEntry chatEntry { get; set; }
        public string LastMessage { get; set; }
        public FEC_InvoiceRequests drInvoiceRequest { get; set; }

        public void settaTypingIndicator()
        {
            if (typingIndicator.Authors.Contains(this.botAuthor) == false)
                typingIndicator.Authors.Add(this.botAuthor);
        }




        public SabicomChatView()
        {
            InitializeComponent();
            drInvoiceRequest = new FEC_InvoiceRequests();
            this.botService = new SabicomRepeatBOTService();
            this.botService.AttachOnReceiveMessage(this.OnBotMessageReceived);


            //chat.InputAreaBackgroundColor = Color.Yellow;



            //var ct = chat.ControlTemplate.CreateContent();

            var child1 = (chat as TemplatedView).Children;
            //var child1 = (ct as Grid).Children;

            foreach (var t in child1)
            {
                if (t is Grid)
                {
                    var internalGrid = t as Grid;
                    foreach (var t2 in internalGrid.Children)
                    {
                        if (t2 is Grid)
                        {
                            var internalGrid2 = t2 as Grid;
                            foreach (var t3 in internalGrid2.Children)
                            {
                                if (t3 is ChatEntry)
                                {
                                    //var chatEntry = t3 as ChatEntry;
                                    chatEntry = t3 as ChatEntry;
                                    //chatEntry.BackgroundColor = Color.Red;
                                    chatEntry.InputTransparent = false;
                                    chatEntry.WatermarkText = "Digita testo da inviare";
                                    //chatEntry.Keyboard = Keyboard.Numeric;
                                    chatEntry.TextChanged += (object sender, TextChangedEventArgs e) =>
                                    {
                                        if (this.LastMessage.Contains(App.K_Digita_Importo))
                                        {
                                            var ci = System.Globalization.CultureInfo.CurrentCulture;
                                            var decimalSeparator = ci.NumberFormat.CurrencyDecimalSeparator;
                                            var decimalSeparatorNotAllowed = "";
                                            if (decimalSeparator == ".")
                                                decimalSeparatorNotAllowed = ",";
                                            else
                                                decimalSeparatorNotAllowed = ".";
                                            System.Diagnostics.Debug.WriteLine("chatEntry_TextChanged " + e.NewTextValue); //20190112
                                            if (e.NewTextValue == null)
                                                return;
                                            if (e.NewTextValue.Contains(decimalSeparatorNotAllowed))
                                            {
                                                if (e.OldTextValue != null)
                                                    this.chatEntry.Text = e.OldTextValue;
                                                else
                                                    this.chatEntry.Text = "";
                                            }
                                            else
                                                this.chatEntry.Text = e.NewTextValue;
                                        }

                                    };

                                }
                            }

                        }

                    }
                }

            }



            this.botAuthor = new Author { Name = "botty", Avatar = "girl.png" };
            if (Device.RuntimePlatform == Device.iOS)
                chat.Author.Avatar = "Icon-Small.png";
            else
                chat.Author.Avatar = "icon.png";

            //typingIndicator.Authors.Add(this.botAuthor);

            ((INotifyCollectionChanged)this.chat.Items).CollectionChanged += ChatItems_CollectionChanged; ;
        }
        // << chat-getting-started-initiliaze
        // >> chat-getting-started-events
        private void ChatItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems[0] is TextMessage)
                {
                    TextMessage chatMessage = (TextMessage)e.NewItems[0];
                    if (chatMessage.Author == chat.Author)
                    {
                        if (!chatMessage.Text.StartsWith("IMG-Path:"))
                        {
                            settaTypingIndicator();
                            this.botService.SendToBot(chatMessage.Text);
                            SettadrRichiesta(chatMessage.Text);
                        }
                    }
                }
            }
        }

        private void SettadrRichiesta(string valore)
        {
            return;
            //non usato
            if (this.LastMessage.Contains(App.K_Digita_Importo))
            {
                this.drInvoiceRequest.TotalAmount = Convert.ToDecimal(valore);
            }
            if (this.LastMessage.Contains(App.K_Scegli_il_carburante))
            {
                this.drInvoiceRequest.ItemCode = valore;
            }
            if (this.LastMessage.Contains(App.K_Please_Read_the_QR))
            {
                this.drInvoiceRequest.IDCompany = Convert.ToInt32(valore);
            }
            if (this.LastMessage.Contains(App.K_Fa_Una_Foto_Ricevuta))
            {
                this.drInvoiceRequest.ReceiptPicture1 = valore;
            }
            if (this.LastMessage.Contains(App.K_Digita_ReceiptNumber))
            {
                this.drInvoiceRequest.ReceiptNumber = valore;
            }

        }



        private void OnBotMessageReceived(string message)
        {
            this.LastMessage = message;
            chatEntry.Keyboard = Keyboard.Text;

            Device.BeginInvokeOnMainThread(async () =>
            {
                TextMessage textMessage = new TextMessage();
                textMessage.Data = message;
                textMessage.Author = this.botAuthor;
                textMessage.Text = message;

                typingIndicator.Authors.Clear();
                //chat.Items.Add(textMessage);

                if (message == null)
                {
                    TextMessage nullMessage = new TextMessage();
                    nullMessage.Data = message;
                    nullMessage.Author = this.botAuthor;
                    nullMessage.Text = "chat Closed Unexpectedly. Try again";
                    chat.Items.Add(nullMessage);
                    await Application.Current.MainPage.DisplayAlert("Messaggio", "Chat is closed", "Ok");
                    return;
                }

                if (message.Contains(App.K_Digita_Importo))
                {
                    chatEntry.Keyboard = Keyboard.Numeric;
                    TextMessage amountMessage = new TextMessage();
                    amountMessage.Author = this.botAuthor;
                    amountMessage.Text = message;
                    chat.Items.Add(amountMessage);
                    return;
                }
                if (message.Contains(App.K_Digita_ReceiptNumber))
                {
                    TextMessage amountMessage = new TextMessage();
                    amountMessage.Author = this.botAuthor;
                    amountMessage.Text = message;
                    chat.Items.Add(amountMessage);
                    return;
                }
                if (message.Contains(App.K_Digita_Tua_PartitaIva)) //20190120
                {
                    //nota: non visualizzo il messaggio, ma mando il primo cliente con cui mi sono registrato
                    settaTypingIndicator();
                    using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                    {
                        var l = cn.Table<FEC_CustomersSystem>().ToList();
                        var resulttext = await SabicomRepeatBOTService.SendMessage2("alessandro", l[0].TaxIDNumber);

                        cn.Close();
                    }



                    return;
                }


                if (message.Contains(App.K_Scegli_il_carburante))
                {
                    var arrMessage = message.Split(new char[] { '\n', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var lMessage = new List<string>(arrMessage);
                    lMessage.RemoveAt(0); //remove header
                    lMessage.RemoveAt(0); //remove Options
                    var header = arrMessage[0];
                    ItemPickerContext context = new ItemPickerContext
                    {
                        ItemsSource = lMessage //new List<string>() { "2 days", "5 days", "7 days", "Another period" }
                    };

                    //remove all the previous picker items (start)
                    List<ChatItem> lToremove = new List<ChatItem>();

                    foreach (var ctrl in chat.Items)
                    {
                        if (ctrl is PickerItem)
                        {
                            lToremove.Add(ctrl);
                        }
                    }
                    foreach (var ctrl in lToremove)
                    {
                        chat.Items.Remove(ctrl);
                    }
                    //remove all the previous picker items (end)


                    PickerItem pickerItem = new PickerItem { Context = context, HeaderText = header };
                    chat.Items.Add(pickerItem);


                    context.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == "SelectedItem")
                        {
                            if (context.SelectedItem != null)
                            {
                                chat.Items.Remove(pickerItem);
                                chat.Items.Add(new TextMessage { Author = chat.Author, Text = "" + context.SelectedItem });
                            }
                        }
                    };

                }
                else if (message.Contains(App.K_Fa_Una_Foto_Ricevuta))
                {
                    ItemPickerContext context = new ItemPickerContext
                    {
                        ItemsSource = new List<string>() { "Browse Image", "Capture Image" }
                    };
                    PickerItem pickerItem = new PickerItem { Context = context, HeaderText = "Clica sotto per aggiungere un'immagine" };

                    chat.Items.Add(textMessage);
                    chat.Items.Add(pickerItem);
                    context.PropertyChanged += async (s, e) =>
                    {
                        if (e.PropertyName == "SelectedItem")
                        {
                            Plugin.Media.Abstractions.MediaFile file = null;
                            if (context.SelectedItem != null)
                            {
                                if (context.SelectedItem.ToString() == "Browse Image")
                                    file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                                    {
                                        PhotoSize = PhotoSize.Medium,
                                        CompressionQuality = 60

                                    });
                                else
                                {
                                    file = await CrossMedia.Current.TakePhotoAsync(
                                            new Plugin.Media.Abstractions.StoreCameraMediaOptions
                                            {
                                                SaveToAlbum = true,
                                                PhotoSize = PhotoSize.Medium,
                                                CompressionQuality = 60

                                            });

                                }
                                var myStream = file.GetStream();
                                var mystreamcont = new StreamContent(myStream);

                                //      https://docs.microsoft.com/en-us/azure/bot-service/rest-api/bot-framework-rest-direct-line-3-0-api-reference?view=azure-bot-service - 3.0#upload-send-files

                                chat.Items.Remove(pickerItem);
                                chat.Items.Add(new TextMessage { Author = chat.Author, Text = "IMG-Path:" + file.Path });

                                var mybotmsg = await SabicomRepeatBOTService.SendMessage3("Ales", "Image Upload", mystreamcont);
                                SettadrRichiesta("abc");
                            }
                        }
                    };
                }
                else if (message.Contains(App.K_Please_Upload_the_QR_image))
                {
                    ItemPickerContext context = new ItemPickerContext
                    {
                        ItemsSource = new List<string>() { "Browse QR Image", "Capture QR Image" }
                    };
                    PickerItem pickerItem = new PickerItem { Context = context, HeaderText = message };

                    chat.Items.Add(textMessage);
                    chat.Items.Add(pickerItem);
                    context.PropertyChanged += async (s, e) =>
                    {
                        if (e.PropertyName == "SelectedItem")
                        {
                            Plugin.Media.Abstractions.MediaFile file = null;
                            if (context.SelectedItem != null)
                            {
                                if (context.SelectedItem.ToString() == "Browse QR Image")
                                    file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                                    {
                                        PhotoSize = PhotoSize.Medium,
                                        CompressionQuality = 60

                                    });
                                else
                                {
                                    file = await CrossMedia.Current.TakePhotoAsync(
                                            new Plugin.Media.Abstractions.StoreCameraMediaOptions
                                            {
                                                SaveToAlbum = true,
                                                PhotoSize = PhotoSize.Medium,
                                                CompressionQuality = 60

                                            });

                                }
                                var myStream = file.GetStream();
                                var mystreamcont = new StreamContent(myStream);

                                //https://docs.microsoft.com/en-us/azure/bot-service/rest-api/bot-framework-rest-direct-line-3-0-api-reference?view=azure-bot-service - 3.0#upload-send-files

                                chat.Items.Remove(pickerItem);
                                chat.Items.Add(new TextMessage { Author = chat.Author, Text = "IMG-Path:" + file.Path });

                                var mybotmsg = await SabicomRepeatBOTService.SendMessage3("Ales", "Image Upload", mystreamcont);
                            }
                        }
                    };
                }
                else if (message.Contains(App.K_Please_Read_the_QR))
                {
                    ItemPickerContext context = new ItemPickerContext
                    {
                        ItemsSource = new List<string>() { "Scan" }
                    };
                    PickerItem pickerItem = new PickerItem { Context = context, HeaderText = message };


                    chat.Items.Add(pickerItem);
                    context.PropertyChanged += async (s, e) =>
                    {
                        if (e.PropertyName == "SelectedItem")
                        {

                            if (context.SelectedItem != null)
                            {
                                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                                //var QRNonStand = AppResources.FormClientiMessaggioQRNonStandard;

                                var result1 = await scanner.Scan();

                                if (result1 != null)
                                {
                                    settaTypingIndicator();
                                    var arrqr = result1.Text.Split("|".ToCharArray());
                                    var mybotmsg = await SabicomRepeatBOTService.SendMessage2("alessandro", arrqr[0]);
                                    SettadrRichiesta(arrqr[0]);
                                    return;


                                }




                            }
                        }
                    };
                }

                else if (message.Contains(App.K_Scegli_modalità_pagamento))
                {
                    var arrMessage = message.Split(new char[] { '\n', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var lMessage = new List<string>(arrMessage);
                    lMessage.RemoveAt(0); //remove header
                    lMessage.RemoveAt(0); //remove Options
                    var header = arrMessage[0];
                    ItemPickerContext context = new ItemPickerContext
                    {
                        ItemsSource = lMessage //new List<string>() { "2 days", "5 days", "7 days", "Another period" }
                    };

                    //remove all the previous picker items (start)
                    List<ChatItem> lToremove = new List<ChatItem>();

                    foreach (var ctrl in chat.Items)
                    {
                        if (ctrl is PickerItem)
                        {
                            lToremove.Add(ctrl);
                        }
                    }
                    foreach (var ctrl in lToremove)
                    {
                        chat.Items.Remove(ctrl);
                    }
                    //remove all the previous picker items (end)


                    PickerItem pickerItem = new PickerItem { Context = context, HeaderText = header };
                    chat.Items.Add(pickerItem);


                    context.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == "SelectedItem")
                        {
                            if (context.SelectedItem != null)
                            {
                                chat.Items.Remove(pickerItem);
                                chat.Items.Add(new TextMessage { Author = chat.Author, Text = "" + context.SelectedItem });
                            }
                        }
                    };

                }

                else if (message.Contains(App.K_ConfermaFinale))
                {
                    var arrMessage = message.Split(new char[] { '\n', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var lMessage = new List<string>(arrMessage);
                    lMessage.RemoveAt(0); //remove header
                    lMessage.RemoveAt(0); //remove Options
                    var header = arrMessage[0];
                    ItemPickerContext context = new ItemPickerContext
                    {
                        ItemsSource = lMessage //new List<string>() { "2 days", "5 days", "7 days", "Another period" }
                    };

                    //remove all the previous picker items (start)
                    List<ChatItem> lToremove = new List<ChatItem>();

                    foreach (var ctrl in chat.Items)
                    {
                        if (ctrl is PickerItem)
                        {
                            lToremove.Add(ctrl);
                        }
                    }
                    foreach (var ctrl in lToremove)
                    {
                        chat.Items.Remove(ctrl);
                    }
                    //remove all the previous picker items (end)


                    PickerItem pickerItem = new PickerItem { Context = context, HeaderText = header };
                    chat.Items.Add(pickerItem);


                    context.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == "SelectedItem")
                        {
                            if (context.SelectedItem != null)
                            {
                                chat.Items.Remove(pickerItem);
                                chat.Items.Add(new TextMessage { Author = chat.Author, Text = "" + context.SelectedItem });
                            }
                        }
                    };

                }
                else if (message.Contains(App.K_ChatChiusa))
                {
                    //chat.Items.Add(textMessage);
                    //nota: il messaggio viene già aggiunto al momenro del send del comando "cancel"
                }
                else
                {
                    var penultimate = chat.Items.Last();
                    if (penultimate is TextMessage && textMessage is TextMessage)
                    {
                        if ((penultimate as TextMessage).Text == textMessage.Text)
                        //server delay: the server replyed
                        {
                            settaTypingIndicator();
                            var resulttext = await SabicomRepeatBOTService.SendMessage2("alessandro", textMessage.Text);

                        }
                        else
                        {
                            chat.Items.Add(textMessage);
                        }
                    }
                    else
                    {
                        chat.Items.Add(textMessage);
                    }
                }
            });
        }



        // << chat-getting-started-events
    }
}