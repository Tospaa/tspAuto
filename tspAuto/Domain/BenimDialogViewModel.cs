using System;
using System.ComponentModel;

namespace tspAuto.Domain
{
    class BenimDialogViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _first_option;
        private string _second_option;
        private string _first_tooltip;
        private string _second_tooltip;

        public BenimDialogViewModel(string name, string first_option, string second_option, string first_tooltip = "", string second_tooltip = "")
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

        public bool FirstTooltipIsEnabled
        {
            get { return FirstTooltip != ""; }
        }

        public bool SecondTooltipIsEnabled
        {
            get { return SecondTooltip != ""; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
