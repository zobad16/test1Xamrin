using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace xUtilityPCL
{
    public class __MyBindableCollection<BaseModel> : ObservableCollection<BaseModel>, INotifyPropertyChanged
    {
        #region "implementazione interfaccia INotifyPropertyChanged"

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region "Property Title"

        private string _Title { set; get; }

        public string Title
        {
            get { return _Title; }
            set
            {
                if (value.Equals(_Title, StringComparison.Ordinal))
                {
                    // Nothing to do - the value hasn't changed;
                    return;
                }
                _Title = value;
                OnPropertyChanged();
            }
        }



        #endregion

        public __MyBindableCollection(string _title)
        {
            Title = _title;
        }
    }


    public class MyObservableCollection<T> : ObservableCollection<T>
    {
        public string Title { get; set; }

        public MyObservableCollection(List<T> list, string _title)
            : base(list)
        {
            this.Title = _title;
        }

        public MyObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

    }


    public class ByteArrayJson
    {
        public ByteArrayJson()
        {
            Images = new List<Image>();
        }

        public List<Image> Images { get; set; }

        public string CityName { get; set; }

        public class Image
        {
            public byte[] ImageBytes { get; set; }

            public string NomeFile { get; set; }
        }

    }

    //20170701 inizio
    public class ByteArrayOCRJson
    {
        public ByteArrayOCRJson()
        {
            Images = new List<ImageOCR>();
        }

        public List<ImageOCR> Images { get; set; }

        public string K_CompanyCod { get; set; }
        public string K_Employee1Matricola { get; set; }
        public string K_DeviceESN { get; set; }
        public string K_Employee1Description { get; set; }
        public string K_Street1Name { get; set; }
        public decimal num_lat { get; set; }
        public decimal num_long { get; set; }
        public decimal num_alt { get; set; }


        public class ImageOCR
        {
            public byte[] ImageBytes { get; set; }

            public string NomeFile { get; set; }

            public string K_Base64 { get; set; }
        }

    }
    //20170701 fine





}

