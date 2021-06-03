using MediaExplorer.Core.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MediaExplorer.Core.ViewModels
{
    public class MediaTagConditionViewModel : MvxViewModel
    {
        public Condition Cond { get; private set; }

        private MediaTagViewModel _tag;
        public MediaTagViewModel Tag
        {
            get { return _tag; }
            set { SetProperty(ref _tag, value); }
        }

        private MvxObservableCollection<MediaTagConditionViewModel> _conditions;
        public MvxObservableCollection<MediaTagConditionViewModel> Conditions
        {
            get { return _conditions; }
            set { SetProperty(ref _conditions, value); }
        }

        public MvxObservableCollection<string> Operations => new MvxObservableCollection<string>(Condition.OperationNames);

        private string _selectedOperation;
        public string SelectedOperation
        {
            get { return _selectedOperation; }
            set 
            { 
                SetProperty(ref _selectedOperation, value);
                Cond.Op = Condition.OperationNameMap[_selectedOperation];
                AddConditionCommand.RaiseCanExecuteChanged();
            }
        }

        private string _tagText;
        public string TagText
        {
            get { return _tagText; }
            set 
            {
                SetProperty(ref _tagText, value);
                ((MediaTag)Cond.Object).Text = _tagText;
            }
        }

        private IMvxCommand _addConditionCommand;
        public IMvxCommand AddConditionCommand =>
            _addConditionCommand ?? (_addConditionCommand = new MvxCommand(AddCondition, AddConditionCanExecute));

        public MediaTagConditionViewModel(Condition condition)
        {
            Cond = condition;
            ((INotifyCollectionChanged)Cond.Conditions).CollectionChanged += ConditionsChanged;
            Conditions = new MvxObservableCollection<MediaTagConditionViewModel>();
            foreach(Condition c in Cond.Conditions)
            {
                Conditions.Add(new MediaTagConditionViewModel(c));
            }
        }

        private void ConditionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(Condition condition in e.NewItems)
                {
                    Conditions.Add(new MediaTagConditionViewModel(condition));
                }
            }
            if(e.OldItems != null)
            {
                foreach(Condition condition in e.OldItems)
                {
                    Conditions.RemoveAt(Conditions.IndexOf(Conditions.Where(x => x.Cond == condition).Single()));
                }
            }
        }

        private void AddCondition()
        {
            Cond.Conditions.Add(new Condition(new MediaTag(string.Empty)));
        }

        private bool AddConditionCanExecute()
        {
            Condition.Operation operation = Condition.OperationNameMap[SelectedOperation];
            return  operation != Condition.Operation.None && operation != Condition.Operation.Not;
        }
    }
}
