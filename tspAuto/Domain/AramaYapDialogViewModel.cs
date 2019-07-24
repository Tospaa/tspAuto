using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace tspAuto.Domain
{
    class AramaYapDialogViewModel : INotifyPropertyChanged
    {
        private List<object> _item = new List<object>();

        public AramaYapDialogViewModel(object item)
        {
            _item.Add(item);
        }

        public List<object> Item
        {
            get { return _item; }
            set
            {
                this.MutateVerbose(ref _item, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
