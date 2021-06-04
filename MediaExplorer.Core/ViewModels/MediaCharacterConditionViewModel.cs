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
    public class MediaCharacterConditionViewModel : MvxViewModel
    {
        public Condition Cond { get; private set; }

        private MvxObservableCollection<MediaCharacterConditionViewModel> _conditions;
        public MvxObservableCollection<MediaCharacterConditionViewModel> Conditions
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

        private MediaCharacterViewModel _character;
        public MediaCharacterViewModel Character 
        { 
            get { return _character; }
            private set { SetProperty(ref _character, value); }
        }

        private IMvxCommand _addConditionCommand;
        public IMvxCommand AddConditionCommand =>
            _addConditionCommand ?? (_addConditionCommand = new MvxCommand(AddCondition, AddConditionCanExecute));

        private IMvxCommand _deleteCommand;
        public IMvxCommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new MvxCommand(Delete, DeleteCanExecute));

        public event EventHandler Deleted;

        public MediaCharacterConditionViewModel(Condition condition, EventHandler deleted = null)
        {
            Cond = condition;
            Cond.CreateObject = new Func<object>(() =>
            {
                var character = new MediaCharacter();
                Character = new MediaCharacterViewModel(character);
                return character;
            });
            if (Cond.Object != null)
            {
                Character = new MediaCharacterViewModel(Cond.Object as MediaCharacter, false);
            }
            else
            {
                Character = new MediaCharacterViewModel(new MediaCharacter(string.Empty));
            }
            ((INotifyCollectionChanged)Cond.Conditions).CollectionChanged += ConditionsChanged;
            Conditions = new MvxObservableCollection<MediaCharacterConditionViewModel>();
           
            if (deleted != null)
            {
                Deleted += deleted;
            }
        }

        private void ConditionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Condition condition in e.NewItems)
                {
                    Conditions.Add(new MediaCharacterConditionViewModel(condition, ConditionDeleted));
                }
            }
            if (e.OldItems != null)
            {
                foreach (Condition condition in e.OldItems)
                {
                    Conditions.RemoveAt(Conditions.IndexOf(Conditions.Where(x => x.Cond == condition).Single()));
                }
            }
        }

        private void AddCondition()
        {
            Cond.Conditions.Add(new Condition(() => new MediaCharacter(string.Empty)));
        }

        private bool AddConditionCanExecute()
        {
            Condition.Operation operation = Condition.OperationNameMap[SelectedOperation];
            return operation != Condition.Operation.None && operation != Condition.Operation.Not;
        }

        private void Delete()
        {
            Deleted?.Invoke(this, new EventArgs());
        }

        private bool DeleteCanExecute()
        {
            return Deleted != null;
        }

        private void ConditionDeleted(object sender, EventArgs e)
        {
            Cond.Conditions.Remove((sender as MediaCharacterConditionViewModel).Cond);
            Conditions.Remove(sender as MediaCharacterConditionViewModel);
        }
    }
}
