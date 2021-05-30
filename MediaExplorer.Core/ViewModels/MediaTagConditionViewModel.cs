using MediaExplorer.Core.Models;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.ViewModels
{
    class MediaTagConditionViewModel : MvxViewModel
    {
        private Condition _condition;

        private MediaTagViewModel _tag;
        public MediaTagViewModel Tag
        {
            get { return _tag; }
            set { SetProperty(ref _tag, value); }
        }

        public MvxObservableCollection<MediaTagConditionViewModel> Conditions;

        public MediaTagConditionViewModel(Condition condition)
        {
            _condition = condition;
        }
    }
}
