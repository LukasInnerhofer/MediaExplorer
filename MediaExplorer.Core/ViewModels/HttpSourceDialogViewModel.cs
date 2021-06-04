using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MediaExplorer.Core.ViewModels
{
    public class HttpSourceDialogViewModel : MvxViewModel<object, List<Tuple<string, MemoryStream>>>
    {
        private class Job : INotifyPropertyChanged
        {
            public string Url { get; private set; }
            public MemoryStream Stream { get; private set; }
            private Thread _thread;

            public event PropertyChangedEventHandler PropertyChanged;
            public event EventHandler DoneChanged;

            private bool _done;
            public bool Done 
            { 
                get
                {
                    return _done;
                }
                private set
                {
                    _done = value;
                    RaisePropertyChanged();
                }
            }

            public Job(string url)
            {
                Url = url;
                Stream = new MemoryStream();
                _thread = new Thread(new ThreadStart(ReadStream));
                _thread.Start();
                Done = false;
            }

            ~Job()
            {
                _thread.Join();
            }

            public void Cancel()
            {
                _thread.Abort();
            }

            private void ReadStream()
            {
                try
                {
                    WebClient client = new WebClient();
                    client.OpenRead(Url).CopyTo(Stream);
                    Stream.Seek(0, SeekOrigin.Begin);
                    Done = true;
                }
                catch(ThreadAbortException)
                {

                }
            }

            private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                if(propertyName == nameof(Done))
                {
                    DoneChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private List<Job> _jobs;

        private string _newUrl;
        public string NewUrl
        {
            get
            {
                return _newUrl;
            }
            set
            {
                SetProperty(ref _newUrl, value);
                AddUrlCommand.RaiseCanExecuteChanged();
            }
        }

        private IMvxCommand _addUrlCommand;
        public IMvxCommand AddUrlCommand =>
            _addUrlCommand ?? (_addUrlCommand = new MvxCommand(AddUrl, AddUrlCanExecute));

        private IMvxCommand _okCommand;
        public IMvxCommand OkCommand =>
            _okCommand ?? (_okCommand = new MvxCommand(Ok, OkCanExecute));

        private IMvxCommand _cancelCommand;
        public IMvxCommand CancelCommand => 
            _cancelCommand ?? (_cancelCommand = new MvxCommand(Cancel));

        public MvxObservableCollection<HttpSourceViewModel> HttpSources { get; private set; }

        public HttpSourceDialogViewModel()
        {
            _jobs = new List<Job>();
            NewUrl = string.Empty;
            HttpSources = new MvxObservableCollection<HttpSourceViewModel>();
            HttpSources.CollectionChanged += OnHttpSourcesChanged;
        }

        public override void Prepare(object parameter)
        {

        }

        private void AddUrl()
        {
            HttpSources.Add(new HttpSourceViewModel(NewUrl));
            NewUrl = string.Empty;
        }

        private bool AddUrlCanExecute()
        {
            return NewUrl != string.Empty && !_jobs.Any(x => x.Url == NewUrl);
        }

        private void Ok()
        {
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(
                this,
                _jobs.Select(x => new Tuple<string, MemoryStream>(HttpSources.Where(y => y.Url == x.Url).Single().FileExtension, x.Stream)).ToList());
        }

        private bool OkCanExecute()
        {
            foreach(var job in _jobs)
            {
                if (!job.Done)
                {
                    return false;
                }
            }
            return true;
        }

        private void Cancel()
        {
            foreach(var job in _jobs)
            {
                if(!job.Done)
                {
                    job.Cancel();
                }
            }
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(this, new List<Tuple<string, MemoryStream>>());
        }

        private void OnHttpSourcesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(HttpSourceViewModel httpSource in e.NewItems)
                {
                    var job = new Job(httpSource.Url);
                    job.DoneChanged += delegate (object _, EventArgs __) { OkCommand.RaiseCanExecuteChanged(); };
                    _jobs.Add(job);
                }
            }
            if(e.OldItems != null)
            {
                foreach (HttpSourceViewModel httpSource in e.OldItems)
                {
                    _jobs.RemoveAt(_jobs.FindIndex(x => x.Url == httpSource.Url));
                }
            }
        }
    }
}
