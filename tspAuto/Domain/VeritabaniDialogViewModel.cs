using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tspAuto.Domain
{
    class VeritabaniDialogViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _first_option;
        private string _second_option;
        private string _first_tooltip;
        private string _second_tooltip;

        public VeritabaniDialogViewModel(string name, string first_option, string second_option, string first_tooltip = "", string second_tooltip = "")
        {
            _name = name;
            _first_option = first_option;
            _second_option = second_option;
            _first_tooltip = first_tooltip;
            _second_tooltip = second_tooltip;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                this.MutateVerbose(ref _name, value, RaisePropertyChanged());
            }
        }

        public string FirstOption
        {
            get { return _first_option; }
            set
            {
                this.MutateVerbose(ref _first_option, value, RaisePropertyChanged());
            }
        }

        public string SecondOption
        {
            get { return _second_option; }
            set
            {
                this.MutateVerbose(ref _second_option, value, RaisePropertyChanged());
            }
        }

        public string FirstTooltip
        {
            get { return _first_tooltip; }
            set
            {
                this.MutateVerbose(ref _first_tooltip, value, RaisePropertyChanged());
            }
        }

        public string SecondTooltip
        {
            get { return _second_tooltip; }
            set
            {
                this.MutateVerbose(ref _second_tooltip, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
