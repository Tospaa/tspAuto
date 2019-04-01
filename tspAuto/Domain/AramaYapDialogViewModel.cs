using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;

namespace tspAuto.Domain
{
    class AramaYapDialogViewModel : INotifyPropertyChanged
    {
        private DataView _dataView;

        public AramaYapDialogViewModel(string tablo, long id)
        {
            DataSet dataSet = new DataSet();

            MethodPack.VeritabaniKodBlogu((con) => {
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter($"SELECT * FROM {tablo} WHERE ID={id}", con))
                {
                    dataAdapter.Fill(dataSet);
                }

                _dataView = dataSet.Tables[0].DefaultView;
            });
        }

        public DataView DataView
        {
            get { return _dataView; }
            set
            {
                this.MutateVerbose(ref _dataView, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
